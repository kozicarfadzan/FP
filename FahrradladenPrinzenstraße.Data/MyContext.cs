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

        public DbSet<Korisnik> Korisnik { get; set; }
        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<Zaposlenik> Zaposlenik { get; set; }
        public DbSet<AutorizacijskiToken> AutorizacijskiToken { get; set; }
        #endregion
    }
}
