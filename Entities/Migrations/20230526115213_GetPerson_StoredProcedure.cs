using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class GetPerson_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROC [dbo].[spGetPersons]
                                   AS
                                   BEGIN
                                   SELECT [Id], [PersonName], [Email], [DateOfBirth], [Gender], [CountryId], [Address], [ReceiveNewsLetters] FROM [dbo].[Persons]
                                   END");

		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROC [dbo].[spGetPersons]");
        }
    }
}
