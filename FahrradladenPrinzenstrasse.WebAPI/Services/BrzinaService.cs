using AutoMapper;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Model;
using FahrradladenPrinzenstrasse.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
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
                .Where(
                    x =>x.BiciklStanje
                    .Where(y => y.Aktivan)
                    .Any(y => y.Kolicina > 0))
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
