using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public interface IKorisnikService
    {
        //List<Model.Korisnik> Get(Model.Requests.KorisnikSearchRequest request);
        Model.Korisnik Insert(Model.Requests.KorisnikInsertRequest request);
        Model.Korisnik Update(int id, Model.Requests.KorisnikUpdateRequest request);
        Model.Korisnik GetById(int id);
        Model.Korisnik MyProfile();
        Model.Korisnik Authenticate(string username, string pass);

        Model.Korisnik GetCurrentUser();
        void SetCurrentUser(Model.Korisnik user);
    }
}
