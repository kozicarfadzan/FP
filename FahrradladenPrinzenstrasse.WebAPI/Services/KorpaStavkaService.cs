using AutoMapper;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Model;
using FahrradladenPrinzenstrasse.Model.Requests;
using FahrradladenPrinzenstrasse.WebAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public class KorpaStavkaService : IKorpaStavkaService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;
        private readonly IKorisnikService _korisnikService;

        public KorpaStavkaService(MyContext context, IMapper mapper, IKorisnikService korisnikService)
        {
            _context = context;
            _mapper = mapper;
            _korisnikService = korisnikService;
        }
        public List<KorpaStavka> Get()
        {
            var query = _context.KorpaStavka.AsQueryable();

            query = query.Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.MaterijalOkvira)
                .Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.Proizvodjac)
                .Include(x => x.Bicikl).ThenInclude(x => x.StarosnaGrupa)
                .Include(x => x.Bicikl).ThenInclude(x => x.VelicinaOkvira)
                .Include(x => x.Bicikl).ThenInclude(x => x.Boja)
                .Include(x => x.Dio).ThenInclude(x => x.Proizvodjac)
                .Include(x => x.Oprema).ThenInclude(x => x.Proizvodjac)
                .Where(x => x.KlijentId == _korisnikService.GetCurrentUser().Klijent.Id);

            var list = query.ToList();

            var model_list = _mapper.Map<List<Model.KorpaStavka>>(list);

            foreach (var item in model_list)
            {
                if (item.Bicikl != null)
                {
                    item.Ocjena = _context.OcjenaProizvoda.Where(x => x.BiciklId == item.BiciklId).Average(x => (double?)x.Ocjena) ?? 0.0;
                    item.Bicikl.Kolicina = _context.BiciklStanje.Where(x => x.BiciklId == item.BiciklId && x.Aktivan && x.KupacId == null).Count();
                }
                else if (item.Dio != null)
                {
                    item.Ocjena = _context.OcjenaProizvoda.Where(x => x.DioId == item.DioId).Average(x => (double?)x.Ocjena) ?? 0.0;
                    item.Dio.Kolicina = _context.DioStanje.Where(x => x.DioId == item.DioId && x.Aktivan && x.KupacId == null).Count();
                }
                else if (item.Oprema != null)
                {
                    item.Ocjena = _context.OcjenaProizvoda.Where(x => x.OpremaId == item.OpremaId).Average(x => (double?)x.Ocjena) ?? 0.0;
                    item.Oprema.Kolicina = _context.OpremaStanje.Where(x => x.OpremaId == item.OpremaId && x.Aktivan && x.KupacId == null).Count();

                }
            }

            return model_list;
        }

        public KorpaStavka GetById(int id)
        {
            throw new NotImplementedException();
        }

        public KorpaStavka Insert(KorpaStavkaInsertRequest request)
        {
            var StavkaQry = _context.KorpaStavka.Where(x => x.KlijentId == _korisnikService.GetCurrentUser().Klijent.Id).AsQueryable();
            var UkupnoUSkladistu = 0;
            if (request.BiciklId != null)
            {
                StavkaQry = StavkaQry.Where(x => x.BiciklId == request.BiciklId);
                UkupnoUSkladistu = _context.BiciklStanje.Where(x => x.BiciklId == request.BiciklId && x.Aktivan && x.KupacId == null).Count();
            }

            else if (request.DioId != null)
            {
                StavkaQry = StavkaQry.Where(x => x.DioId == request.DioId);
                UkupnoUSkladistu = _context.DioStanje.Where(x => x.DioId == request.DioId && x.Aktivan && x.KupacId == null).Count();
            }

            else if (request.OpremaId != null)
            {
                StavkaQry = StavkaQry.Where(x => x.OpremaId == request.OpremaId);
                UkupnoUSkladistu = _context.OpremaStanje.Where(x => x.OpremaId == request.OpremaId && x.Aktivan && x.KupacId == null).Count();
            }

            var entity = StavkaQry.FirstOrDefault();
            if (entity != null)
            {
                if (entity.Kolicina + request.Kolicina > UkupnoUSkladistu)
                    throw new UserException("Proizvod nije dostupan u traženoj količini.");

                entity.Kolicina += request.Kolicina;
            }
            else
            {
                if (UkupnoUSkladistu < request.Kolicina)
                    throw new UserException("Proizvod nije dostupan u traženoj količini.");

                entity = _mapper.Map<Data.EntityModels.KorpaStavka>(request);
                entity.KlijentId = _korisnikService.GetCurrentUser().Klijent.Id;
                _context.KorpaStavka.Add(entity);
            }
            _context.SaveChanges();

            return _mapper.Map<Model.KorpaStavka>(entity);
        }

        public KorpaStavka Update(int id, KorpaStavkaInsertRequest request)
        {
            var entity = _context.KorpaStavka.Find(id);
            var UkupnoUSkladistu = 0;
            if (request.BiciklId != null)
            {
                UkupnoUSkladistu = _context.BiciklStanje.Where(x => x.BiciklId == request.BiciklId && x.Aktivan && x.KupacId == null).Count();
            }
            else if (request.DioId != null)
            {
                UkupnoUSkladistu = _context.DioStanje.Where(x => x.DioId == request.DioId && x.Aktivan && x.KupacId == null).Count();
            }

            else if (request.OpremaId != null)
            {
                UkupnoUSkladistu = _context.OpremaStanje.Where(x => x.OpremaId == request.OpremaId && x.Aktivan && x.KupacId == null).Count();
            }

            if (UkupnoUSkladistu < request.Kolicina)
                throw new UserException("Proizvod nije dostupan u traženoj količini.");

            entity.Kolicina = request.Kolicina;

            _context.SaveChanges();

            return _mapper.Map<Model.KorpaStavka>(entity);
        }
        public bool Delete(int id)
        {
            var entity = _context.KorpaStavka.Find(id);
            if (entity != null)
            {
                _context.Remove(entity);
                _context.SaveChanges();
            }
            return entity != null;
        }

        public int GetBrojStavki()
        {
            return _context.KorpaStavka
                .Where(x => x.KlijentId == _korisnikService.GetCurrentUser().Klijent.Id)
                .Sum(x => x.Kolicina);
        }
    }
}
