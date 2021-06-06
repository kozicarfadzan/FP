using AutoMapper;
using FahrradladenPrinzenstrasse.Model;
using FahrradladenPrinzenstrasse.Model.Requests;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.WebAPI.Exceptions;
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

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public class DioService : IDioService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;

        public DioService(MyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<Model.Dio> Get(DioSearchRequest request)
        {
            var query = _context.Dio.AsQueryable();


            query = query.Include(x => x.Proizvodjac);
                     
                    

            query = query.Where(x => x.IsDeleted == false).Where(x => x.DioStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaDio.Count() == 0).Any())
                .Where(x => x.Aktivan);

            #region Filteri

            

       

            if (request.ProizvodjacId != null && request.ProizvodjacId.Count != 0)
            {
                query = query.Where(x => request.ProizvodjacId.Contains(x.ProizvodjacID));
            }



            #endregion

            if (request.Poredak != 0)
            {
                switch (request.Poredak)
                {
                    case 1: query = query.OrderBy(x => x.Naziv); break;
                    case 2: query = query.OrderByDescending(x => x.Naziv); break;
                    case 3: query = query.OrderByDescending(x => x.Cijena); break;
                    case 4: query = query.OrderBy(x => x.Cijena); break;
                }
            }

            var list = query.ToList();

            var model_list = _mapper.Map<List<Model.Dio>>(list);

            foreach (var item in model_list)
            {
                item.Ocjena = _context.OcjenaProizvoda.Where(x => x.DioId == item.DioId).Average(x => (double?)x.Ocjena) ?? 0.0;


                var SizesQuery = _context.Dio.Where(x => x.IsDeleted == false).Where(x => x.DioStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaDio.Count() == 0).Any())
                    .Where(x => x.Aktivan)
                    .AsQueryable();

               

               
            }

            return model_list;
        }

        public Model.Dio GetById(int id)
        {
            var entity = _context.Dio.Where(x => x.DioId == id)
                .Include(x => x.Proizvodjac) .FirstOrDefault();

            return _mapper.Map<Model.Dio>(entity);
        }

        

    }
}
