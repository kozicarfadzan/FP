using FahrradladenPrinzenstrasse.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Helper
{
    public static class KorisnikHelper
    {
        public static void SetLozinka(this Korisnik korisnik, string novaLozinka)
        {
            korisnik.LozinkaSalt = PasswordHelper.GenerateSalt();
            korisnik.LozinkaHash = PasswordHelper.GenerateHash(korisnik.LozinkaSalt, novaLozinka);
        }
    }
}
