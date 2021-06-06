using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Areas.Klijent.ViewModels;
using FahrradladenPrinzenstrasse.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FahrradladenPrinzenstrasse.Web.Areas.Klijent.Controllers
{
    [Area("Klijent")]
    [Autorizacija(klijent: true)]
    public class RezervacijeController : Controller
    {

        private readonly MyContext db;

        public RezervacijeController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index(RezervacijePretragaVM VM)
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;
            var model = new RezervacijePretragaVM
            {
                DatumOd = VM?.DatumOd ?? DateTime.Now.Date.AddMonths(-1),
                DatumDo = VM?.DatumDo ?? DateTime.Now.Date,
                VrstaRezervacije = VM.VrstaRezervacije
            };

            IQueryable<Rezervacija> query = db.Rezervacija
                .Where(x => x.KlijentId == Klijent.Id)
                .Include(x => x.RezervacijaIznajmljenaBicikla)
                .Include(x => x.RezervacijaProdajaBicikla)
                .Include(x => x.RezervacijaProdajaDio)
                .Include(x => x.RezervacijaProdajaOprema)
                .Include(x => x.RezervacijaServis);

            if (model.DatumOd.HasValue && model.DatumDo.HasValue)
                query = query.Where(x => model.DatumOd <= x.DatumRezervacije.Date && x.DatumRezervacije.Date <= model.DatumDo.Value);

            if(model.VrstaRezervacije.HasValue)
            {
                switch (model.VrstaRezervacije.Value)
                {
                    case VrstaRezervacije.Kupovina:
                        query = query.Where(x => x.RezervacijaProdajaBicikla.Any() || x.RezervacijaProdajaDio.Any() || x.RezervacijaProdajaOprema.Any());
                        break;
                    case VrstaRezervacije.Servis:
                        query = query.Where(x => x.RezervacijaServis.Any());
                        break;
                    case VrstaRezervacije.Iznajmljivanje:
                        query = query.Where(x => x.RezervacijaIznajmljenaBicikla.Any());
                        break;
                    default:
                        break;
                }
            }


            model.Rezervacije = query.OrderByDescending(x => x.RezervacijaId).ToList();

            return View(model);
        }

        public IActionResult Detalji(int Id)
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;


            Rezervacija vm = db.Rezervacija.Where(x => x.RezervacijaId == Id)
                .Where(x => x.KlijentId == Klijent.Id)
                .Include("RezervacijaIznajmljenaBicikla.BiciklStanje.Bicikl.OcjenaProizvoda")
                .Include("RezervacijaIznajmljenaBicikla.BiciklStanje.Bicikl.Model.Proizvodjac")
                .Include("RezervacijaProdajaBicikla.BiciklStanje.Bicikl.Model.Proizvodjac")
                .Include("RezervacijaProdajaBicikla.BiciklStanje.Bicikl.OcjenaProizvoda")
                .Include("RezervacijaProdajaDio.DioStanje.Dio")
                .Include("RezervacijaProdajaDio.DioStanje.Dio.OcjenaProizvoda")
                .Include("RezervacijaProdajaOprema.OpremaStanje.Oprema")
                .Include("RezervacijaProdajaOprema.OpremaStanje.Oprema.OcjenaProizvoda")
                .Include("RezervacijaServis.Servis")
                .FirstOrDefault();

            return View(vm);
        }

        public IActionResult OcijeniBicikl(int Id, int ocjena, int rezervacijaid)
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;

            if(ocjena < 1 || ocjena > 5)
            {
                return new JsonResult(new { error = "Neispravna ocjena." });
            }

            var kupio_bicikl = db.RezervacijaProdajaBicikla.Where(x => x.BiciklStanje.BiciklId == Id)
                .Where(x => x.Rezervacija.KlijentId == Klijent.Id)
                .Any();
            if(!kupio_bicikl)
            {
                kupio_bicikl = db.RezervacijaIznajmljenaBicikla.Where(x => x.BiciklStanje.BiciklId == Id)
                .Where(x => x.Rezervacija.KlijentId == Klijent.Id)
                .Any();
            }

            if(kupio_bicikl)
            {
                var postojeca_ocjena = db.OcjenaProizvoda.Where(x => x.KlijentId == Klijent.Id && x.BiciklId == Id).FirstOrDefault();
                if(postojeca_ocjena != null)
                {
                    postojeca_ocjena.Ocjena = ocjena;
                    postojeca_ocjena.DatumOcjene = DateTime.Now;
                }
                else
                {
                    var ocjena_proizvoda = new OcjenaProizvoda
                    {
                        KlijentId = Klijent.Id,
                        BiciklId = Id,
                        Ocjena = ocjena,
                        DatumOcjene = DateTime.Now
                    };
                    db.OcjenaProizvoda.Add(ocjena_proizvoda);
                }

                db.SaveChanges();

                return RedirectToAction("Detalji", new { Id = rezervacijaid});
            }
            return new JsonResult(new { error = "Ne možete ocijeniti proizvod koji niste kupili." });
        }

        public IActionResult OcijeniDio(int Id, int ocjena, int rezervacijaid)
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;

            if (ocjena < 1 || ocjena > 5)
            {
                return new JsonResult(new { error = "Neispravna ocjena." });
            }

            var kupio_dio = db.RezervacijaProdajaDio.Where(x => x.DioStanje.DioId== Id)
                .Where(x => x.Rezervacija.KlijentId == Klijent.Id)
                .Any();
            if (kupio_dio)
            {
                var postojeca_ocjena = db.OcjenaProizvoda.Where(x => x.KlijentId == Klijent.Id && x.DioId == Id).FirstOrDefault();
                if (postojeca_ocjena != null)
                {
                    postojeca_ocjena.Ocjena = ocjena;
                    postojeca_ocjena.DatumOcjene = DateTime.Now;
                }
                else
                {
                    var ocjena_proizvoda = new OcjenaProizvoda
                    {
                        KlijentId = Klijent.Id,
                        DioId = Id,
                        Ocjena = ocjena,
                        DatumOcjene = DateTime.Now
                    };
                    db.OcjenaProizvoda.Add(ocjena_proizvoda);
                }

                db.SaveChanges();

                return RedirectToAction("Detalji", new { Id = rezervacijaid });
            }
            return new JsonResult(new { error = "Ne možete ocijeniti proizvod koji niste kupili." });
        }
        

        public IActionResult OcijeniOpremu(int Id, int ocjena, int rezervacijaid)
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;

            if (ocjena < 1 || ocjena > 5)
            {
                return new JsonResult(new { error = "Neispravna ocjena." });
            }

            var kupio_opremu = db.RezervacijaProdajaOprema.Where(x => x.OpremaStanje.OpremaId== Id)
                .Where(x => x.Rezervacija.KlijentId == Klijent.Id)
                .Any();
            if (kupio_opremu)
            {
                var postojeca_ocjena = db.OcjenaProizvoda.Where(x => x.KlijentId == Klijent.Id && x.OpremaId == Id).FirstOrDefault();
                if (postojeca_ocjena != null)
                {
                    postojeca_ocjena.Ocjena = ocjena;
                    postojeca_ocjena.DatumOcjene = DateTime.Now;
                }
                else
                {
                    var ocjena_proizvoda = new OcjenaProizvoda
                    {
                        KlijentId = Klijent.Id,
                        OpremaId = Id,
                        Ocjena = ocjena,
                        DatumOcjene = DateTime.Now
                    };
                    db.OcjenaProizvoda.Add(ocjena_proizvoda);
                }

                db.SaveChanges();

                return RedirectToAction("Detalji", new { Id = rezervacijaid });
            }
            return new JsonResult(new { error = "Ne možete ocijeniti proizvod koji niste kupili." });
        }

    }
}