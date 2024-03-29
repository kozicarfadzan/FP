﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Data.EntityModels
{
    public class Korisnik
    {
        #region Attributes
        private int _korisnikID;
        private string _ime;
        private string _prezime;

        private string _korisnickoIme;
        private string _lozinkaHash;
        private string _lozinkaSalt;
        private bool _aktivan;

        private string _adresaStanovanja;
        private string _brojTelefona;
        private string _email;
  
        private string _spol;
      
     
        private int _gradID;
        public Grad _grad;
        #endregion
        public int KorisnikID
        {
            get { return _korisnikID; }
            set { _korisnikID = value; }
        }


        #region Properties
        public string Ime
        {
            get { return _ime; }
            set { _ime = value; }
        }
        public string Prezime
        {
            get { return _prezime; }
            set { _prezime = value; }
        }
        public string KorisnickoIme
        {
            get { return _korisnickoIme; }
            set { _korisnickoIme = value; }
        }
        public string LozinkaHash
        {
            get { return _lozinkaHash; }
            set { _lozinkaHash = value; }
        }
        public string LozinkaSalt
        {
            get { return _lozinkaSalt; }
            set { _lozinkaSalt = value; }
        }
        public bool Aktivan
        {
            get { return _aktivan; }
            set { _aktivan = value; }
        }
        public string AdresaStanovanja
        {
            get { return _adresaStanovanja; }
            set { _adresaStanovanja = value; }
        }
        public string BrojTelefona
        {
            get { return _brojTelefona; }
            set { _brojTelefona = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Spol
        {
            get { return _spol; }
            set { _spol = value; }
        }
        public int GradID
        {
            get { return _gradID; }
            set { _gradID = value; }
        }
       
        public Grad Grad
        {
            get { return _grad; }
            set { _grad = value; }
        }

        public Zaposlenik Zaposlenik { get; set; }
        public Administrator Administrator { get; set; }
        public Klijent Klijent { get; set; }
        #endregion

        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;
    }
}
