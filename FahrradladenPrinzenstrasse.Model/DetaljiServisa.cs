namespace FahrradladenPrinzenstrasse.Model
{
    public class DetaljiServisa
    {
        public string Proizvodjac { get; set; }
        public string Model { get; set; }
        public string Boja { get; set; }
        public string Opis { get; set; }
        public Tip Tip { get; set; }
        public string DodatniTroskovi { get; set; }
    }
}