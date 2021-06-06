using FahrradladenPrinzenstrasse.Model;
using System;
using System.ComponentModel;
using Xamarin.Forms.Internals;

namespace FahrradladenPrinzenstrasse.Mobile.Models.DetaljiRezervacija
{
    /// <summary>
    /// Model for transaction history template.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class Transactions : INotifyPropertyChanged
    {
        #region Fields

        private string productName;

        private string transactionDescription;

        private byte[] image;

        private DateTime date;

        private string transactionAmount;

        private bool isCredited;

        #endregion

        #region EventHandler

        /// <summary>
        /// EventHandler of property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of an customer.
        /// </summary>
        public string ProductName
        {
            get
            {
                return this.productName;
            }

            set
            {
                this.productName = value;
                this.OnPropertyChanged(nameof(ProductName));
            }
        }

        /// <summary>
        /// Gets or sets the transaction description.
        /// </summary>
        public string TransactionDescription
        {
            get
            {
                return this.transactionDescription;
            }

            set
            {
                this.transactionDescription = value;
                this.OnPropertyChanged(nameof(TransactionDescription));
            }
        }

        /// <summary>
        /// Gets or sets the image of an user.
        /// </summary>
        public byte[] Image
        {
            get
            {
                return this.image;
            }

            set
            {
                this.image = value;
                this.OnPropertyChanged(nameof(Image));
            }
        }

        /// <summary>
        /// Gets or sets the transaction amount.
        /// </summary>
        public string TransactionAmount
        {
            get
            {
                return this.transactionAmount;
            }

            set
            {
                this.transactionAmount = value;
                this.OnPropertyChanged(nameof(TransactionAmount));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the transaction type is credit.
        /// </summary>
        public bool IsCredited
        {
            get
            {
                return this.isCredited;
            }

            set
            {
                this.isCredited = value;
                this.OnPropertyChanged(nameof(IsCredited));
            }
        }

        /// <summary>
        /// Gets or sets the transaction amount.
        /// </summary>
        public DateTime Date
        {
            get
            {
                return this.date;
            }

            set
            {
                this.date = value;
                this.OnPropertyChanged(nameof(Date));
            }
        }

        public string UnitPrice { get; set; }
        public int Amount { get; set; }
        public int Ocjena { get; set; }
        public object Product { get; set; }
        public bool PrikaziOcjenjivanje
        {
            get
            {
                if (!isCredited) return false;

                if (Product is RezervacijaIznajmljenaBicikla || Product is RezervacijaProdajaBicikla || Product is RezervacijaProdajaDio || Product is RezervacijaProdajaOprema)
                    return true;

                return false;
            }
        }

        private Star _star1 = new Star();
        public Star Star1
        {
            get { return _star1; }
            set {
                this._star1 = value;
                this.OnPropertyChanged(nameof(Star1));
            }
        }
        private Star _star2 = new Star();
        public Star Star2
        {
            get { return _star2; }
            set
            {
                this._star2 = value;
                this.OnPropertyChanged(nameof(Star2));
            }
        }
        private Star _star3 = new Star();
        public Star Star3
        {
            get { return _star3; }
            set
            {
                this._star3 = value;
                this.OnPropertyChanged(nameof(Star3));
            }
        }
        private Star _star4 = new Star();
        public Star Star4
        {
            get { return _star4; }
            set
            {
                this._star4 = value;
                this.OnPropertyChanged(nameof(Star4));
            }
        }
        private Star _star5 = new Star();
        public Star Star5
        {
            get { return _star5; }
            set
            {
                this._star5 = value;
                this.OnPropertyChanged(nameof(Star5));
            }
        }


        #endregion

        #region Methods

        /// <summary>
        /// The PropertyChanged event occurs when changing the value of property.
        /// </summary>
        /// <param name="propertyName">The PropertyName</param>
        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
