using FahrradladenPrinzenstrasse.Model;
using Syncfusion.DataSource.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FahrradladenPrinzenstrasse.Mobile.Models.PotvrdaNarudzbe
{
    public class DetaljiServisaMobile
    {
        public string Proizvodjac { get; set; }
        public string Model { get; set; }
        public string Boja { get; set; }
        public string Opis { get; set; }
        public string Tip { get; set; }
        public string DodatniTroskovi { get; set; }

        public string DetaljiText { get; set; }
        public ObservableCollection<string> TipoviBicikala { get; set; } = new ObservableCollection<string>();

    }
}
