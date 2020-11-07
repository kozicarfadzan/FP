using AutoMapper;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Model;
using FahrradladenPrinzenstraße.Model.Requests;
using FahrradladenPrinzenstraße.WebAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FahrradladenPrinzenstraße.WebAPI.Services
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
        public List<Rezervacija> Get()
        {
            var list = _context.Rezervacija.ToList();

            return _mapper.Map<List<Model.Rezervacija>>(list);
        }

        public Rezervacija GetById(int id)
        {
            var entity = _context.Rezervacija.Find(id);
            return _mapper.Map<Model.Rezervacija>(entity);
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
                    entity.Postarina = _context.NacinOtpreme.Where(x => x.NacinOtpremeId == entity.NacinOtpremeId).Single()?.Cijena ?? 0.0;
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
                entity.NacinOtpremeId = _context.NacinOtpreme.First(x => x.Cijena == 0).NacinOtpremeId;

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
            else
            {
                var korpa_stavke = _context.KorpaStavka.Where(x => x.KlijentId == entity.KlijentId).ToList();
                foreach (var item in korpa_stavke)
                {
                    _context.KorpaStavka.Remove(item);
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
        public bool Delete(int id)
        {
            var entity = _context.Rezervacija.Find(id);
            if (entity != null)
            {
                _context.Remove(entity);
                _context.SaveChanges();
            }
            return entity != null;
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
                return new JsonResult(new { clientSecret = paymentIntent.ClientSecret });
            }

            return new JsonResult(new { error = paymentIntent.Status });
        }

    }
}
