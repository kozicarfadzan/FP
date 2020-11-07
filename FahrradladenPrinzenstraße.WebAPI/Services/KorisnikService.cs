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

namespace FahrradladenPrinzenstraße.WebAPI.Services
{
    public class KorisnikService : IKorisnikService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;

        private Model.Korisnik _currentUser;

        public KorisnikService(MyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //public List<Model.Korisnik> Get(UsersSearchRequest request)
        //{
        //    var query = _context.Korisnik.AsQueryable();

        //    if (!string.IsNullOrWhiteSpace(request?.Search))
        //        query = query.Where(x => x.KorisnickoIme.Contains(request.Search) ||
        //        x.Email.Contains(request.Search) ||
        //        x.Ime.Contains(request.Search) ||
        //        x.Prezime.Contains(request.Search)
        //        );

        //    query = query.Include(x => x.Administrator);
        //    query = query.Include(x => x.Zaposlenik);
        //    query = query.Include(x => x.Klijent);
        //    var list = query.ToList();

        //    return _mapper.Map<List<Model.Korisnik>>(list);
        //}

        public Model.Korisnik GetById(int id)
        {
            var entity = _context.Korisnik.Where(x => x.KorisnikID == id).Include(x => x.Administrator).Include(x => x.Zaposlenik).Include(x => x.Klijent).FirstOrDefault();


            return _mapper.Map<Model.Korisnik>(entity);
        }

        public Model.Korisnik Insert(KorisnikInsertRequest request)
        {
            var entity = _mapper.Map<Data.EntityModels.Korisnik>(request);
            if (request.Lozinka != request.LozinkaPotvrda)
            {
                throw new UserException("Lozinke se ne podudaraju.");
            }
            if (CheckUsernameExists(request.KorisnickoIme))
            {
                throw new UserException("Korisničko ime je već u upotrebi.");
            }
            if (CheckEmailExists(request.Email))
            {
                throw new UserException("Email adresa je već u upotrebi.");
            }

            entity.LozinkaSalt = GenerateSalt();
            entity.LozinkaHash = GenerateHash(entity.LozinkaSalt, request.Lozinka);

            _context.Korisnik.Add(entity);
            _context.SaveChanges();

            return _mapper.Map<Model.Korisnik>(entity);
        }

        public Model.Korisnik Update(int id, KorisnikUpdateRequest request)
        {
            var entity = _context.Korisnik.Find(id);

            _context.Korisnik.Attach(entity);
            _context.Korisnik.Update(entity);
            if (!string.IsNullOrWhiteSpace(request.Lozinka))
            {
                if (request.Lozinka != request.LozinkaPotvrda)
                {
                    throw new UserException("Lozinke se ne podudaraju.");
                }

                entity.LozinkaSalt = GenerateSalt();
                entity.LozinkaHash = GenerateHash(entity.LozinkaSalt, request.Lozinka);
            }
            if (entity.KorisnickoIme != request.Lozinka && CheckUsernameExists(request.KorisnickoIme))
            {
                throw new UserException("Korisničko ime je već u upotrebi.");
            }
            if (entity.Email != request.Email && CheckEmailExists(request.Email))
            {
                throw new UserException("Email adresa je već u upotrebi.");
            }

            _mapper.Map(request, entity);

            _context.SaveChanges();

            return _mapper.Map<Model.Korisnik>(entity);
        }


        public static string GenerateSalt()
        {
            var buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }

        public static string GenerateHash(string salt, string password)
        {
            byte[] src = Convert.FromBase64String(salt);
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] dst = new byte[src.Length + bytes.Length];

            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }

        public Model.Korisnik MyProfile()
        {
            var query = _context.Korisnik.AsQueryable();

            query = query.Where(x => x.KorisnikID == _currentUser.KorisnikID);

            query = query.Include(x => x.Administrator);
            query = query.Include(x => x.Zaposlenik);
            query = query.Include(x => x.Klijent);
            query = query.Include(x => x.Grad);

            var entity = query.FirstOrDefault();

            return _mapper.Map<Model.Korisnik>(entity);
        }

        public Model.Korisnik Authenticate(string username, string pass)
        {
            var user = _context.Korisnik
                         .Include(x => x.Administrator)
                         .Include(x => x.Zaposlenik)
                         .Include(x => x.Klijent)
                         .FirstOrDefault(x => x.KorisnickoIme == username);

            if (user != null)
            {
                var newHash = GenerateHash(user.LozinkaSalt, pass);

                if (newHash == user.LozinkaHash)
                {
                    _context.SaveChanges();
                    return _mapper.Map<Model.Korisnik>(user);
                }
            }
            return null;
        }

        public void SetCurrentUser(Model.Korisnik user)
        {
            _currentUser = user;
        }
        public Model.Korisnik GetCurrentUser()
        {
            return _currentUser;
        }

        public bool CheckUsernameExists(string username)
        {
            return _context.Korisnik.Any(x => x.KorisnickoIme == username);
        }
        public bool CheckEmailExists(string email)
        {
            return _context.Korisnik.Any(x => x.Email == email);
        }
    }
}
