﻿// <auto-generated />
using System;
using FahrradladenPrinzenstraße.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FahrradladenPrinzenstraße.Data.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20200309181703_Bicikl_BiciklStanje_Aktivan")]
    partial class Bicikl_BiciklStanje_Aktivan
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
                    b.Property<int>("Id");

                    b.HasKey("Id");

                    b.ToTable("Administrator");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.AutorizacijskiToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("KorisnikId");

                    b.Property<string>("Vrijednost");

                    b.Property<DateTime>("VrijemeEvidentiranja");

                    b.HasKey("Id");

                    b.HasIndex("KorisnikId");

                    b.ToTable("AutorizacijskiToken");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Bicikl", b =>
                {
                    b.Property<int>("BiciklId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Aktivan")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<int>("BojaId");

                    b.Property<double?>("Cijena");

                    b.Property<double?>("CijenaPoDanu");

                    b.Property<short>("GodinaProizvodnje");

                    b.Property<int>("ModelId");

                    b.Property<bool>("NoznaKocnica");

                    b.Property<string>("Slika");

                    b.Property<int>("Stanje");

                    b.HasKey("BiciklId");

                    b.HasIndex("BojaId");

                    b.HasIndex("ModelId");

                    b.ToTable("Bicikl");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.BiciklStanje", b =>
                {
                    b.Property<int>("BiciklStanjeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Aktivan")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<int>("BiciklId");

                    b.Property<int>("LokacijaId");

                    b.Property<string>("Sifra");

                    b.HasKey("BiciklStanjeId");

                    b.HasIndex("BiciklId");

                    b.HasIndex("LokacijaId");

                    b.ToTable("BiciklStanje");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Boja", b =>
                {
                    b.Property<int>("BojaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naziv");

                    b.HasKey("BojaId");

                    b.ToTable("Boja");
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

                    b.Property<string>("Spol");

                    b.HasKey("KorisnikID");

                    b.HasIndex("GradID");

                    b.ToTable("Korisnik");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Lokacija", b =>
                {
                    b.Property<int>("LokacijaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naziv");

                    b.HasKey("LokacijaId");

                    b.ToTable("Lokacija");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.MaterijalOkvira", b =>
                {
                    b.Property<int>("MaterijalOkviraId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naziv");

                    b.HasKey("MaterijalOkviraId");

                    b.ToTable("MaterijalOkvira");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Model", b =>
                {
                    b.Property<int>("ModelId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Brzina");

                    b.Property<int?>("MaterijalOkviraId");

                    b.Property<string>("Naziv");

                    b.Property<int>("ProizvodjacId");

                    b.Property<int>("SpolBicikl");

                    b.Property<int>("StarosnaGrupaId");

                    b.Property<int>("Suspenzija");

                    b.Property<int>("Tip");

                    b.Property<int>("VelicinaOkviraId");

                    b.HasKey("ModelId");

                    b.HasIndex("MaterijalOkviraId");

                    b.HasIndex("ProizvodjacId");

                    b.HasIndex("StarosnaGrupaId");

                    b.HasIndex("VelicinaOkviraId");

                    b.ToTable("Modeli");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Proizvodjac", b =>
                {
                    b.Property<int>("ProizvodjacId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naziv");

                    b.HasKey("ProizvodjacId");

                    b.ToTable("Proizvodjac");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.StarosnaGrupa", b =>
                {
                    b.Property<int>("StarosnaGrupaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naziv");

                    b.HasKey("StarosnaGrupaId");

                    b.ToTable("StarosnaGrupa");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.VelicinaOkvira", b =>
                {
                    b.Property<int>("VelicinaOkviraId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naziv");

                    b.HasKey("VelicinaOkviraId");

                    b.ToTable("VelicinaOkvira");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Zaposlenik", b =>
                {
                    b.Property<int>("Id");

                    b.HasKey("Id");

                    b.ToTable("Zaposlenik");
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Administrator", b =>
                {
                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.Korisnik", "Korisnik")
                        .WithOne("Administrator")
                        .HasForeignKey("FahrradladenPrinzenstraße.Data.EntityModels.Administrator", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.AutorizacijskiToken", b =>
                {
                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.Korisnik", "Korisnik")
                        .WithMany()
                        .HasForeignKey("KorisnikId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Bicikl", b =>
                {
                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.Boja", "Boja")
                        .WithMany()
                        .HasForeignKey("BojaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.Model", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.BiciklStanje", b =>
                {
                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.Bicikl", "Bicikl")
                        .WithMany("BiciklStanje")
                        .HasForeignKey("BiciklId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.Lokacija", "Lokacija")
                        .WithMany()
                        .HasForeignKey("LokacijaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Korisnik", b =>
                {
                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.Grad", "Grad")
                        .WithMany()
                        .HasForeignKey("GradID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Model", b =>
                {
                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.MaterijalOkvira", "MaterijalOkvira")
                        .WithMany()
                        .HasForeignKey("MaterijalOkviraId");

                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.Proizvodjac", "Proizvodjac")
                        .WithMany()
                        .HasForeignKey("ProizvodjacId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.StarosnaGrupa", "StarosnaGrupa")
                        .WithMany()
                        .HasForeignKey("StarosnaGrupaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.VelicinaOkvira", "VelicinaOkvira")
                        .WithMany()
                        .HasForeignKey("VelicinaOkviraId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FahrradladenPrinzenstraße.Data.EntityModels.Zaposlenik", b =>
                {
                    b.HasOne("FahrradladenPrinzenstraße.Data.EntityModels.Korisnik", "Korisnik")
                        .WithOne("Zaposlenik")
                        .HasForeignKey("FahrradladenPrinzenstraße.Data.EntityModels.Zaposlenik", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
