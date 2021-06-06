using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Data.EntityModels
{
    public class Grad
    {
        private int _gradID;
        private string _naziv;
        public int GradID
        {
            get { return _gradID; }
            set { _gradID = value; }
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
