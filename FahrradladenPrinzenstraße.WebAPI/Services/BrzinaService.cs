using AutoMapper;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Model;
using FahrradladenPrinzenstraße.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FahrradladenPrinzenstraße.WebAPI.Services
{
    public class BrzinaService : IBrzinaService
    {
        private readonly MyContext _context;

        public BrzinaService(MyContext context)
        {
            _context = context;
        }
        public List<int> Get(BrzinaSearchRequest request)
        {
            var query = _context.Bicikl
                .Where(x => x.BiciklStanje.Where(y => y.Aktivan)
                .Where(y => y.RezervacijaProdajaBicikla.Count() == 0).Any())
                .Where(x => x.Aktivan);

            if (request.SamoIznajmljivanje)
                query = query.Where(x => x.Stanje == Data.EntityModels.Stanje.Korišteno);
            else
                query = query.Where(x => x.Stanje == Data.EntityModels.Stanje.Novo || x.Stanje == Data.EntityModels.Stanje.Polovno);

            var list = query
                .Select(x => x.Model.Brzina)
                .Distinct().ToList();

            return list;
        }

    }
}
