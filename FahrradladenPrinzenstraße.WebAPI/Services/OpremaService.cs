using AutoMapper;
using FahrradladenPrinzenstraße.Model;
using FahrradladenPrinzenstraße.Model.Requests;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.WebAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Storage;

namespace FahrradladenPrinzenstraße.WebAPI.Services
{
    public class OpremaService : IOpremaService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;

        public OpremaService(MyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Model.Oprema> Get(OpremaSearchRequest request)
        {
            var query = _context.Oprema.AsQueryable();


            query = query.Include(x => x.Proizvodjac);
                     
                    

            query = query.Where(x => x.OpremaStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaOprema.Count() == 0).Any())
                .Where(x => x.Aktivan);

            #region Filteri

            

       

            if (request.ProizvodjacId != null && request.ProizvodjacId.Count != 0)
            {
                query = query.Where(x => request.ProizvodjacId.Contains(x.ProizvodjacID));
            }

          

            #endregion

            var list = query.ToList();

            var model_list = _mapper.Map<List<Model.Oprema>>(list);

            foreach (var item in model_list)
            {
                item.Ocjena = _context.OcjenaProizvoda.Where(x => x.OpremaId == item.OpremaId).Average(x => (double?)x.Ocjena) ?? 0.0;


                var SizesQuery = _context.Oprema.Where(x => x.OpremaStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaOprema.Count() == 0).Any())
                    .Where(x => x.Aktivan)
                    .AsQueryable();

               

               
            }

            return model_list;
        }

        public Model.Oprema GetById(int id)
        {
            var entity = _context.Oprema.Where(x => x.OpremaId == id)
                .Include(x => x.Proizvodjac) .FirstOrDefault();

            return _mapper.Map<Model.Oprema>(entity);
        }

        

    }
}
