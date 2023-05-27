using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiveNewsLetters = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "CountryName" },
                values: new object[,]
                {
                    { new Guid("079c9af0-bea6-4407-b4cb-c960e8ceb4b6"), "South Korea" },
                    { new Guid("8f1da55f-7dfb-4caa-9785-6f901336d6dc"), "UK" },
                    { new Guid("a7872c03-9643-47d1-ab56-f603f2aba8b1"), "USA" },
                    { new Guid("b3e3c9a0-0925-4493-9e24-569c89a58ead"), "Canada" },
                    { new Guid("f225ccca-10c7-44bd-886a-8d0ea28ed1c3"), "Austrailia" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Address", "CountryId", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetters" },
                values: new object[,]
                {
                    { new Guid("337e4125-3830-4e62-91b0-cabf0752b3d4"), "USA, Washitone DC", new Guid("a7872c03-9643-47d1-ab56-f603f2aba8b1"), new DateTime(2002, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "b@b.com", "Male", "Ali Osman", true },
                    { new Guid("60c318d6-94f0-4424-8d0d-c076e6aacd4f"), "Canada, Ontario", new Guid("b3e3c9a0-0925-4493-9e24-569c89a58ead"), new DateTime(1982, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "c@c.com", "Female", "Anna Ahmed", true },
                    { new Guid("64ce3461-96b5-4f6e-a091-7464d78f8142"), "South Korea, Seoul", new Guid("079c9af0-bea6-4407-b4cb-c960e8ceb4b6"), new DateTime(1993, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "k@k.com", "Male", "Aiham KhIdr", true },
                    { new Guid("6d875b26-7ed8-4353-ac1d-b61091f47fa9"), "UK Liverpool", new Guid("8f1da55f-7dfb-4caa-9785-6f901336d6dc"), new DateTime(1995, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "a@a.com", "Male", "Yahia Zakaria", true },
                    { new Guid("c58dc233-6a37-42b8-bde1-a8b9a2420a09"), "Australia melbourne", new Guid("f225ccca-10c7-44bd-886a-8d0ea28ed1c3"), new DateTime(2000, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "n@n.com", "Male", "Naif Ahmed", true },
                    { new Guid("e4d8086f-838c-42e4-8f51-50c4c2975cb5"), "Australia melbourne", new Guid("f225ccca-10c7-44bd-886a-8d0ea28ed1c3"), new DateTime(1981, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "d@d.com", "Female", "Gidaa Zakaria", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
