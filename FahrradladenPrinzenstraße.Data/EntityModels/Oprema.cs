﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FahrradladenPrinzenstraße.Data.EntityModels
{
    public class Oprema
    {
      public Oprema()
        {
            OpremaStanje = new List<OpremaStanje>();
        }
        public int OpremaId { get; set; }
        public string Naziv { get; set; }
        public double Cijena { get; set; }
        public string Opis { get; set; }
        public int ProizvodjacID { get; set; }
        public Proizvodjac Proizvodjac { get; set; }

        [DefaultValue(true)]
        public bool Aktivan { get; set; } = true;
        public byte[] Slika { get; set; }
        public IEnumerable<OpremaStanje> OpremaStanje { get; set; }

    }

}
