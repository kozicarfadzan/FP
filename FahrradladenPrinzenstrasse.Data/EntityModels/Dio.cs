﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FahrradladenPrinzenstrasse.Data.EntityModels
{
    public class Dio
    {
        public Dio()
        {
            DioStanje = new List<DioStanje>();
        }

        public int DioId { get; set; }
        public string Naziv { get; set; }
        public double Cijena { get; set; }
        public string Opis { get; set; }
       
        public int ProizvodjacID { get; set; }
        public Proizvodjac Proizvodjac { get; set; }
        [DefaultValue(true)]
        public bool Aktivan { get; set; } = true;
        public byte[] Slika { get; set; }
        public IEnumerable<DioStanje> DioStanje { get; set; }
        public IEnumerable<OcjenaProizvoda> OcjenaProizvoda { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;

    }
}
