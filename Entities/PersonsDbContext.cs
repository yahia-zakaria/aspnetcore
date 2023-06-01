using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entities
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		public virtual DbSet<Country> Countries { get; set; }
		public virtual DbSet<Person> Persons { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			#region Table names
			modelBuilder.Entity<Country>().ToTable("Countries");
			modelBuilder.Entity<Person>().ToTable("Persons");

			#endregion

			#region Seed
			/*seed countries*/
			var countriesFileText = File.ReadAllText("Countries.json");
			var countries = JsonSerializer.Deserialize<List<Country>>(countriesFileText);
			foreach (var country in countries)
			{
				modelBuilder.Entity<Country>().HasData(country);
			}

			/*seed persons*/
			var personsFileText = File.ReadAllText("Persons.json");
			var persons = JsonSerializer.Deserialize<List<Person>>(personsFileText);
			foreach (var person in persons)
			{
				modelBuilder.Entity<Person>().HasData(person);
			}

			#endregion

			#region fluentAPI
			modelBuilder.Entity<Person>().Property(prop => prop.TIN)
				.HasColumnName("TaxIdentificationNumber")
				.HasColumnType("varchar(8)")
				.HasDefaultValue("ABC12345");

			//modelBuilder.Entity<Person>().HasIndex(property=>property.TIN).IsUnique();
			modelBuilder.Entity<Person>().ToTable(
				b=>b.HasCheckConstraint("chk_Persons_TINLenShoudBeEight", "len([TaxIdentificationNumber]) = 8"));
			#endregion
		}

		public List<Person> sp_GetAllPersons()
		{
			return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
		}

		public int sp_InsertPerson(Person person)
		{
			SqlParameter[] parameters = new SqlParameter[] {
		new SqlParameter("@Id", person.Id),
		new SqlParameter("@PersonName", person.PersonName),
		new SqlParameter("@Email", person.Email),
		new SqlParameter("@DateOfBirth", person.DateOfBirth),
		new SqlParameter("@Gender", person.Gender),
		new SqlParameter("@CountryId", person.CountryId),
		new SqlParameter("@Address", person.Address),
		new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)
	  };

			return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @Id, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetters", parameters);
		}
	}
}
