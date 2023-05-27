﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entities.Migrations
{
    [DbContext(typeof(PersonsDbContext))]
    [Migration("20230526115213_GetPerson_StoredProcedure")]
    partial class GetPerson_StoredProcedure
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Countries", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("a7872c03-9643-47d1-ab56-f603f2aba8b1"),
                            CountryName = "USA"
                        },
                        new
                        {
                            Id = new Guid("8f1da55f-7dfb-4caa-9785-6f901336d6dc"),
                            CountryName = "UK"
                        },
                        new
                        {
                            Id = new Guid("f225ccca-10c7-44bd-886a-8d0ea28ed1c3"),
                            CountryName = "Austrailia"
                        },
                        new
                        {
                            Id = new Guid("b3e3c9a0-0925-4493-9e24-569c89a58ead"),
                            CountryName = "Canada"
                        },
                        new
                        {
                            Id = new Guid("079c9af0-bea6-4407-b4cb-c960e8ceb4b6"),
                            CountryName = "South Korea"
                        });
                });

            modelBuilder.Entity("Entities.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ReceiveNewsLetters")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Persons", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("6d875b26-7ed8-4353-ac1d-b61091f47fa9"),
                            Address = "UK Liverpool",
                            CountryId = new Guid("8f1da55f-7dfb-4caa-9785-6f901336d6dc"),
                            DateOfBirth = new DateTime(1995, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "a@a.com",
                            Gender = "Male",
                            PersonName = "Yahia Zakaria",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            Id = new Guid("337e4125-3830-4e62-91b0-cabf0752b3d4"),
                            Address = "USA, Washitone DC",
                            CountryId = new Guid("a7872c03-9643-47d1-ab56-f603f2aba8b1"),
                            DateOfBirth = new DateTime(2002, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "b@b.com",
                            Gender = "Male",
                            PersonName = "Ali Osman",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            Id = new Guid("c58dc233-6a37-42b8-bde1-a8b9a2420a09"),
                            Address = "Australia melbourne",
                            CountryId = new Guid("f225ccca-10c7-44bd-886a-8d0ea28ed1c3"),
                            DateOfBirth = new DateTime(2000, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "n@n.com",
                            Gender = "Male",
                            PersonName = "Naif Ahmed",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            Id = new Guid("e4d8086f-838c-42e4-8f51-50c4c2975cb5"),
                            Address = "Australia melbourne",
                            CountryId = new Guid("f225ccca-10c7-44bd-886a-8d0ea28ed1c3"),
                            DateOfBirth = new DateTime(1981, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "d@d.com",
                            Gender = "Female",
                            PersonName = "Gidaa Zakaria",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            Id = new Guid("60c318d6-94f0-4424-8d0d-c076e6aacd4f"),
                            Address = "Canada, Ontario",
                            CountryId = new Guid("b3e3c9a0-0925-4493-9e24-569c89a58ead"),
                            DateOfBirth = new DateTime(1982, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "c@c.com",
                            Gender = "Female",
                            PersonName = "Anna Ahmed",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            Id = new Guid("64ce3461-96b5-4f6e-a091-7464d78f8142"),
                            Address = "South Korea, Seoul",
                            CountryId = new Guid("079c9af0-bea6-4407-b4cb-c960e8ceb4b6"),
                            DateOfBirth = new DateTime(1993, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "k@k.com",
                            Gender = "Male",
                            PersonName = "Aiham KhIdr",
                            ReceiveNewsLetters = true
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
