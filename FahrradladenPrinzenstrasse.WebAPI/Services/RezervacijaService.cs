using AutoMapper;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Model;
using FahrradladenPrinzenstrasse.Model.Requests;
using FahrradladenPrinzenstrasse.WebAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public class RezervacijaService : IRezervacijaService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;
        private readonly IKorisnikService _korisnikService;

        public RezervacijaService(MyContext context, IMapper mapper, IKorisnikService korisnikService)
        {
            _context = context;
            _mapper = mapper;
            this._korisnikService = korisnikService;
        }
        public List<Rezervacija> Get(RezervacijaSearchRequest request)
        {
            var query = _context.Rezervacija
                .Where(x => x.KlijentId == _korisnikService.GetCurrentUser().Klijent.Id);

            if(request.DatumOd.HasValue)
            {
                query = query.Where(x => x.DatumRezervacije.Date >= request.DatumOd.Value.Date);
            }
            if(request.DatumDo.HasValue)
            {
                query = query.Where(x => x.DatumRezervacije.Date <= request.DatumDo.Value.Date);
            }

            query = query
                .OrderByDescending(x => x.RezervacijaId);

            var result = _mapper.Map<List<Model.Rezervacija>>(query.ToList());

            foreach (var item in result)
            {
                if (_context.RezervacijaIznajmljenaBicikla.Any(x => x.RezervacijaId == item.RezervacijaId))
                {
                    item.IsTerminRezervacija = true;
                }
                else if (_context.RezervacijaServis.Any(x => x.RezervacijaId == item.RezervacijaId))
                {
                    item.IsServisRezervacija = true;
                }
            }

            return result;
        }

        public RezervacijaDetalji GetById(int Id)
        {
            var entity = _context.Rezervacija.Where(x => x.RezervacijaId == Id)
                .Where(x => x.KlijentId == _korisnikService.GetCurrentUser().KorisnikID)
                .Include("RezervacijaIznajmljenaBicikla.BiciklStanje.Bicikl.OcjenaProizvoda")
                .Include("RezervacijaIznajmljenaBicikla.BiciklStanje.Bicikl.Model.Proizvodjac")
                .Include("RezervacijaProdajaBicikla.BiciklStanje.Bicikl.Model.Proizvodjac")
                .Include("RezervacijaProdajaBicikla.BiciklStanje.Bicikl.OcjenaProizvoda")
                .Include("RezervacijaProdajaDio.DioStanje.Dio")
                .Include("RezervacijaProdajaDio.DioStanje.Dio.OcjenaProizvoda")
                .Include("RezervacijaProdajaOprema.OpremaStanje.Oprema")
                .Include("RezervacijaProdajaOprema.OpremaStanje.Oprema.OcjenaProizvoda")
                .Include("RezervacijaServis.Servis")
                .Include(x => x.NacinOtpreme)
                .FirstOrDefault();

            return _mapper.Map<Model.RezervacijaDetalji>(entity);
        }

        public Rezervacija Insert(RezervacijaInsertRequest request)
        {
            var entity = _mapper.Map<Data.EntityModels.Rezervacija>(request);
            entity.KlijentId = _korisnikService.GetCurrentUser().Klijent.Id;


            if (request.ObradiIznajmljivanje)
            {
                var Stavke = GetTerminStavke(entity.KlijentId);

                foreach (var stavka in Stavke)
                {
                    double cijena = 0;
                    if (stavka.Bicikl != null)
                        cijena = stavka.Bicikl.CijenaPoDanu.Value * stavka.BrojDana;

                    entity.UkupniIznos += cijena * stavka.Kolicina;
                }

                double stopa_poreza = 17.0;
                entity.Osnovica = entity.UkupniIznos / (1 + stopa_poreza / 100);
                entity.UkupnoPoreza = entity.UkupniIznos - entity.Osnovica;

                entity.RezervacijaIznajmljenaBicikla = new List<Data.EntityModels.RezervacijaIznajmljenaBicikla>();

                // Bicikla
                foreach (var naruceno_biciklo in Stavke.ToList())
                {
                    var bicikl_stanja = _context.BiciklStanje
                        .Where(x => x.BiciklId == naruceno_biciklo.BiciklId)
                        .Where(x => x.Aktivan == true)
                        .Where(x => x.KupacId == null)
                        .Take(naruceno_biciklo.Kolicina)
                        .ToList();

                    if (bicikl_stanja.Count < naruceno_biciklo.Kolicina)
                        throw new UserException("Naručeno biciklo " + naruceno_biciklo.Bicikl.PuniNaziv + " nije dostupno u traženoj kolicini!");

                    foreach (var bicikl_na_stanju in bicikl_stanja)
                    {
                        entity.RezervacijaIznajmljenaBicikla.Add(new Data.EntityModels.RezervacijaIznajmljenaBicikla
                        {
                            BiciklStanjeId = bicikl_na_stanju.BiciklStanjeId,
                            DatumPreuzimanja = naruceno_biciklo.DatumPreuzimanja,
                            DatumVracanja = naruceno_biciklo.DatumVracanja
                        });
                    }
                }
            }
            else if (request.RezervacijaServis != null)
            {
                var DatumServisiranja = request.RezervacijaServis[0].DatumServisiranja;

                foreach (var stavka in request.RezervacijaServis)
                {
                    var servis = _context.Servis.Where(x=>x.ServisId == stavka.ServisId)
                        .Where(x => x.IsDeleted == false).FirstOrDefault();
                    if (servis is null)
                        throw new UserException("Servis nije pronađen: " + stavka.ServisId);

                    stavka.DatumServisiranja = DatumServisiranja;
                    DatumServisiranja = DatumServisiranja.AddHours(servis.Trajanje);

                    entity.UkupniIznos += servis.Cijena;
                }

                double stopa_poreza = 17.0;
                entity.Osnovica = entity.UkupniIznos / (1 + stopa_poreza / 100);
                entity.UkupnoPoreza = entity.UkupniIznos - entity.Osnovica;

                entity.RezervacijaServis = new List<Data.EntityModels.RezervacijaServis>();

                foreach (var rezervacija_servis in request.RezervacijaServis)
                {
                    var item = _mapper.Map<Data.EntityModels.RezervacijaServis>(rezervacija_servis);
                    entity.RezervacijaServis.Add(item);
                }
            }
            else

            {
                var Stavke = GetKorpaStavke(entity.KlijentId);

                foreach (var stavka in Stavke)
                {
                    double cijena = 0;
                    if (stavka.Bicikl != null)
                        cijena = stavka.Bicikl.Cijena.Value;
                    else if (stavka.Oprema != null)
                        cijena = stavka.Oprema.Cijena;
                    else if (stavka.Dio != null)
                        cijena = stavka.Dio.Cijena;

                    entity.UkupnoProizvodi += cijena * stavka.Kolicina;
                }

                double stopa_poreza = 17.0;
                entity.Postarina = 0.0;
                if (entity.NacinOtpremeId != 0)
                    entity.Postarina = _context.NacinOtpreme
                        .Where(x => x.IsDeleted == false).Where(x => x.NacinOtpremeId == entity.NacinOtpremeId).Single()?.Cijena ?? 0.0;
                entity.UkupniIznos = entity.UkupnoProizvodi;
                entity.Osnovica = entity.UkupniIznos / (1 + stopa_poreza / 100);
                entity.UkupnoPoreza = entity.UkupniIznos - entity.Osnovica;
                entity.UkupniIznos += entity.Postarina;

                entity.RezervacijaProdajaBicikla = new List<Data.EntityModels.RezervacijaProdajaBicikla>();
                entity.RezervacijaProdajaDio = new List<Data.EntityModels.RezervacijaProdajaDio>();
                entity.RezervacijaProdajaOprema = new List<Data.EntityModels.RezervacijaProdajaOprema>();

                // Bicikla
                foreach (var naruceno_biciklo in Stavke.Where(x => x.BiciklId != null).ToList())
                {
                    var bicikl_stanja = _context.BiciklStanje
                        .Where(x => x.BiciklId == naruceno_biciklo.BiciklId)
                        .Where(x => x.Aktivan == true)
                        .Where(x => x.KupacId == null)
                        .Take(naruceno_biciklo.Kolicina)
                        .ToList();

                    if (bicikl_stanja.Count < naruceno_biciklo.Kolicina)
                        throw new UserException("Naručeno biciklo " + naruceno_biciklo.Bicikl.PuniNaziv + " nije dostupno u traženoj kolicini!");

                    foreach (var bicikl_na_stanju in bicikl_stanja)
                    {
                        entity.RezervacijaProdajaBicikla.Add(new Data.EntityModels.RezervacijaProdajaBicikla
                        {
                            BiciklStanjeId = bicikl_na_stanju.BiciklStanjeId
                        });
                        bicikl_na_stanju.Aktivan = false;
                        bicikl_na_stanju.KupacId = entity.KlijentId;
                    }
                }
                // Dijelovi
                foreach (var naruceno_dio in Stavke.Where(x => x.DioId != null).ToList())
                {
                    var dio_stanja = _context.DioStanje
                        .Where(x => x.DioId == naruceno_dio.DioId)
                        .Where(x => x.Aktivan == true)
                        .Where(x => x.KupacId == null)
                        .Take(naruceno_dio.Kolicina)
                        .ToList();

                    if (dio_stanja.Count < naruceno_dio.Kolicina)
                        throw new UserException("Naručeno dio " + naruceno_dio.Dio.Naziv + " nije dostupno u traženoj kolicini!");

                    foreach (var dio_na_stanju in dio_stanja)
                    {
                        entity.RezervacijaProdajaDio.Add(new Data.EntityModels.RezervacijaProdajaDio
                        {
                            DioStanjeId = dio_na_stanju.DioStanjeId
                        });
                        dio_na_stanju.Aktivan = false;
                        dio_na_stanju.KupacId = entity.KlijentId;
                    }
                }
                // Oprema
                foreach (var narucena_oprema in Stavke.Where(x => x.OpremaId != null).ToList())
                {
                    var oprema_stanja = _context.OpremaStanje
                        .Where(x => x.OpremaId == narucena_oprema.OpremaId)
                        .Where(x => x.Aktivan == true)
                        .Where(x => x.KupacId == null)
                        .Take(narucena_oprema.Kolicina)
                        .ToList();

                    if (oprema_stanja.Count < narucena_oprema.Kolicina)
                        throw new UserException("Naručena oprema " + narucena_oprema.Oprema.Naziv + " nije dostupna u traženoj kolicini!");

                    foreach (var oprema_na_stanju in oprema_stanja)
                    {
                        entity.RezervacijaProdajaOprema.Add(new Data.EntityModels.RezervacijaProdajaOprema
                        {
                            OpremaStanjeId = oprema_na_stanju.OpremaStanjeId
                        });
                        oprema_na_stanju.Aktivan = false;
                        oprema_na_stanju.KupacId = entity.KlijentId;
                    }
                }
            }

            entity.DatumRezervacije = DateTime.Now;
            entity.StanjeRezervacije = Data.EntityModels.StanjeRezervacije.Čekanje_uplate;

            if (entity.NacinOtpremeId == 0)
                entity.NacinOtpremeId = _context.NacinOtpreme
                    .Where(x => x.IsDeleted == false).First(x => x.Cijena == 0).NacinOtpremeId;

            entity.NacinOtpreme = _context.NacinOtpreme.First(x => x.NacinOtpremeId == entity.NacinOtpremeId);

            _context.Rezervacija.Add(entity);
            _context.SaveChanges();

            if (request.ObradiIznajmljivanje)
            {
                var termin_stavke = _context.TerminStavka.Where(x => x.KlijentId == entity.KlijentId).ToList();
                foreach (var item in termin_stavke)
                {
                    _context.TerminStavka.Remove(item);
                }
            }
            else if(request.RezervacijaServis != null)
            {
                // do nothing
            }
            else
            {
                var korpa_stavke = _context.KorpaStavka.Where(x => x.KlijentId == entity.KlijentId).ToList();
                foreach (var item in korpa_stavke)
                {
                    _context.KorpaStavka.Remove(item);
                }

                if(entity.NacinPlacanja != "online")
                {
                    var zaposlenici = _context.Zaposlenik.Where(x => x.Korisnik.Aktivan == true).ToList();
                    foreach (var zaposlenik in zaposlenici)
                    {
                        var notifikacija = new Data.EntityModels.Notifikacija
                        {
                            ZaposlenikId = zaposlenik.Id,
                            Tip = Data.EntityModels.TipNotifikacije.Nova_Narudzba,
                            Rezervacija = entity,
                            DatumVrijeme = DateTime.Now
                        };
                        _context.Notifikacija.Add(notifikacija);
                    }
                }
            }
            _context.SaveChanges();

            return _mapper.Map<Model.Rezervacija>(entity);
        }

        private List<Data.EntityModels.TerminStavka> GetTerminStavke(int klijentId)
        {
            return _context.TerminStavka
                .Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.MaterijalOkvira)
                .Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.Proizvodjac)
                .Include(x => x.Bicikl).ThenInclude(x => x.StarosnaGrupa)
                .Include(x => x.Bicikl).ThenInclude(x => x.VelicinaOkvira)
                .Include(x => x.Bicikl).ThenInclude(x => x.Boja)
                .Include(x => x.Bicikl).ThenInclude(x => x.BiciklStanje)
                .Where(x => x.KlijentId == klijentId).ToList();
        }

        public Rezervacija Update(int id, RezervacijaInsertRequest request)
        {
            var entity = _context.Rezervacija.Find(id);
            _mapper.Map(request, entity);

            _context.SaveChanges();

            return _mapper.Map<Model.Rezervacija>(entity);
        }

        private List<Data.EntityModels.KorpaStavka> GetKorpaStavke(int KlijentId)
        {
            return _context.KorpaStavka
                .Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.MaterijalOkvira)
                .Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.Proizvodjac)
                .Include(x => x.Bicikl).ThenInclude(x => x.StarosnaGrupa)
                .Include(x => x.Bicikl).ThenInclude(x => x.VelicinaOkvira)
                .Include(x => x.Bicikl).ThenInclude(x => x.Boja)
                .Include(x => x.Bicikl).ThenInclude(x => x.BiciklStanje)
                .Include(x => x.Dio).ThenInclude(x => x.Proizvodjac)
                .Include(x => x.Oprema).ThenInclude(x => x.Proizvodjac)
                .Where(x => x.KlijentId == KlijentId).ToList();
        }

        public ActionResult PotvrdiUplatu(PaymentIntentConfirmRequest request)
        {
            var narudzba = _context.Rezervacija.Where(x => x.RezervacijaId == request.RezervacijaId && x.StanjeRezervacije == Data.EntityModels.StanjeRezervacije.Čekanje_uplate).FirstOrDefault();
            if (narudzba == null)
                return new JsonResult(new { error = "Narudžba nije u ispravnom stanju." });

            var narudzba_iznos = (int)narudzba.UkupniIznos * 100;

            if (narudzba_iznos == 0)
                return new JsonResult(new { error = "Iznos narudžbe je neispravan." });

            var paymentIntents = new PaymentIntentService();
            var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
            {
                Amount = narudzba_iznos,
                Currency = "usd",
                Confirm = true,
                Metadata = new Dictionary<string, string>(),
                PaymentMethod = request.PaymentMethodId
            });
            paymentIntent.Metadata["RezervacijaId"] = request.RezervacijaId.ToString();


            if (paymentIntent.Amount == narudzba_iznos && paymentIntent.Status == "succeeded")
            {
                narudzba.StanjeRezervacije = Data.EntityModels.StanjeRezervacije.U_obradi;
                if (narudzba.DatumUplate is null)
                    narudzba.DatumUplate = DateTime.Now;
                _context.SaveChanges();

                bool IsServisRezervacija = narudzba.RezervacijaServis.Any(),
                         IsTerminRezervacija = narudzba.RezervacijaIznajmljenaBicikla.Any();

                if (IsServisRezervacija || IsTerminRezervacija || narudzba.NacinPlacanja == "online")
                {
                    var zaposlenici = _context.Zaposlenik.Where(x => x.Korisnik.Aktivan == true).ToList();
                    foreach (var zaposlenik in zaposlenici)
                    {
                        var notifikacija = new Data.EntityModels.Notifikacija
                        {
                            ZaposlenikId = zaposlenik.Id,
                            Rezervacija = narudzba,
                            DatumVrijeme = DateTime.Now
                        };
                        if (IsServisRezervacija)
                            notifikacija.Tip = Data.EntityModels.TipNotifikacije.Novi_Servis;
                        else if (IsTerminRezervacija)
                            notifikacija.Tip = Data.EntityModels.TipNotifikacije.Novi_Termin;
                        else
                            notifikacija.Tip = Data.EntityModels.TipNotifikacije.Nova_Narudzba;

                        _context.Notifikacija.Add(notifikacija);
                    }
                }

                return new JsonResult(new { clientSecret = paymentIntent.ClientSecret });
            }

            return new JsonResult(new { error = paymentIntent.Status });
        }

        public bool OtkaziRezervaciju(int id)
        {
            var entity = _context.Rezervacija.Find(id);
            if (entity == null || entity.StanjeRezervacije == Data.EntityModels.StanjeRezervacije.Otkazana)
                return false;

            entity.StanjeRezervacije = Data.EntityModels.StanjeRezervacije.Otkazana;

            _context.SaveChanges();
            return true;
        }
    }
}
