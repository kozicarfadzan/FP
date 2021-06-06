using AutoMapper;
using FahrradladenPrinzenstrasse.Model.Requests;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.WebAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Mvc;
using FahrradladenPrinzenstrasse.Data.EntityModels;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public class BiciklService : IBiciklService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;
        private readonly IKorisnikService korisnikService;

        public BiciklService(MyContext context, IMapper mapper, IKorisnikService korisnikService)
        {
            _context = context;
            _mapper = mapper;
            this.korisnikService = korisnikService;
        }

        public List<Model.Bicikl> Get(BiciklSearchRequest request)
        {
            var query = _context.Bicikl.AsQueryable();


            query = query.Include(x => x.Boja)
                        .Include(x => x.Model.MaterijalOkvira)
                        .Include(x => x.Model.Proizvodjac)
                        .Include(x => x.VelicinaOkvira)
                        .Include(x => x.StarosnaGrupa);

            query = query.Where(x => x.BiciklStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaBicikla.Count() == 0).Any())
                .Where(x => x.Aktivan);

            #region Filteri

            if (request.Stanje != null && request.Stanje.Count != 0)
            {
                var stanja = new List<Data.EntityModels.Stanje>();
                foreach (var item in request.Stanje)
                {
                    stanja.Add((Data.EntityModels.Stanje)item);
                }
                query = query.Where(x => stanja.Contains(x.Stanje));
            }
            else
            {
                if (request.SamoIznajmljivanje)
                    query = query.Where(x => x.Stanje == Data.EntityModels.Stanje.Korišteno);
                else
                    query = query.Where(x => x.Stanje == Data.EntityModels.Stanje.Novo || x.Stanje == Data.EntityModels.Stanje.Polovno);
            }

            if (request.SpolBicikla != null && request.SpolBicikla.Count != 0)
            {
                var spolovi = new List<Data.EntityModels.SpolBicikl>();
                foreach (var item in request.SpolBicikla)
                {
                    spolovi.Add((Data.EntityModels.SpolBicikl)item);
                }
                query = query.Where(x => spolovi.Contains(x.Model.SpolBicikl));
            }


            if (request.ModelId != 0)
                query = query.Where(x => x.ModelId == request.ModelId);

            if (!string.IsNullOrEmpty(request.VelicinaOkvira))
                query = query.Where(x => x.VelicinaOkvira.Naziv == request.VelicinaOkvira);

            if (request.VelicinaOkviraId != null && request.VelicinaOkviraId.Count != 0)
            {
                query = query.Where(x => request.VelicinaOkviraId.Contains(x.VelicinaOkviraId));
            }

            if (request.ProizvodjacId != null && request.ProizvodjacId.Count != 0)
            {
                query = query.Where(x => request.ProizvodjacId.Contains(x.Model.ProizvodjacId));
            }

            if (request.StarosnaGrupaId != null && request.StarosnaGrupaId.Count != 0)
            {
                query = query.Where(x => request.StarosnaGrupaId.Contains(x.StarosnaGrupaId));
            }

            if (request.BojaId != null && request.BojaId.Count != 0)
            {
                query = query.Where(x => request.BojaId.Contains(x.BojaId));
            }

            if (request.Brzina != null && request.Brzina.Count != 0)
            {
                query = query.Where(x => request.Brzina.Contains(x.Model.Brzina));
            }

            if (request.NoznaKocnica != null)
            {
                if (request.NoznaKocnica == 0)
                    query = query.Where(x => x.NoznaKocnica == false);
                else
                    query = query.Where(x => x.NoznaKocnica == true);
            }

            if (request.Suspenzija != null)
            {
                var suspenzija = (Data.EntityModels.Suspenzija)request.Suspenzija;
                query = query.Where(x => x.Model.Suspenzija == suspenzija);
            }

            #endregion

            if (request.Poredak != 0)
            {
                switch (request.Poredak)
                {
                    case 1: query = query.OrderBy(x => x.Model.Naziv); break;
                    case 2: query = query.OrderByDescending(x => x.Model.Naziv); break;
                    case 3: query = query.OrderByDescending(x => x.Cijena); break;
                    case 4: query = query.OrderBy(x => x.Cijena); break;
                }
            }

            var list = query.ToList();

            var model_list = _mapper.Map<List<Model.Bicikl>>(list);

            foreach (var item in model_list)
            {
                item.Ocjena = _context.OcjenaProizvoda.Where(x => x.BiciklId == item.BiciklId).Average(x => (double?)x.Ocjena) ?? 0.0;

                Data.EntityModels.Stanje ItemStanje = (Data.EntityModels.Stanje)item.Stanje;
                var SizesQuery = _context.Bicikl.Where(x => x.ModelId == item.ModelId)
                    .Where(x => x.Stanje == ItemStanje)
                    .Where(x => x.BiciklStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaBicikla.Count() == 0).Any())
                    .Where(x => x.Aktivan)
                    .AsQueryable();

                if (request.SamoIznajmljivanje)
                    SizesQuery = SizesQuery.Where(x => x.Stanje == Data.EntityModels.Stanje.Korišteno);
                else
                    SizesQuery = SizesQuery.Where(x => x.Stanje == Data.EntityModels.Stanje.Novo || x.Stanje == Data.EntityModels.Stanje.Polovno);

                item.SizeVariants = SizesQuery.GroupBy(x => x.VelicinaOkvira.Naziv).Select(x => x.Key).ToList();
            }

            return model_list;
        }

        public Model.Bicikl GetById(int id)
        {
            var entity = _context.Bicikl.Where(x => x.BiciklId == id)
                .Include(x => x.Boja)
                .Include(x => x.Model.MaterijalOkvira)
                .Include(x => x.Model.Proizvodjac)
                .Include(x => x.VelicinaOkvira)
                .Include(x => x.StarosnaGrupa)
                .FirstOrDefault();

            return _mapper.Map<Model.Bicikl>(entity);
        }

        public List<DateTime> GetDaneBezDostupnihTermina(int Id, int Kolicina)
        {
            var rezervisani_termini = new List<DateTime>();

            var aktivne_rezervacije = _context.RezervacijaIznajmljenaBicikla
                .Where(x => x.BiciklStanje.BiciklId == Id)
                .Where(x => x.DatumPreuzimanja.Date >= DateTime.Now.Date || x.DatumVracanja.Date >= DateTime.Now.Date)
                .ToList();

            Dictionary<DateTime, int> broj_rezervacija_po_danima = new Dictionary<DateTime, int>();

            foreach (var item in aktivne_rezervacije)
            {
                foreach (DateTime date in DateTimeHelper.EachDay(item.DatumPreuzimanja, item.DatumVracanja))
                {
                    if (broj_rezervacija_po_danima.ContainsKey(date))
                        broj_rezervacija_po_danima[date]++;
                    else
                        broj_rezervacija_po_danima[date] = 1;
                }
            }

            var termini_u_kosarici = _context.TerminStavka.Where(x => x.KlijentId == korisnikService.GetCurrentUser().Klijent.Id && x.BiciklId == Id).ToList();
            foreach (var termin_u_kosarici in termini_u_kosarici)
            {
                foreach (DateTime date in DateTimeHelper.EachDay(termin_u_kosarici.DatumPreuzimanja, termin_u_kosarici.DatumVracanja))
                {
                    if (broj_rezervacija_po_danima.ContainsKey(date))
                        broj_rezervacija_po_danima[date] += termin_u_kosarici.Kolicina;
                    else
                        broj_rezervacija_po_danima[date] = termin_u_kosarici.Kolicina;
                }
            }

            var ukupno_u_skladistu = _context.BiciklStanje.Count(x => x.BiciklId == Id && x.Aktivan == true && x.KupacId == null);
            foreach (var item in broj_rezervacija_po_danima)
            {
                DateTime date = item.Key;
                int broj_rezervacija_za_dan = item.Value;
                int preostalo_bicikala = ukupno_u_skladistu - broj_rezervacija_za_dan;

                if (preostalo_bicikala < Kolicina)
                    rezervisani_termini.Add(date);
            }

            return rezervisani_termini;
        }

        public bool OdaberiTermin(Model.Requests.BiciklOdaberiTerminRequest request)
        {
            int StanjeInt = (int)Stanje.Korišteno;
            Data.EntityModels.Stanje StanjeEnum = (Data.EntityModels.Stanje)StanjeInt;
            Bicikl Bicikl = _context.Bicikl
                .Include(x => x.BiciklStanje)
                .Where(x => x.BiciklId == request.Id)
                .Where(x => x.Stanje == StanjeEnum)
                .FirstOrDefault();

            if (Bicikl == null)
            {
                throw new UserException("Biciklo nije pronađeno.");
            }

            if (request.DatumPreuzimanja.Date < DateTime.Now.Date || request.DatumPreuzimanja.Date > request.DatumVracanja.Date)
            {
                throw new UserException("Forma nije ispravno popunjena.");
            }

            var Klijent = korisnikService.GetCurrentUser().Klijent;

            int ukupno_u_skladistu = Bicikl.BiciklStanje.Count(x => x.Aktivan == true && x.KupacId == null);
            int ukupno_u_kosarici = _context.TerminStavka.Where(x => x.KlijentId == Klijent.Id && x.BiciklId == request.Id)
                .Where(x =>
                   (
                       (x.DatumPreuzimanja.Date >= request.DatumPreuzimanja.Date && x.DatumPreuzimanja.Date <= request.DatumVracanja.Date)
                       || (x.DatumVracanja.Date >= request.DatumPreuzimanja.Date && x.DatumVracanja.Date <= request.DatumVracanja.Date)
                   )
                   ||
                   (
                       (request.DatumPreuzimanja.Date >= x.DatumPreuzimanja.Date && request.DatumPreuzimanja.Date <= x.DatumVracanja.Date)
                       || (request.DatumVracanja.Date >= x.DatumPreuzimanja.Date && request.DatumVracanja.Date <= x.DatumVracanja.Date)
                   ))
                   .Sum(x => x.Kolicina);

            var broj_termina_kolizija = _context.RezervacijaIznajmljenaBicikla
                .Where(x => x.BiciklStanje.BiciklId == request.Id)
                .Where(x =>
                   (
                       (x.DatumPreuzimanja.Date >= request.DatumPreuzimanja.Date && x.DatumPreuzimanja.Date <= request.DatumVracanja.Date)
                       || (x.DatumVracanja.Date >= request.DatumPreuzimanja.Date && x.DatumVracanja.Date <= request.DatumVracanja.Date)
                   )
                   ||
                   (
                       (request.DatumPreuzimanja.Date >= x.DatumPreuzimanja.Date && request.DatumPreuzimanja.Date <= x.DatumVracanja.Date)
                       || (request.DatumVracanja.Date >= x.DatumPreuzimanja.Date && request.DatumVracanja.Date <= x.DatumVracanja.Date)
                   )
                )
                .ToList();

            int ukupno_dostupno = ukupno_u_skladistu - ukupno_u_kosarici - broj_termina_kolizija.Count();
            if (request.Kolicina > ukupno_dostupno)
            {
                if (ukupno_dostupno == 0)
                    throw new UserException("Nema dostupnih termina za odabrani dan ili vremenski opseg.");

                throw new UserException("Biciklo nije dostupno u traženoj količini za odabrani vremenski opseg.");
            }

            var PostojecaStvaka = _context.TerminStavka.Where(x => x.KlijentId == Klijent.Id && x.BiciklId == request.Id && x.DatumPreuzimanja == request.DatumPreuzimanja && x.DatumVracanja == request.DatumVracanja).FirstOrDefault();
            if (PostojecaStvaka != null)
            {
                PostojecaStvaka.Kolicina += request.Kolicina;
            }
            else
            {
                _context.TerminStavka.Add(new TerminStavka
                {
                    KlijentId = Klijent.Id,
                    BiciklId = request.Id,
                    Kolicina = request.Kolicina,
                    DatumPreuzimanja = request.DatumPreuzimanja.Date,
                    DatumVracanja = request.DatumVracanja.Date.AddHours(23).AddMinutes(59).AddSeconds(59)
                });
            }
            _context.SaveChanges();

            return true;
        }

        public IActionResult SaveGPSLocation(int Id, string lat, string lon)
        {
            var Bicikl = _context.Bicikl.Find(Id);
            if (Bicikl == null)
                return new NotFoundResult();

            var GPSLokacija = new BiciklGPSLokacije
            {
                BiciklId = Id,
                Latitude = decimal.Parse(lat.Replace(',', '.')),
                Longitude = decimal.Parse(lon.Replace(',', '.')),
                DateReported = DateTime.Now
            };
            try
            {
                _context.BiciklGPSLokacije.Add(GPSLokacija);
                _context.SaveChanges();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        //public ImageResult GetPhotoById(int id)
        //{
        //    var Slika = _context.Bicikl.Where(x => x.BiciklId == id).Select(x=>x.Slika)
        //        .FirstOrDefault();

        //    Image image;
        //    byte[] imageBytes;

        //    if (Slika != null && Slika.Length > 0)
        //    {
        //        imageBytes = Slika;
        //        // Convert byte[] to Image
        //        using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
        //        {
        //            image = Image.FromStream(ms, true);
        //        }
        //    }
        //    else
        //    {
        //        var path = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "default_FP.png");

        //        image = Image.FromFile(path);
        //        imageBytes = System.IO.File.ReadAllBytes(path);
        //    }

        //    return new ImageResult()
        //    {
        //        ImageFormat = image.RawFormat,
        //        EncodedImageBytes = imageBytes
        //    };
        //}

    }
}
