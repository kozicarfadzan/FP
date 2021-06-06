using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model.Requests
{
    public class DioSearchRequest
    {


        // checkbox

        public List<int> ProizvodjacId { get; set; }

        public int Poredak { get; set; }
    }
}
