using System;
using System.Collections.Generic;
using System.Text;
using FahrradladenPrinzenstraße.Data.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace FahrradladenPrinzenstraße.Data
{

    public class MyContext : DbContext

    {
        public MyContext(DbContextOptions<MyContext> x) : base(x)
        {

        }

        public MyContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Zaposlenik>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Korisnik)
                                .WithOne(p => p.Zaposlenik)
                                .HasForeignKey<Zaposlenik>(d => d.Id);
            });

            modelBuilder.Entity<Administrator>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Korisnik)
                                .WithOne(p => p.Administrator)
                                .HasForeignKey<Administrator>(d => d.Id);
            });

            modelBuilder.Entity<Klijent>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Korisnik)
                                .WithOne(p => p.Klijent)
                                .HasForeignKey<Klijent>(d => d.Id);
            });

            modelBuilder.Entity<Bicikl>(entity =>
            {
                entity.Property(e => e.Aktivan)
                .HasDefaultValue(true);
            });
            modelBuilder.Entity<BiciklStanje>(entity =>
            {
                entity.Property(e => e.Aktivan)
                .HasDefaultValue(true);
            });
        }



  
        public DbSet<Grad> Grad { get; set; }

        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<AutorizacijskiToken> AutorizacijskiToken { get; set; }
        public DbSet<BiciklStanje> BiciklStanje { get; set; }
        public DbSet<Bicikl> Bicikl { get; set; }
        public DbSet<Korisnik> Korisnik { get; set; }
        public DbSet<Klijent> Klijent { get; set; }
        public DbSet<Zaposlenik> Zaposlenik { get; set; }
        public DbSet<Boja> Boja { get; set; }
        public DbSet<Model> Modeli { get; set; }
        public DbSet<Proizvodjac> Proizvodjac { get; set; }
        public DbSet<Rezervacija> Rezervacija { get; set; }
        public DbSet<RezervacijaIznajmljenaBicikla> RezervacijaIznajmljenaBicikla { get; set; }
        public DbSet<RezervacijaProdajaBicikla> RezervacijaProdajaBicikla { get; set; }
        public DbSet<RezervacijaProdajaDio> RezervacijaProdajaDio { get; set; }
        public DbSet<RezervacijaProdajaOprema> RezervacijaProdajaOprema { get; set; }
        public DbSet<Servis> Servis { get; set; }
        public DbSet<Dio> Dio { get; set; }
        public DbSet<DioStanje> DioStanje { get; set; }
        public DbSet<Oprema> Oprema { get; set; }
        public DbSet<OpremaStanje> OpremaStanje { get; set; }

        public DbSet<RezervacijaServis> RezervacijaServis { get; set; }
        public DbSet<MaterijalOkvira> MaterijalOkvira { get; set; }
        public DbSet<StarosnaGrupa> StarosnaGrupa { get; set; }
        public DbSet<VelicinaOkvira> VelicinaOkvira { get; set; }
        public DbSet<Lokacija> Lokacija { get; set; }
        public DbSet<KorpaStavka> KorpaStavka { get; set; }
        public DbSet<NacinOtpreme> NacinOtpreme { get; set; }
        public DbSet<RadnoVrijeme> RadnoVrijeme { get; set; }
        public DbSet<TerminStavka> TerminStavka { get; set; }
        public DbSet<Notifikacija> Notifikacija { get; set; }
        public DbSet<OcjenaProizvoda> OcjenaProizvoda { get; set; }
    }
}
