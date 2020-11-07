using AutoMapper;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Model;
using FahrradladenPrinzenstraße.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FahrradladenPrinzenstraße.WebAPI.Services
{
    public class NacinOtpremeService : INacinOtpremeService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;

        public NacinOtpremeService(MyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<NacinOtpreme> Get()
        {
            var list = _context.NacinOtpreme.ToList();

            return _mapper.Map<List<Model.NacinOtpreme>>(list);
        }

        public NacinOtpreme GetById(int id)
        {
            var entity = _context.NacinOtpreme.Find(id);
            return _mapper.Map<Model.NacinOtpreme>(entity);
        }

        public NacinOtpreme Insert(NacinOtpremeInsertRequest request)
        {
            var entity = _mapper.Map<Data.EntityModels.NacinOtpreme>(request);
            _context.NacinOtpreme.Add(entity);
            _context.SaveChanges();

            return _mapper.Map<Model.NacinOtpreme>(entity);
        }

        public NacinOtpreme Update(int id, NacinOtpremeInsertRequest request)
        {
            var entity = _context.NacinOtpreme.Find(id);
            _mapper.Map(request, entity);

            _context.SaveChanges();

            return _mapper.Map<Model.NacinOtpreme>(entity);
        }
        public bool Delete(int id)
        {
            var entity = _context.NacinOtpreme.Find(id);
            if (entity != null)
            {
                _context.Remove(entity);
                _context.SaveChanges();
            }
            return entity != null;
        }
    }
}
