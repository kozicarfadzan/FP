using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.WebAPI.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Data.EntityModels.Administrator, Model.Administrator>();
            CreateMap<Data.EntityModels.Bicikl, Model.Bicikl>();
            CreateMap<Data.EntityModels.BiciklStanje, Model.BiciklStanje>();
            CreateMap<Data.EntityModels.Boja, Model.Boja>();
            CreateMap<Data.EntityModels.Dio, Model.Dio>();
            CreateMap<Data.EntityModels.DioStanje, Model.DioStanje>();
            CreateMap<Data.EntityModels.Grad, Model.Grad>();
            CreateMap<Data.EntityModels.Klijent, Model.Klijent>();
            CreateMap<Data.EntityModels.Korisnik, Model.Korisnik>();
            CreateMap<Data.EntityModels.KorpaStavka, Model.KorpaStavka>();
            CreateMap<Data.EntityModels.Lokacija, Model.Lokacija>();
            CreateMap<Data.EntityModels.MaterijalOkvira, Model.MaterijalOkvira>();
            CreateMap<Data.EntityModels.Model, Model.Model>();
            CreateMap<Data.EntityModels.NacinOtpreme, Model.NacinOtpreme>();
            CreateMap<Data.EntityModels.Notifikacija, Model.Notifikacija>();
            CreateMap<Data.EntityModels.OcjenaProizvoda, Model.OcjenaProizvoda>();
            CreateMap<Data.EntityModels.Oprema, Model.Oprema>();
            CreateMap<Data.EntityModels.OpremaStanje, Model.OpremaStanje>();
            CreateMap<Data.EntityModels.Proizvodjac, Model.Proizvodjac>();
            CreateMap<Data.EntityModels.RadnoVrijeme, Model.RadnoVrijeme>();
            CreateMap<Data.EntityModels.Rezervacija, Model.Rezervacija>();
            CreateMap<Data.EntityModels.Rezervacija, Model.RezervacijaDetalji>();
            CreateMap<Data.EntityModels.RezervacijaIznajmljenaBicikla, Model.RezervacijaIznajmljenaBicikla>();
            CreateMap<Data.EntityModels.RezervacijaProdajaBicikla, Model.RezervacijaProdajaBicikla>();
            CreateMap<Data.EntityModels.RezervacijaProdajaDio, Model.RezervacijaProdajaDio>();
            CreateMap<Data.EntityModels.RezervacijaProdajaOprema, Model.RezervacijaProdajaOprema>();
            CreateMap<Data.EntityModels.RezervacijaServis, Model.RezervacijaServis>().ReverseMap();
            CreateMap<Data.EntityModels.Servis, Model.Servis>();
            CreateMap<Data.EntityModels.StarosnaGrupa, Model.StarosnaGrupa>();
            CreateMap<Data.EntityModels.TerminStavka, Model.TerminStavka>();
            CreateMap<Data.EntityModels.VelicinaOkvira, Model.VelicinaOkvira>();
            CreateMap<Data.EntityModels.Zaposlenik, Model.Zaposlenik>();

            CreateMap<Data.EntityModels.Korisnik, Model.Requests.KorisnikInsertRequest>().ReverseMap();
            CreateMap<Data.EntityModels.Korisnik, Model.Requests.KorisnikUpdateRequest>().ReverseMap();
            CreateMap<Data.EntityModels.KorpaStavka, Model.Requests.KorpaStavkaInsertRequest>().ReverseMap();
            CreateMap<Data.EntityModels.NacinOtpreme, Model.Requests.NacinOtpremeInsertRequest>().ReverseMap();
            CreateMap<Data.EntityModels.Rezervacija, Model.Requests.RezervacijaInsertRequest>().ReverseMap();
        }
    }
}
