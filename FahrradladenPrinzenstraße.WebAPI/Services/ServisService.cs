using AutoMapper;
using FahrradladenPrinzenstraße.Model;
using FahrradladenPrinzenstraße.Model.Requests;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.WebAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FahrradladenPrinzenstraße.WebAPI.Services
{
    public class ServisService : IServisService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;
        private readonly IKorisnikService _korisnikService;

        public ServisService(MyContext context, IMapper mapper, IKorisnikService korisnikService)
        {
            _context = context;
            _mapper = mapper;
            _korisnikService = korisnikService;
        }

        public List<Model.Servis> Get()
        {
            var query = _context.Servis.AsQueryable();

            var list = query.ToList();

            var model_list = _mapper.Map<List<Model.Servis>>(list);

            return model_list;
        }

        public Model.Servis GetById(int id)
        {
            var entity = _context.Servis.Where(x => x.ServisId == id).FirstOrDefault();

            return _mapper.Map<Model.Servis>(entity);
        }

        public List<string> DostupniTermini(ServisOdaberiTerminRequest request)
        {
            var Klijent = _korisnikService.GetCurrentUser().Klijent;
            var lista_termina = new List<string>();

            Data.EntityModels.Servis Servis = _context.Servis
                .Where(x => x.ServisId == request.Id).FirstOrDefault();

            Data.EntityModels.RadnoVrijeme radnoVrijeme = _context.RadnoVrijeme.Where(x => x.DanUSedmici == request.Datum.DayOfWeek).FirstOrDefault();

            if (radnoVrijeme == null || Servis == null)
                return lista_termina;

            var rezervacije_za_dan = _context.RezervacijaServis.Where(x => x.DatumServisiranja.Date == request.Datum.Date).Include(x => x.Servis).ToList();

            int pocetak_minute = radnoVrijeme.Pocetak.Hours * 60 + radnoVrijeme.Pocetak.Minutes;
            int kraj_minute = radnoVrijeme.Kraj.Hours * 60 + radnoVrijeme.Kraj.Minutes;

            for (int pocetak_termina = pocetak_minute; pocetak_termina < kraj_minute; pocetak_termina += 30)
            {
                int termin_sati = pocetak_termina / 60;
                int termin_minute = pocetak_termina % 60;

                int kraj_termina = pocetak_termina + (int)(Servis.Trajanje * 60) * request.Kolicina;

                string sati_string = termin_sati.ToString().PadLeft(2, '0');
                string minute_string = termin_minute.ToString().PadLeft(2, '0');

                bool kolizija = false;
                if (pocetak_termina + Servis.Trajanje * 60 > kraj_minute)
                {
                    kolizija = true;
                }
                else if (kraj_termina > kraj_minute)
                {
                    kolizija = true;
                }

                foreach (var rezervacija in rezervacije_za_dan)
                {
                    int pocetak_rezervacije = rezervacija.DatumServisiranja.Hour * 60 + rezervacija.DatumServisiranja.Minute;
                    int kraj_rezervacije = pocetak_rezervacije + (int)(rezervacija.Servis.Trajanje * 60);
                    if (pocetak_termina >= pocetak_rezervacije && pocetak_termina < kraj_rezervacije)
                        kolizija = true;

                    else if (kraj_termina > pocetak_rezervacije && kraj_termina <= kraj_rezervacije)
                        kolizija = true;

                }

                if (!kolizija)
                    lista_termina.Add(sati_string + ":" + minute_string);

            }

            return lista_termina;
        }

        public bool OdaberiTermin(ServisOdaberiTerminRequest request)
        {
            if(request.Datum.Date <= DateTime.Now.Date)
                throw new UserException("Forma nije ispravno popunjena.");

            Data.EntityModels.Servis Servis = _context.Servis
                .Where(x => x.ServisId == request.Id).FirstOrDefault();
            if (Servis == null)
            {
                throw new UserException("Biciklo nije pronađeno.");
            }

            double TrajanjeServisa = Servis.Trajanje * request.Kolicina;
            double SumServiceHours = _context.RezervacijaServis.Where(x => x.DatumServisiranja.Date == request.Datum).Sum(x => (double?)x.Servis.Trajanje ?? 0);
            if (SumServiceHours + TrajanjeServisa > 8)
            {
                throw new UserException("Nema dostupnih termina za odabrani dan ili vremenski opseg.");
            }

            return true;
        }

        public object PotvrdaRezervacijeTermina(ServisPotvrdaRezervacijeTerminaRequest request)
        {
            var Korisnik = _korisnikService.GetCurrentUser();
            var Klijent = Korisnik.Klijent;

            Data.EntityModels.Servis Servis = _context.Servis
                .Where(x => x.ServisId == request.Id).FirstOrDefault();
            if (request.Datum.Date <= DateTime.Now.Date || Servis == null)
            {
                throw new UserException("Neispravan unos.");
            }

            double TrajanjeServisa = Servis.Trajanje * request.Kolicina;
            if (TrajanjeServisa > 8)
            {
                throw new UserException("Ukupno trajanje servisa ne smije preci 8 sati.");
            }

            request.UkupniIznos = Servis.Cijena * request.Kolicina;
            request.BrojServisStavki = request.Kolicina;

            if (request.DetaljiServisa != null && request.DetaljiServisa.Length > 0)
            {
                double SumServiceHours = _context.RezervacijaServis.Where(x => x.DatumServisiranja.Date == request.Datum).Sum(x => (double?)x.Servis.Trajanje ?? 0);
                if (SumServiceHours + TrajanjeServisa > 8)
                {
                    throw new UserException("Odabrani termin servisa prekoracuje dostupno vrijeme za odabrani dan.");
                }

                request.Datum = request.Datum.Add(request.Satnica);

                var lista_servisa = new List<Data.EntityModels.RezervacijaServis>();
                for (int i = 0; i < request.DetaljiServisa.Length; i++)
                {
                    lista_servisa.Add(new Data.EntityModels.RezervacijaServis
                    {
                        Boja = request.DetaljiServisa[i].Boja,
                        Proizvodjac = request.DetaljiServisa[i].Proizvodjac,
                        DodatniTroskovi = request.DetaljiServisa[i].DodatniTroskovi,
                        Model = request.DetaljiServisa[i].Model,
                        Opis = request.DetaljiServisa[i].Opis,
                        ServisId = request.Id,
                        Tip = (Data.EntityModels.Tip)((int)request.DetaljiServisa[i].Tip),
                        DatumServisiranja = request.Datum
                    });
                    request.Datum = request.Datum.AddHours(Servis.Trajanje);
                }

                Data.EntityModels.Rezervacija narudzba = new Data.EntityModels.Rezervacija
                {
                    AdresaStanovanja = Korisnik.AdresaStanovanja,
                    BrojTelefona = Korisnik.BrojTelefona,
                    Email = Korisnik.Email,
                    Grad = Korisnik.Grad.Naziv,
                    Ime = Korisnik.Ime,
                    Prezime = Korisnik.Prezime,

                    DatumRezervacije = DateTime.Now,
                    KlijentId = Klijent.Id,
                    NacinOtpremeId = _context.NacinOtpreme.Where(x => x.Cijena == 0).First().NacinOtpremeId,
                    NacinPlacanja = "online",
                    UkupniIznos = request.UkupniIznos,
                    RezervacijaServis = lista_servisa,
                    StanjeRezervacije = Data.EntityModels.StanjeRezervacije.Čekanje_uplate
                };

                _context.Rezervacija.Add(narudzba);
                _context.SaveChanges();

                return true;
            }
            else
            {
                return request;
            }
        }

    }
}
