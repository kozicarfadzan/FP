using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Mobile.Controls;
using FahrradladenPrinzenstrasse.Mobile.Models.DetaljiRezervacija;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace FahrradladenPrinzenstrasse.Mobile.ViewModels.DetaljiRezervacija
{
    /// <summary>
    /// ViewModel of transaction history template.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class TransactionHistoryViewModel : BaseViewModel
    {
        private readonly APIService _serviceRezervacija = new APIService("Rezervacija");
        private readonly APIService _serviceOcjenaProizvoda = new APIService("OcjenaProizvoda");
        private readonly int rezervacijaId;
        private Models.DetaljiRezervacija.Rezervacija Rezervacija;

        #region Constructor
        public TransactionHistoryViewModel(int RezervacijaId)
        {
            rezervacijaId = RezervacijaId;
            Title = "Detalji rezervacije #" + RezervacijaId;
        }

        public async void LoadTransactions()
        {
            Rezervacija = await _serviceRezervacija.GetById<Models.DetaljiRezervacija.Rezervacija>(rezervacijaId);

            Rezervacija.RezervacijaProdajaBicikla
                .GroupBy(x => x.BiciklStanje.BiciklId)
                .Select(t => new Transactions
                {
                    Product = t.First(),
                    ProductName = t.First().BiciklStanje.Bicikl.PuniNaziv,
                    UnitPrice = "- " + t.First().BiciklStanje.Bicikl.Cijena.Value.ToString("0.00") + " KM",
                    Amount = t.Count(),
                    TransactionAmount = "- " + (t.First().BiciklStanje.Bicikl.Cijena.Value * t.Count()).ToString("0.00") + " KM",
                    IsCredited = Rezervacija.DatumUplate != null,
                    Date = Rezervacija.DatumRezervacije,
                    Image = t.First().BiciklStanje.Bicikl.Slika,
                    Ocjena = t.First().OcjenaKorisnika ?? 0
                })
                .ForEach(x => { UpdateRatingStars(x);  TransactionDetails.Add(x); });

            Rezervacija.RezervacijaProdajaDio
                .GroupBy(x => x.DioStanje.DioId)
                .Select(t => new Transactions
                {
                    Product = t.First(),
                    ProductName = t.First().DioStanje.Dio.Naziv,
                    UnitPrice = "- " + t.First().DioStanje.Dio.Cijena.ToString("0.00") + " KM",
                    Amount = t.Count(),
                    TransactionAmount = "- " + (t.First().DioStanje.Dio.Cijena * t.Count()).ToString("0.00") + " KM",
                    IsCredited = Rezervacija.DatumUplate != null,
                    Date = Rezervacija.DatumRezervacije,
                    Image = t.First().DioStanje.Dio.Slika,
                    Ocjena = t.First().OcjenaKorisnika ?? 0
                })
                .ForEach(x => { UpdateRatingStars(x); TransactionDetails.Add(x); });

            Rezervacija.RezervacijaProdajaOprema
                .GroupBy(x => x.OpremaStanje.OpremaId)
                .Select(t => new Transactions
                {
                    Product = t.First(),
                    ProductName = t.First().OpremaStanje.Oprema.Naziv,
                    UnitPrice = "- " + t.First().OpremaStanje.Oprema.Cijena.ToString("0.00") + " KM",
                    Amount = t.Count(),
                    TransactionAmount = "- " + (t.First().OpremaStanje.Oprema.Cijena * t.Count()).ToString("0.00") + " KM",
                    IsCredited = Rezervacija.DatumUplate != null,
                    Date = Rezervacija.DatumRezervacije,
                    Image = t.First().OpremaStanje.Oprema.Slika,
                    Ocjena = t.First().OcjenaKorisnika ?? 0
                })
                .ForEach(x => { UpdateRatingStars(x); TransactionDetails.Add(x); });

            Rezervacija.RezervacijaServis
                .GroupBy(x => x.ServisId)
                .Select(t => new Transactions
                {
                    ProductName = t.First().Servis.Naziv,
                    UnitPrice = "- " + t.First().Servis.Cijena.ToString("0.00") + " KM",
                    Amount = t.Count(),
                    TransactionAmount = "- " + (t.First().Servis.Cijena * t.Count()).ToString("0.00") + " KM",
                    IsCredited = Rezervacija.DatumUplate != null,
                    Date = Rezervacija.DatumRezervacije
                })
                .ForEach(x => TransactionDetails.Add(x));


            Rezervacija.RezervacijaIznajmljenaBicikla
                .GroupBy(x => x.BiciklStanje.BiciklId)
                .Select(t => new Transactions
                {
                    Product = t.First(),
                    ProductName = t.First().BiciklStanje.Bicikl.PuniNaziv,
                    UnitPrice = "- " + t.First().BiciklStanje.Bicikl.CijenaPoDanu.Value.ToString("0.00") + " KM",
                    Amount = t.Count(),
                    TransactionAmount = "- " + (t.First().BiciklStanje.Bicikl.CijenaPoDanu.Value * t.Count()).ToString("0.00") + " KM",
                    IsCredited = Rezervacija.DatumUplate != null,
                    Date = Rezervacija.DatumRezervacije,
                    Image = t.First().BiciklStanje.Bicikl.Slika,
                    Ocjena = t.First().OcjenaKorisnika ?? 0
                })
                .ForEach(x => { UpdateRatingStars(x); TransactionDetails.Add(x); });

        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the employee details.
        /// </summary>
        /// <value>The employee details.</value>
        public ObservableCollection<Transactions> TransactionDetails { get; set; } = new ObservableCollection<Transactions>();

        #endregion

        #region Methods

        #endregion


        #region star rating
        public void UpdateRatingStars(Transactions Transaction)
        {
            var star_emptyinside = new Star { Image = "star_empty.png" };
            var Star_Filled = new Star { Image = "star_filled.png" };

            Transaction.Star1 = star_emptyinside;
            Transaction.Star2 = star_emptyinside;
            Transaction.Star3 = star_emptyinside;
            Transaction.Star4 = star_emptyinside;
            Transaction.Star5 = star_emptyinside;

            if (Transaction.Ocjena >= 1)
                Transaction.Star1 = Star_Filled;
            if (Transaction.Ocjena >= 2)
                Transaction.Star2 = Star_Filled;
            if (Transaction.Ocjena >= 3)
                Transaction.Star3 = Star_Filled;
            if (Transaction.Ocjena >= 4)
                Transaction.Star4 = Star_Filled;
            if (Transaction.Ocjena == 5)
                Transaction.Star5 = Star_Filled;
        }


        public async Task RateStar(int RatingNum, Transactions Transaction)
        {
            if (RatingNum >= 1 && RatingNum <= 5)
            {
                var request = new Model.Requests.OcjenaKorisnikaInsertRequest
                {
                    Ocjena = RatingNum
                };

                if (Transaction.Product is Model.RezervacijaProdajaBicikla r1)
                {
                    request.BiciklId = r1.BiciklStanje.BiciklId;
                }
                else if (Transaction.Product is Model.RezervacijaProdajaDio r2)
                {
                    request.DioId = r2.DioStanje.DioId;
                }
                else if (Transaction.Product is Model.RezervacijaProdajaOprema r3)
                {
                    request.OpremaId = r3.OpremaStanje.OpremaId;
                }
                else if (Transaction.Product is Model.RezervacijaIznajmljenaBicikla r4)
                {
                    request.BiciklId = r4.BiciklStanje.BiciklId;
                }
                else
                {
                    return;
                }

                Transaction.Ocjena = RatingNum;

                UpdateRatingStars(Transaction);

                var rezultat = await _serviceOcjenaProizvoda.Insert<bool>(request, "OcijeniProizvod");
                if (!rezultat) {
                    await Application.Current.MainPage.DisplayAlert("Greška", "Greška prilikom ocjenjivanja proizvoda", "OK");
                }
            }
        }
        #endregion

    }
}
