﻿using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Helper;
using FahrradladenPrinzenstrasse.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Controllers
{
    public class IznajmljivanjeController : Controller
    {
        private readonly MyContext db;

        public IznajmljivanjeController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index(PrikaziBiciklaZaIznajmitiVM VM)
        {
            PrikaziBiciklaZaIznajmitiVM Model = new PrikaziBiciklaZaIznajmitiVM
            {
                Proizvodjaci = db.Proizvodjac
                .Where(x => x.IsDeleted == false).ToList(),
                Stanja = Enum.GetValues(typeof(Stanje)).Cast<Stanje>().Where(x => x == Stanje.Korišteno).ToList(),
                VelicineOkvira = db.VelicinaOkvira
                .Where(x => x.IsDeleted == false).ToList(),
                StarosneGrupe = db.StarosnaGrupa
                .Where(x => x.IsDeleted == false).ToList(),
                SpoloviBicikla = Enum.GetValues(typeof(SpolBicikl)).Cast<SpolBicikl>().ToList(),
                Tipovi = Enum.GetValues(typeof(Tip)).Cast<Tip>().ToList(),
                MaterijaliOkvira = db.MaterijalOkvira
                .Where(x => x.IsDeleted == false).ToList(),
                Suspenzije = Enum.GetValues(typeof(Suspenzija)).Cast<Suspenzija>().ToList(),
                Brzine = db.Modeli.Where(x => x.IsDeleted == false).Select(x => x.Brzina).Distinct().ToList(),
                Boje = db.Boja.ToList(),

                Stanje = VM.Stanje

            };
            return View(Model);
        }

        public ActionResult UcitajListuBicikalaZaIznajmiti(PrikaziBiciklaZaIznajmitiVM VM)
        {
            PrikaziBiciklaZaIznajmitiVM Model = new PrikaziBiciklaZaIznajmitiVM();

            IQueryable<Bicikl> BiciklaQry = db.Bicikl
                .Where(x => x.Aktivan)
                .Where(x => x.Stanje == Stanje.Korišteno);

            if (VM.ProizvodjacId != null)
            {
                BiciklaQry = BiciklaQry.Where(x => VM.ProizvodjacId.Contains(x.Model.ProizvodjacId));
            }

            if (VM.BojaId != null)
            {
                BiciklaQry = BiciklaQry.Where(x => VM.BojaId.Contains(x.BojaId));
            }


            if (VM.NoznaKocnica != null && VM.NoznaKocnica != -1)
            {
                BiciklaQry = BiciklaQry.Where(x => x.NoznaKocnica == (VM.NoznaKocnica.Value == 0 ? false : true));
            }

            if (VM.VelicinaOkviraId != null)
            {
                BiciklaQry = BiciklaQry.Where(x => VM.VelicinaOkviraId.Contains(x.VelicinaOkviraId));
            }

            if (VM.SpolBicikla != null)
            {
                List<SpolBicikl> TrazeniSpolovi = new List<SpolBicikl>();

                foreach (SpolBicikl spol in Enum.GetValues(typeof(SpolBicikl)))
                {
                    foreach (string odabraniSpol in VM.SpolBicikla)
                    {
                        if (Enum.GetName(typeof(SpolBicikl), spol) == odabraniSpol)
                        {
                            TrazeniSpolovi.Add(spol);
                        }
                    }
                }

                BiciklaQry = BiciklaQry.Where(x => TrazeniSpolovi.Contains(x.Model.SpolBicikl));
            }
            if (VM.StarosnaGrupaId != null)
            {
                BiciklaQry = BiciklaQry.Where(x => VM.StarosnaGrupaId.Contains(x.StarosnaGrupaId));
            }

            if (VM.Tip != null)
            {
                List<Tip> TrazeniSpolovi = new List<Tip>();

                foreach (Tip spol in Enum.GetValues(typeof(Tip)))
                {
                    foreach (string odabraniSpol in VM.Tip)
                    {
                        if (Enum.GetName(typeof(Tip), spol) == odabraniSpol)
                        {
                            TrazeniSpolovi.Add(spol);
                        }
                    }
                }

                BiciklaQry = BiciklaQry.Where(x => TrazeniSpolovi.Contains(x.Model.Tip));
            }
            if (VM.MaterijalOkviraId != null)
            {
                BiciklaQry = BiciklaQry.Where(x => VM.MaterijalOkviraId.Contains(x.Model.MaterijalOkviraId));
            }

            if (VM.Suspenzija != null && VM.Suspenzija != -1)
            {
                List<Suspenzija> TrazenaStanja = new List<Suspenzija>();

                foreach (Suspenzija stanje in Enum.GetValues(typeof(Suspenzija)))
                {
                    if ((int)stanje == VM.Suspenzija.Value)
                    {
                        TrazenaStanja.Add(stanje);
                    }
                }

                BiciklaQry = BiciklaQry.Where(x => TrazenaStanja.Contains(x.Model.Suspenzija));
            }


            if (VM.Brzina != null && VM.Brzina != -1)
            {
                BiciklaQry = BiciklaQry.Where(x => x.Model.Brzina == VM.Brzina.Value);
            }


            if (VM.Poredak.HasValue)
            {
                switch (VM.Poredak.Value)
                {
                    case 1: BiciklaQry = BiciklaQry.OrderBy(x => x.Model.Naziv); break;
                    case 2: BiciklaQry = BiciklaQry.OrderByDescending(x => x.Model.Naziv); break;
                    case 3: BiciklaQry = BiciklaQry.OrderByDescending(x => x.Cijena); break;
                    case 4: BiciklaQry = BiciklaQry.OrderBy(x => x.Cijena); break;
                }
            }

            Model.PagedResult = BiciklaQry.Select(
               x => new PrikaziBiciklaZaIznajmitiVM.Row
               {
                   BiciklId = x.BiciklId,
                   Boja = x.Boja,
                   Cijena = x.Cijena,
                   CijenaPoDanu = x.CijenaPoDanu,
                   GodinaProizvodnje = x.GodinaProizvodnje,
                   Model = new Data.EntityModels.Model
                   {
                       Naziv = x.Model.Naziv,
                       Proizvodjac = x.Model.Proizvodjac
                   },
                   NoznaKocnica = x.NoznaKocnica,
                   Slika = x.Slika,
                   Stanje = x.Stanje,
                   Kolicina = x.BiciklStanje.Where(y => y.Aktivan).Sum(y => y.Kolicina),
                   Aktivan = x.Aktivan
               }
            ).GetPaged(VM.Page, 6);

            return PartialView(Model);

        }

    }
}
