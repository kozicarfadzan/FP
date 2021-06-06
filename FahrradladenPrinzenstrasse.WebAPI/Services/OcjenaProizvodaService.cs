using AutoMapper;
using FahrradladenPrinzenstrasse.Model;
using FahrradladenPrinzenstrasse.Model.Requests;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.WebAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public class OcjenaProizvodaService : IOcjenaProizvodaService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;
        private readonly IKorisnikService _korisnikService;

        public OcjenaProizvodaService(MyContext context, IMapper mapper, IKorisnikService korisnikService)
        {
            _context = context;
            _mapper = mapper;
            _korisnikService = korisnikService;
        }

        public bool OcijeniProizvod(OcjenaKorisnikaInsertRequest request)
        {
            var Klijent = _korisnikService.GetCurrentUser().Klijent;

            if (request.Ocjena < 1 || request.Ocjena > 5)
            {
                throw new UserException("Neispravna ocjena.");
            }

            var postojeca_ocjena_qry = _context.OcjenaProizvoda.Where(x => x.KlijentId == Klijent.Id);
            var ocjena_proizvoda = new Data.EntityModels.OcjenaProizvoda
            {
                KlijentId = Klijent.Id,
                Ocjena = request.Ocjena,
                DatumOcjene = DateTime.Now
            };

            bool kupio_bicikl = false, kupio_dio = false, kupio_opremu = false;

            if (request.BiciklId  != null)
            {
                kupio_bicikl = _context.RezervacijaProdajaBicikla.Where(x => x.BiciklStanje.BiciklId == request.BiciklId)
                .Where(x => x.Rezervacija.KlijentId == Klijent.Id)
                .Any();
                if (!kupio_bicikl)
                {
                    kupio_bicikl = _context.RezervacijaIznajmljenaBicikla.Where(x => x.BiciklStanje.BiciklId == request.BiciklId)
                    .Where(x => x.Rezervacija.KlijentId == Klijent.Id)
                    .Any();
                }

                postojeca_ocjena_qry = postojeca_ocjena_qry.Where(x => x.BiciklId == request.BiciklId);
                ocjena_proizvoda.BiciklId = request.BiciklId;
            }
            else if (request.DioId  != null)
            {
                kupio_dio = _context.RezervacijaProdajaDio.Where(x => x.DioStanje.DioId == request.DioId)
                .Where(x => x.Rezervacija.KlijentId == Klijent.Id)
                .Any();

                postojeca_ocjena_qry = postojeca_ocjena_qry.Where(x => x.DioId == request.DioId);
                ocjena_proizvoda.DioId = request.DioId;
            }
            
            else if (request.OpremaId  != null)
            {
                kupio_opremu = _context.RezervacijaProdajaOprema.Where(x => x.OpremaStanje.OpremaId == request.OpremaId)
                .Where(x => x.Rezervacija.KlijentId == Klijent.Id)
                .Any();

                postojeca_ocjena_qry = postojeca_ocjena_qry.Where(x => x.OpremaId == request.OpremaId);
                ocjena_proizvoda.OpremaId = request.OpremaId;
            }

            if (kupio_bicikl || kupio_dio || kupio_opremu)
            {
                var postojeca_ocjena = postojeca_ocjena_qry.FirstOrDefault();
                if (postojeca_ocjena != null)
                {
                    postojeca_ocjena.Ocjena = request.Ocjena;
                    postojeca_ocjena.DatumOcjene = DateTime.Now;
                }
                else
                {
                    _context.OcjenaProizvoda.Add(ocjena_proizvoda);
                }

                _context.SaveChanges();
                return true;
            }
            throw new UserException("Ne možete ocijeniti proizvod koji niste kupili.");
        }

    }
}
