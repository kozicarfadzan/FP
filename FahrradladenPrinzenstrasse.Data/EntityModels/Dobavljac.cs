using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FahrradladenPrinzenstrasse.Data.EntityModels
{
  public  class Dobavljac
    {

        private int _dobavljacID;
        private string _naziv;
        public int DobavljacID
        {
            get { return _dobavljacID; }
            set { _dobavljacID = value; }
        }
        public string Naziv
        {
            get { return _naziv; }
            set { _naziv = value; }
        }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;
    }

}

