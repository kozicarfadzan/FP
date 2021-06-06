using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Model
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
    }
}
