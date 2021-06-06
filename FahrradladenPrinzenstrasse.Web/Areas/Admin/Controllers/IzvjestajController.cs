using System;
using System.Collections.Generic;
using System.Linq;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels;
using FahrradladenPrinzenstrasse.Web.Controllers;
using FahrradladenPrinzenstrasse.Web.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels.IzvjestajInventarPrikazVM;
using static FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels.IzvjestajObracunVM;

namespace FahrradladenPrinzenstrasse.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Autorizacija(administrator: true)]
    public class IzvjestajController : Controller
    {

        private readonly MyContext db;

        public IzvjestajController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Inventar()
        {
            var model = new IzvjestajInventarVM
            {
                Lokacije = db.Lokacija
                .Where(x => x.IsDeleted == false).Select(x => new SelectListItem
                {
                    Value = x.LokacijaId.ToString(),
                    Text = x.Naziv
                }).ToList(),
                DostupneOpcije = new List<SelectListItem>
                {
                    new SelectListItem {Value = "NoviBicikli", Text ="Novi bicikli"},
                    new SelectListItem {Value = "PolovniBicikli", Text ="Polovni bicikli"},
                    new SelectListItem {Value = "KorišteniBicikli", Text ="Korišteni bicikli"},
                    new SelectListItem {Value = "Dijelovi", Text ="Dijelovi"},
                    new SelectListItem {Value = "Oprema", Text ="Oprema"},
                },
                NaciniPrikaza = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Šifra proizvoda"},
                    new SelectListItem { Value = "2", Text = "Serijski broj"}
                }
            };
            return View(model);
        }
        public IActionResult InventarPrikaz(IzvjestajInventarVM request)
        {
            var stavke = new List<InventarStavka>();
            bool showAll = request.OdabraneOpcije is null;

            if (showAll || request.OdabraneOpcije.Contains("NoviBicikli"))
            {
                if (request.NacinPrikaza == 1)
                {

                    stavke.AddRange(db.Bicikl.Where(x => x.Stanje == Stanje.Novo).Select(x => new InventarStavka
                    {
                        Šifra = x.BiciklId.ToString(),
                        Cijena = x.Cijena.Value,
                        Naziv = x.Model.Proizvodjac.Naziv + " " + x.Model.Naziv,
                        VrstaStavke = "Novo biciklo",
                        KolicinaNaStanju = x.BiciklStanje.Count(y => y.LokacijaId == request.LokacijaId && y.KupacId == null && y.Aktivan)
                    }).Where(x => x.KolicinaNaStanju > 0).ToList());
                }
                else
                {
                    stavke.AddRange(db.BiciklStanje.Where(x => x.Bicikl.Stanje == Stanje.Novo)
                        .Where(x => x.LokacijaId == request.LokacijaId && x.KupacId == null && x.Aktivan)
                        .Select(x => new InventarStavka
                        {
                            Šifra = x.Sifra,
                            Cijena = x.Bicikl.Cijena.Value,
                            Naziv = x.Bicikl.Model.Proizvodjac.Naziv + " " + x.Bicikl.Model.Naziv,
                            VrstaStavke = "Novo biciklo",
                            KolicinaNaStanju = 1
                        }).ToList());
                }
            }
            if (showAll || request.OdabraneOpcije.Contains("PolovniBicikli"))
            {
                if (request.NacinPrikaza == 1)
                {

                    stavke.AddRange(db.Bicikl.Where(x => x.Stanje == Stanje.Polovno).Select(x => new InventarStavka
                    {
                        Šifra = x.BiciklId.ToString(),
                        Cijena = x.Cijena.Value,
                        Naziv = x.Model.Proizvodjac.Naziv + " " + x.Model.Naziv,
                        VrstaStavke = "Polovno biciklo",
                        KolicinaNaStanju = x.BiciklStanje.Count(y => y.LokacijaId == request.LokacijaId && y.KupacId == null && y.Aktivan)
                    }).Where(x => x.KolicinaNaStanju > 0).ToList());
                }
                else
                {
                    stavke.AddRange(db.BiciklStanje.Where(x => x.Bicikl.Stanje == Stanje.Polovno)
                        .Where(x => x.LokacijaId == request.LokacijaId && x.KupacId == null && x.Aktivan)
                        .Select(x => new InventarStavka
                        {
                            Šifra = x.Sifra,
                            Cijena = x.Bicikl.Cijena.Value,
                            Naziv = x.Bicikl.Model.Proizvodjac.Naziv + " " + x.Bicikl.Model.Naziv,
                            VrstaStavke = "Polovno biciklo",
                            KolicinaNaStanju = 1
                        }).ToList());
                }
            }
            if (showAll || request.OdabraneOpcije.Contains("KorišteniBicikli"))
            {
                if (request.NacinPrikaza == 1)
                {
                    stavke.AddRange(db.Bicikl.Where(x => x.Stanje == Stanje.Korišteno).Select(x => new InventarStavka
                    {
                        Šifra = x.BiciklId.ToString(),
                        Cijena = x.CijenaPoDanu.Value,
                        Naziv = x.Model.Proizvodjac.Naziv + " " + x.Model.Naziv,
                        VrstaStavke = "Korišteno biciklo",
                        KolicinaNaStanju = x.BiciklStanje.Count(y => y.LokacijaId == request.LokacijaId && y.KupacId == null && y.Aktivan)
                    }).Where(x => x.KolicinaNaStanju > 0).ToList());
                }
                else
                {
                    stavke.AddRange(db.BiciklStanje.Where(x => x.Bicikl.Stanje == Stanje.Korišteno)
                        .Where(x => x.LokacijaId == request.LokacijaId && x.KupacId == null && x.Aktivan)
                        .Select(x => new InventarStavka
                        {
                            Šifra = x.Sifra,
                            Cijena = x.Bicikl.CijenaPoDanu.Value,
                            Naziv = x.Bicikl.Model.Proizvodjac.Naziv + " " + x.Bicikl.Model.Naziv,
                            VrstaStavke = "Korišteno biciklo",
                            KolicinaNaStanju = 1
                        }).ToList());
                }
            }
            if (showAll || request.OdabraneOpcije.Contains("Dijelovi"))
            {
                if (request.NacinPrikaza == 1)
                {
                    stavke.AddRange(db.Dio.Where(x => x.IsDeleted == false).Select(x => new InventarStavka
                    {
                        Šifra = x.DioId.ToString(),
                        Cijena = x.Cijena,
                        Naziv = x.Naziv,
                        VrstaStavke = "Dio",
                        KolicinaNaStanju = x.DioStanje.Count(y => y.LokacijaId == request.LokacijaId && y.KupacId == null && y.Aktivan)
                    }).Where(x => x.KolicinaNaStanju > 0).ToList());
                }
                else
                {
                    stavke.AddRange(db.DioStanje
                        .Where(x => x.LokacijaId == request.LokacijaId && x.KupacId == null && x.Aktivan)
                        .Select(x => new InventarStavka
                        {
                            Šifra = x.Sifra,
                            Cijena = x.Dio.Cijena,
                            Naziv = x.Dio.Naziv,
                            VrstaStavke = "Dio",
                            KolicinaNaStanju = 1
                        }).ToList());
                }
            }
            if (showAll || request.OdabraneOpcije.Contains("Oprema"))
            {
                if (request.NacinPrikaza == 1)
                {
                    stavke.AddRange(db.Oprema.Where(x => x.IsDeleted == false).Select(x => new InventarStavka
                    {
                        Šifra = x.OpremaId.ToString(),
                        Cijena = x.Cijena,
                        Naziv = x.Naziv,
                        VrstaStavke = "Oprema",
                        KolicinaNaStanju = x.OpremaStanje.Count(y => y.LokacijaId == request.LokacijaId && y.KupacId == null && y.Aktivan)
                    }).Where(x => x.KolicinaNaStanju > 0).ToList());
                }
                else
                {
                    stavke.AddRange(db.OpremaStanje
                        .Where(x => x.LokacijaId == request.LokacijaId && x.KupacId == null && x.Aktivan)
                        .Select(x => new InventarStavka
                        {
                            Šifra = x.Sifra,
                            Cijena = x.Oprema.Cijena,
                            Naziv = x.Oprema.Naziv,
                            VrstaStavke = "Oprema",
                            KolicinaNaStanju = 1
                        }).ToList());
                }
            }

            var vm = new IzvjestajInventarPrikazVM
            {
                Stavke = stavke,
                NacinPrikaza = request.NacinPrikaza
            };
            return PartialView(vm);
        }
        public IActionResult Obračun(IzvjestajObracunVM request)
        {
            var vm = new IzvjestajObracunVM
            {
                ReportTypes = Enum.GetValues(typeof(ObracunReportType)).Cast<ObracunReportType>().ToList().Select(
                    x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = ((int)x).ToString()
                    }).ToList(),
                ChartLabels = new List<string>(),
                Year = request.Year ?? DateTime.Today.Year,
                Month = request.Month ?? DateTime.Today.Month,
                DateTime = request.DateTime ?? DateTime.Today
            };

            if (request.ReportType.HasValue)
                vm.ReportType = request.ReportType;
            else
                vm.ReportType = ObracunReportType.Mjesečno;

            var MonthList = new List<string> { "Jan", "Feb", "Mar", "Apr", "May<", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            if (vm.ReportType == ObracunReportType.Mjesečno)
            {
                var now = DateTime.Now;
                var NumDays = DateTime.DaysInMonth(now.Year, now.Month);
                for (int i = 1; i <= NumDays; i++)
                {
                    vm.ChartLabels.Add(i.ToString());
                }

                vm.Months = new List<SelectListItem>();
                for (int i = 0; i < MonthList.Count; i++)
                {
                    vm.Months.Add(new SelectListItem
                    {
                        Text = MonthList[i],
                        Value = (i + 1).ToString()
                    });
                }
            }
            else if (vm.ReportType == ObracunReportType.Godišnje)
            {
                vm.ChartLabels = MonthList;
            }
            else if (vm.ReportType == ObracunReportType.Dnevno)
            {
                for (int i = 0; i <= 23; i++)
                {
                    vm.ChartLabels.Add(i.ToString().PadLeft(2, '0') + ":00");
                }
            }
            else if (vm.ReportType == ObracunReportType.Sedmično)
            {
                var day = DateTime.Now.AddDays(-6);
                for (int i = 0; i < 7; i++)
                {
                    vm.ChartLabels.Add(day.DayOfWeek.ToString() + " " + day.ToShortDateString());
                    day = day.AddDays(1);
                }
            }

            vm.Data = GetObračunChart(vm);
            vm.NajprodavanijiProizvodi = GetNajprodavanijiProizvodi(vm);

            vm.Maximum = 0;
            foreach (VrstaProizvoda vrsta in Enum.GetValues(typeof(VrstaProizvoda)))
            {
                if (vm.Data[vrsta].Max() > vm.Maximum)
                    vm.Maximum = vm.Data[vrsta].Max();
            }

            return View(vm);
        }

        private List<ProdaniProizvod> GetNajprodavanijiProizvodi(IzvjestajObracunVM vm)
        {
            var list = new List<ProdaniProizvod>();
            list.AddRange(
                vm.Bicikli.GroupBy(x => x.BiciklStanje.Bicikl).Select(x => new ProdaniProizvod {
                    Naziv = x.Key.PuniNaziv,
                    Kolicina = x.Count(),
                    JedCijena = x.Key.Cijena.Value,
                    Zarada = x.Sum(s => s.BiciklStanje.Bicikl.Cijena.Value),
                    VrstaProizvoda = VrstaProizvoda.Biciklo
                }).ToList()
            );
            list.AddRange(
                vm.Oprema.GroupBy(x => x.OpremaStanje.Oprema).Select(x => new ProdaniProizvod {
                    Naziv = x.Key.Naziv,
                    Kolicina = x.Count(),
                    JedCijena = x.Key.Cijena,
                    Zarada = x.Sum(s => s.OpremaStanje.Oprema.Cijena),
                    VrstaProizvoda = VrstaProizvoda.Oprema
                }).ToList()
            );
            list.AddRange(
                vm.Dijelovi.GroupBy(x => x.DioStanje.Dio).Select(x => new ProdaniProizvod {
                    Naziv = x.Key.Naziv,
                    Kolicina = x.Count(),
                    JedCijena = x.Key.Cijena,
                    Zarada = x.Sum(s => s.DioStanje.Dio.Cijena),
                    VrstaProizvoda = VrstaProizvoda.Dio
                }).ToList()
            );
            list.AddRange(
                vm.Servisi.GroupBy(x => x.Servis).Select(x => new ProdaniProizvod  {
                    Naziv = x.Key.Naziv,
                    Kolicina = x.Count(),
                    JedCijena = x.Key.Cijena,
                    Zarada = x.Sum(s => s.Servis.Cijena),
                    VrstaProizvoda = VrstaProizvoda.Servis
                }).OrderByDescending(x => x.Zarada).ToList()
            );
            list.AddRange(
                vm.Iznajmljivanje.GroupBy(x => x.BiciklStanje.Bicikl).Select(x => new ProdaniProizvod {
                    Naziv = x.Key.PuniNaziv,
                    Kolicina = x.Count(),
                    JedCijena = x.Key.CijenaPoDanu.Value,
                    Zarada = x.Sum(s => s.BiciklStanje.Bicikl.CijenaPoDanu.Value * s.BrojDana),
                    VrstaProizvoda = VrstaProizvoda.Iznajmljivanje
                }).ToList()
            );

            list = list.OrderByDescending(x => x.Zarada).ToList();
            return list;
        }

        private Dictionary<VrstaProizvoda, List<double>> GetObračunChart(IzvjestajObracunVM vm)
        {
            var result = new Dictionary<VrstaProizvoda, List<double>>();

            var biciklQry = db.RezervacijaProdajaBicikla
                .Where(x => x.Rezervacija.StanjeRezervacije == StanjeRezervacije.U_obradi || x.Rezervacija.StanjeRezervacije == StanjeRezervacije.Završena).Include(x => x.Rezervacija).Include(x => x.BiciklStanje.Bicikl.Model.Proizvodjac).AsQueryable();
            var dioQry = db.RezervacijaProdajaDio
                .Where(x => x.Rezervacija.StanjeRezervacije == StanjeRezervacije.U_obradi || x.Rezervacija.StanjeRezervacije == StanjeRezervacije.Završena).Include(x => x.Rezervacija).Include(x => x.DioStanje.Dio).AsQueryable();
            var opremaQry = db.RezervacijaProdajaOprema
                .Where(x => x.Rezervacija.StanjeRezervacije == StanjeRezervacije.U_obradi || x.Rezervacija.StanjeRezervacije == StanjeRezervacije.Završena).Include(x => x.Rezervacija).Include(x => x.OpremaStanje.Oprema).AsQueryable();
            var servisQry = db.RezervacijaServis
                .Where(x => x.Rezervacija.StanjeRezervacije == StanjeRezervacije.U_obradi || x.Rezervacija.StanjeRezervacije == StanjeRezervacije.Završena).Include(x => x.Rezervacija).Include(x => x.Servis).AsQueryable();
            var iznajmljivanjeQry = db.RezervacijaIznajmljenaBicikla
                .Where(x => x.Rezervacija.StanjeRezervacije == StanjeRezervacije.U_obradi || x.Rezervacija.StanjeRezervacije == StanjeRezervacije.Završena).Include(x => x.Rezervacija).Include(x => x.BiciklStanje.Bicikl.Model.Proizvodjac).AsQueryable();

            var now = DateTime.Now;

            if (vm.Month != null && vm.Year != null && (vm.ReportType == ObracunReportType.Mjesečno || vm.ReportType == ObracunReportType.Godišnje))
            {
                now = new DateTime(vm.Year.Value, vm.Month.Value, 1);
            }
            else if(vm.DateTime != null && (vm.ReportType == ObracunReportType.Dnevno || vm.ReportType == ObracunReportType.Sedmično))
            {
                now = vm.DateTime.Value;
            }

            switch (vm.ReportType)
            {
                case ObracunReportType.Mjesečno:
                    biciklQry = biciklQry.Where(x => x.Rezervacija.DatumUplate.Value.Year == now.Year && x.Rezervacija.DatumUplate.Value.Month == now.Month);
                    dioQry = dioQry.Where(x => x.Rezervacija.DatumUplate.Value.Year == now.Year && x.Rezervacija.DatumUplate.Value.Month == now.Month);
                    opremaQry = opremaQry.Where(x => x.Rezervacija.DatumUplate.Value.Year == now.Year && x.Rezervacija.DatumUplate.Value.Month == now.Month);
                    servisQry = servisQry.Where(x => x.Rezervacija.DatumUplate.Value.Year == now.Year && x.Rezervacija.DatumUplate.Value.Month == now.Month);
                    iznajmljivanjeQry = iznajmljivanjeQry.Where(x => x.Rezervacija.DatumUplate.Value.Year == now.Year && x.Rezervacija.DatumUplate.Value.Month == now.Month);
                    break;
                case ObracunReportType.Dnevno:
                    biciklQry = biciklQry.Where(x => x.Rezervacija.DatumUplate.Value.Date == now.Date);
                    dioQry = dioQry.Where(x => x.Rezervacija.DatumUplate.Value.Date == now.Date);
                    opremaQry = opremaQry.Where(x => x.Rezervacija.DatumUplate.Value.Date == now.Date);
                    servisQry = servisQry.Where(x => x.Rezervacija.DatumUplate.Value.Date == now.Date);
                    iznajmljivanjeQry = iznajmljivanjeQry.Where(x => x.Rezervacija.DatumUplate.Value.Date == now.Date);
                    break;
                case ObracunReportType.Sedmično:
                    biciklQry = biciklQry.Where(x => x.Rezervacija.DatumUplate.Value.Date >= now.Date.AddDays(-6) && x.Rezervacija.DatumUplate.Value.Date <= now.Date);
                    dioQry = dioQry.Where(x => x.Rezervacija.DatumUplate.Value.Date >= now.Date.AddDays(-6) && x.Rezervacija.DatumUplate.Value.Date <= now.Date);
                    opremaQry = opremaQry.Where(x => x.Rezervacija.DatumUplate.Value.Date >= now.Date.AddDays(-6) && x.Rezervacija.DatumUplate.Value.Date <= now.Date);
                    servisQry = servisQry.Where(x => x.Rezervacija.DatumUplate.Value.Date >= now.Date.AddDays(-6) && x.Rezervacija.DatumUplate.Value.Date <= now.Date);
                    iznajmljivanjeQry = iznajmljivanjeQry.Where(x => x.Rezervacija.DatumUplate.Value.Date >= now.Date.AddDays(-6) && x.Rezervacija.DatumUplate.Value.Date <= now.Date);
                    break;
                case ObracunReportType.Godišnje:
                    biciklQry = biciklQry.Where(x => x.Rezervacija.DatumUplate.Value.Year == now.Year);
                    dioQry = dioQry.Where(x => x.Rezervacija.DatumUplate.Value.Year == now.Year);
                    opremaQry = opremaQry.Where(x => x.Rezervacija.DatumUplate.Value.Year == now.Year);
                    servisQry = servisQry.Where(x => x.Rezervacija.DatumUplate.Value.Year == now.Year);
                    iznajmljivanjeQry = iznajmljivanjeQry.Where(x => x.Rezervacija.DatumUplate.Value.Year == now.Year);
                    break;
                default:
                    break;
            }

            vm.Bicikli = biciklQry.ToList();
            vm.Dijelovi = dioQry.ToList();
            vm.Oprema = opremaQry.ToList();
            vm.Servisi = servisQry.ToList();
            vm.Iznajmljivanje = iznajmljivanjeQry.ToList();

            Dictionary<VrstaProizvoda, List<ZaradaResult>> zarada = new Dictionary<VrstaProizvoda, List<ZaradaResult>>();
            switch (vm.ReportType)
            {
                case ObracunReportType.Mjesečno:
                    zarada[VrstaProizvoda.Biciklo] = vm.Bicikli.GroupBy(x => x.Rezervacija.DatumUplate.Value.Date).Select(g => new ZaradaResult { DateTime = g.Key, Sum = g.Sum(s => s.BiciklStanje.Bicikl.Cijena.Value) }).ToList();
                    zarada[VrstaProizvoda.Dio] = vm.Dijelovi.GroupBy(x => x.Rezervacija.DatumUplate.Value.Date).Select(g => new ZaradaResult { DateTime = g.Key, Sum = g.Sum(s => s.DioStanje.Dio.Cijena) }).ToList();
                    zarada[VrstaProizvoda.Oprema] = vm.Oprema.GroupBy(x => x.Rezervacija.DatumUplate.Value.Date).Select(g => new ZaradaResult { DateTime = g.Key, Sum = g.Sum(s => s.OpremaStanje.Oprema.Cijena) }).ToList();
                    zarada[VrstaProizvoda.Servis] = vm.Servisi.GroupBy(x => x.Rezervacija.DatumUplate.Value.Date).Select(g => new ZaradaResult { DateTime = g.Key, Sum = g.Sum(s => s.Servis.Cijena) }).ToList();
                    zarada[VrstaProizvoda.Iznajmljivanje] = vm.Iznajmljivanje.GroupBy(x => x.Rezervacija.DatumUplate.Value.Date).Select(g => new ZaradaResult { DateTime = g.Key, Sum = g.Sum(s => s.BrojDana * s.BiciklStanje.Bicikl.CijenaPoDanu.Value) }).ToList();
                    break;
                case ObracunReportType.Dnevno:
                    zarada[VrstaProizvoda.Biciklo] = vm.Bicikli.GroupBy(x => x.Rezervacija.DatumUplate.Value.Hour).Select(g => new ZaradaResult { Hour = g.Key, Sum = g.Sum(s => s.BiciklStanje.Bicikl.Cijena.Value) }).ToList();
                    zarada[VrstaProizvoda.Dio] = vm.Dijelovi.GroupBy(x => x.Rezervacija.DatumUplate.Value.Hour).Select(g => new ZaradaResult { Hour = g.Key, Sum = g.Sum(s => s.DioStanje.Dio.Cijena) }).ToList();
                    zarada[VrstaProizvoda.Oprema] = vm.Oprema.GroupBy(x => x.Rezervacija.DatumUplate.Value.Hour).Select(g => new ZaradaResult { Hour = g.Key, Sum = g.Sum(s => s.OpremaStanje.Oprema.Cijena) }).ToList();
                    zarada[VrstaProizvoda.Servis] = vm.Servisi.GroupBy(x => x.Rezervacija.DatumUplate.Value.Hour).Select(g => new ZaradaResult { Hour = g.Key, Sum = g.Sum(s => s.Servis.Cijena) }).ToList();
                    zarada[VrstaProizvoda.Iznajmljivanje] = vm.Iznajmljivanje.GroupBy(x => x.Rezervacija.DatumUplate.Value.Hour).Select(g => new ZaradaResult { Hour = g.Key, Sum = g.Sum(s => s.BrojDana * s.BiciklStanje.Bicikl.CijenaPoDanu.Value) }).ToList();
                    break;
                case ObracunReportType.Sedmično:
                    zarada[VrstaProizvoda.Biciklo] = vm.Bicikli.GroupBy(x => x.Rezervacija.DatumUplate.Value.Date).Select(g => new ZaradaResult { DateTime = g.Key, Sum = g.Sum(s => s.BiciklStanje.Bicikl.Cijena.Value) }).ToList();
                    zarada[VrstaProizvoda.Dio] = vm.Dijelovi.GroupBy(x => x.Rezervacija.DatumUplate.Value.Date).Select(g => new ZaradaResult { DateTime = g.Key, Sum = g.Sum(s => s.DioStanje.Dio.Cijena) }).ToList();
                    zarada[VrstaProizvoda.Oprema] = vm.Oprema.GroupBy(x => x.Rezervacija.DatumUplate.Value.Date).Select(g => new ZaradaResult { DateTime = g.Key, Sum = g.Sum(s => s.OpremaStanje.Oprema.Cijena) }).ToList();
                    zarada[VrstaProizvoda.Servis] = vm.Servisi.GroupBy(x => x.Rezervacija.DatumUplate.Value.Date).Select(g => new ZaradaResult { DateTime = g.Key, Sum = g.Sum(s => s.Servis.Cijena) }).ToList();
                    zarada[VrstaProizvoda.Iznajmljivanje] = vm.Iznajmljivanje.GroupBy(x => x.Rezervacija.DatumUplate.Value.Date).Select(g => new ZaradaResult { DateTime = g.Key, Sum = g.Sum(s => s.BrojDana * s.BiciklStanje.Bicikl.CijenaPoDanu.Value) }).ToList();
                    break;
                case ObracunReportType.Godišnje:
                    zarada[VrstaProizvoda.Biciklo] = vm.Bicikli.GroupBy(x => x.Rezervacija.DatumUplate.Value.Month).Select(g => new ZaradaResult { Month = g.Key, Sum = g.Sum(s => s.BiciklStanje.Bicikl.Cijena.Value) }).ToList();
                    zarada[VrstaProizvoda.Dio] = vm.Dijelovi.GroupBy(x => x.Rezervacija.DatumUplate.Value.Month).Select(g => new ZaradaResult { Month = g.Key, Sum = g.Sum(s => s.DioStanje.Dio.Cijena) }).ToList();
                    zarada[VrstaProizvoda.Oprema] = vm.Oprema.GroupBy(x => x.Rezervacija.DatumUplate.Value.Month).Select(g => new ZaradaResult { Month = g.Key, Sum = g.Sum(s => s.OpremaStanje.Oprema.Cijena) }).ToList();
                    zarada[VrstaProizvoda.Servis] = vm.Servisi.GroupBy(x => x.Rezervacija.DatumUplate.Value.Month).Select(g => new ZaradaResult { Month = g.Key, Sum = g.Sum(s => s.Servis.Cijena) }).ToList();
                    zarada[VrstaProizvoda.Iznajmljivanje] = vm.Iznajmljivanje.GroupBy(x => x.Rezervacija.DatumUplate.Value.Month).Select(g => new ZaradaResult { Month = g.Key, Sum = g.Sum(s => s.BrojDana * s.BiciklStanje.Bicikl.CijenaPoDanu.Value) }).ToList();
                    break;
                default:
                    break;
            }

            foreach (VrstaProizvoda vrsta in Enum.GetValues(typeof(VrstaProizvoda)))
            {
                result[vrsta] = new List<double>();

                switch (vm.ReportType)
                {
                    case ObracunReportType.Mjesečno:

                        int NumDays = DateTime.DaysInMonth(now.Year, now.Month);

                        for (int i = 1; i <= NumDays; i++)
                        {
                            result[vrsta].Add(0);
                        }

                        foreach (var zaradaStavka in zarada[vrsta])
                        {
                            result[vrsta][zaradaStavka.DateTime.Day - 1] = zaradaStavka.Sum;
                        }

                        break;
                    case ObracunReportType.Dnevno:

                        for (int i = 0; i <= 23; i++)
                        {
                            result[vrsta].Add(0);
                        }

                        foreach (var zaradaStavka in zarada[vrsta])
                        {
                            result[vrsta][zaradaStavka.Hour] = zaradaStavka.Sum;
                        }
                        break;
                    case ObracunReportType.Sedmično:
                        for (int i = 0; i < 7; i++)
                        {
                            result[vrsta].Add(0);
                        }

                        foreach (var zaradaStavka in zarada[vrsta])
                        {
                            // 2021-01-23 - 2021-01-23 = 0
                            // 6 - 0 = 6
                            // result[vrsta][6] 

                            // 2021-01-23 - 2021-01-17 = 6
                            // 6- 6 = 0
                            // result[vrsta][0]

                            result[vrsta][6 - (now.Date - zaradaStavka.DateTime.Date).Days] = zaradaStavka.Sum;
                        }
                        break;
                    case ObracunReportType.Godišnje:
                        for (int i = 0; i < 12; i++)
                        {
                            result[vrsta].Add(0);
                        }

                        foreach (var zaradaStavka in zarada[vrsta])
                        {
                            result[vrsta][zaradaStavka.Month - 1] = zaradaStavka.Sum;
                        }
                        break;
                    default:
                        break;
                }

            }

            return result;
        }
    }
}