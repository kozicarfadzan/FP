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
        }



        #region Tables


        public DbSet<Grad> Grad { get; set; }

        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<AutorizacijskiToken> AutorizacijskiToken { get; set; }
        public DbSet<BiciklStanje> BiciklStanje { get; set; }
        public DbSet<Bicikl> Bicikl { get; set; }
        public DbSet<Korisnik> Korisnik { get; set; }
        public DbSet<Zaposlenik> Zaposlenik { get; set; }
        public DbSet<Boja> Boja { get; set; }
        public DbSet<Model> Modeli { get; set; }
        public DbSet<Proizvodjac> Proizvodjac { get; set; }
        public DbSet<StarosnaGrupa> StarosnaGrupa { get; set; }
        public DbSet<VelicinaOkvira> VelicinaOkvira { get; set; }
        public DbSet<Lokacija> Lokacija { get; set; }
        #endregion
    }
}
