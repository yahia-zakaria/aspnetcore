using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTINCoumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TIN",
                table: "Persons",
                newName: "TaxIdentificationNumber");

            migrationBuilder.AlterColumn<string>(
                name: "TaxIdentificationNumber",
                table: "Persons",
                type: "varchar(8)",
                nullable: true,
                defaultValue: "ABC12345",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("337e4125-3830-4e62-91b0-cabf0752b3d4"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("60c318d6-94f0-4424-8d0d-c076e6aacd4f"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("64ce3461-96b5-4f6e-a091-7464d78f8142"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("6d875b26-7ed8-4353-ac1d-b61091f47fa9"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("c58dc233-6a37-42b8-bde1-a8b9a2420a09"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("e4d8086f-838c-42e4-8f51-50c4c2975cb5"),
                column: "TaxIdentificationNumber",
                value: "ABC12345");

            migrationBuilder.AddCheckConstraint(
                name: "chk_Persons_TINLenShoudBeEight",
                table: "Persons",
                sql: "len([TaxIdentificationNumber]) = 8");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "chk_Persons_TINLenShoudBeEight",
                table: "Persons");

            migrationBuilder.RenameColumn(
                name: "TaxIdentificationNumber",
                table: "Persons",
                newName: "TIN");

            migrationBuilder.AlterColumn<string>(
                name: "TIN",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(8)",
                oldNullable: true,
                oldDefaultValue: "ABC12345");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("337e4125-3830-4e62-91b0-cabf0752b3d4"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("60c318d6-94f0-4424-8d0d-c076e6aacd4f"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("64ce3461-96b5-4f6e-a091-7464d78f8142"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("6d875b26-7ed8-4353-ac1d-b61091f47fa9"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("c58dc233-6a37-42b8-bde1-a8b9a2420a09"),
                column: "TIN",
                value: null);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("e4d8086f-838c-42e4-8f51-50c4c2975cb5"),
                column: "TIN",
                value: null);
        }
    }
}
