using AutoMapper;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Model;
using FahrradladenPrinzenstraße.Model.Requests;
using FahrradladenPrinzenstraße.WebAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.WebAPI.Services
{
    public class TerminStavkaService : ITerminStavkaService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;
        private readonly IKorisnikService _korisnikService;

        public TerminStavkaService(MyContext context, IMapper mapper, IKorisnikService korisnikService)
        {
            _context = context;
            _mapper = mapper;
            _korisnikService = korisnikService;
        }
        public List<TerminStavka> Get()
        {
            var query = _context.TerminStavka.AsQueryable();

            query = query.Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.MaterijalOkvira)
                .Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.Proizvodjac)
                .Include(x => x.Bicikl).ThenInclude(x => x.StarosnaGrupa)
                .Include(x => x.Bicikl).ThenInclude(x => x.VelicinaOkvira)
                .Include(x => x.Bicikl).ThenInclude(x => x.Boja)
                .Where(x => x.KlijentId == _korisnikService.GetCurrentUser().Klijent.Id);

            var list = query.ToList();

            var model_list = _mapper.Map<List<Model.TerminStavka>>(list);
            return model_list;
        }

        public TerminStavka GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            var entity = _context.TerminStavka.Find(id);
            if (entity != null)
            {
                _context.Remove(entity);
                _context.SaveChanges();
            }
            return entity != null;
        }

        public int GetBrojStavki()
        {
            return _context.TerminStavka
                .Where(x => x.KlijentId == _korisnikService.GetCurrentUser().Klijent.Id)
                .Sum(x => x.Kolicina);
        }
    }
}
