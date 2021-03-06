﻿// <auto-generated />
using FahrradladenPrinzenstraße.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20191127111955_Inicijalna")]
    partial class Inicijalna
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Administrator", b =>
                {
                    b.Property<int>("AdministratorID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("KorisnikID");

                    b.HasKey("AdministratorID");

                    b.HasIndex("KorisnikID");

                    b.ToTable("Administrator");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Grad", b =>
                {
                    b.Property<int>("GradID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naziv");

                    b.HasKey("GradID");

                    b.ToTable("Grad");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Korisnik", b =>
                {
                    b.Property<int>("KorisnikID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdresaStanovanja");

                    b.Property<bool>("Aktivan");

                    b.Property<string>("BrojTelefona");

                    b.Property<string>("Email");

                    b.Property<int>("GradID");

                    b.Property<string>("Ime");

                    b.Property<string>("KorisnickoIme");

                    b.Property<string>("LozinkaHash");

                    b.Property<string>("LozinkaSalt");

                    b.Property<string>("Prezime");

                    b.Property<int>("SpolID");

                    b.HasKey("KorisnikID");

                    b.HasIndex("GradID");

                    b.HasIndex("SpolID");

                    b.ToTable("Korisnik");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Spol", b =>
                {
                    b.Property<int>("SpolID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naziv");

                    b.HasKey("SpolID");

                    b.ToTable("Spol");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Zaposlenik", b =>
                {
                    b.Property<int>("ZaposlenikID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("KorisnikID");

                    b.HasKey("ZaposlenikID");

                    b.HasIndex("KorisnikID");

                    b.ToTable("Zaposlenik");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Administrator", b =>
                {
                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.Korisnik", "Korisnik")
                        .WithMany()
                        .HasForeignKey("KorisnikID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Korisnik", b =>
                {
                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.Grad", "Grad")
                        .WithMany()
                        .HasForeignKey("GradID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.Spol", "Spol")
                        .WithMany()
                        .HasForeignKey("SpolID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Zaposlenik", b =>
                {
                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.Korisnik", "Korisnik")
                        .WithMany()
                        .HasForeignKey("KorisnikID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
