using AutoMapper;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public class RecommenderService : IRecommenderService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;
        private readonly IKorisnikService korisnikService;

        public RecommenderService(MyContext context, IMapper mapper, IKorisnikService korisnikService)
        {
            _context = context;
            _mapper = mapper;
            this.korisnikService = korisnikService;
        }

        public List<PreporuceniProizvod> Get()
        {
            var PopularniProizvodi = new List<PreporuceniProizvod>();

            var popularna_bicikla = _context.Bicikl.Where(x => (x.Stanje == Data.EntityModels.Stanje.Novo || x.Stanje == Data.EntityModels.Stanje.Polovno) && x.OcjenaProizvoda.Any())
                .Where(x => x.BiciklStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaBicikla.Count() == 0).Any())
                .Where(x => x.Aktivan)
                .Include(x => x.Model.Proizvodjac)
                .OrderByDescending(x => x.OcjenaProizvoda.Average(x => x.Ocjena))
                .Take(2)
                .Select(x => new PreporuceniProizvod
                {
                    Id = x.BiciklId,
                    Naziv = x.PuniNaziv,
                    Cijena = x.Cijena.Value,
                    Slika = x.Slika,
                    Tip = TipProizvoda.Bicikl
                }).ToList();

            var popularni_dijelovi = _context.Dio.Where(x => x.IsDeleted == false).Where(x => x.OcjenaProizvoda.Any())
                .Where(x => x.DioStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaDio.Count() == 0).Any())
                .Where(x => x.Aktivan)
                .OrderByDescending(x => x.OcjenaProizvoda.Average(x => x.Ocjena))
                .Take(2)
                .Select(x => new PreporuceniProizvod
                {
                    Id = x.DioId,
                    Naziv = x.Naziv,
                    Cijena = x.Cijena,
                    Slika = x.Slika,
                    Tip = TipProizvoda.Dio
                }).ToList();

            var popularna_oprema = _context.Oprema.Where(x => x.IsDeleted == false).Where(x => x.OcjenaProizvoda.Any())
                .Where(x => x.OpremaStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaOprema.Count() == 0).Any())
                .Where(x => x.Aktivan)
                .OrderByDescending(x => x.OcjenaProizvoda.Average(x => x.Ocjena))
                .Take(2)
                .Select(x => new PreporuceniProizvod
                {
                    Id = x.OpremaId,
                    Naziv = x.Naziv,
                    Cijena = x.Cijena,
                    Slika = x.Slika,
                    Tip = TipProizvoda.Oprema
                }).ToList();

            PopularniProizvodi.AddRange(popularna_bicikla);
            PopularniProizvodi.AddRange(popularni_dijelovi);
            PopularniProizvodi.AddRange(popularna_oprema);

            return PopularniProizvodi;
        }
    }
}
