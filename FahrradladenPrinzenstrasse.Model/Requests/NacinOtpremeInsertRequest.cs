﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model.Requests
{
    public class NacinOtpremeInsertRequest
    {
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public double Cijena { get; set; }
    }
}
