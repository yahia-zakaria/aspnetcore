using aspnetcore.Filters.ActionFilters;
using Entities;
using Infrastructure;
using Microsoft.AspNetCore.HttpLogging;
using Rotativa.AspNetCore;
using ServiceContracts.Repository;
using ServiceContracts;
using Services.Mapping;
using Services;
using Serilog;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore.Extensions
{
	public static class ConfigureServicesExtension
	{
		 public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, ConfigureHostBuilder host)
  {
			services.AddControllersWithViews(options =>
			{
				options.Filters.Add(new ReponseHeaderActionFilter("X-Custom-Key-FromGlobal", "Custom-Value-FromGlobal", 3));
			});
			services.AddAutoMapper(typeof(MappingProfile).Assembly);
			services.AddTransient(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
			services.AddTransient<IUnitOfWork, UnitOfWork>();
			services.AddScoped<ICountryService, CountryService>();
			services.AddScoped<IPersonService, PersonService>();
			services.AddHttpLogging(opt => { opt.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders | HttpLoggingFields.ResponsePropertiesAndHeaders; });

			host.UseSerilog((context, services, loggerConfiguration) =>
			{
				loggerConfiguration
				.ReadFrom.Configuration(context.Configuration)
				.ReadFrom.Services(services);
			});

			//DbContext
			services.AddDbContext<ApplicationDbContext>(opt =>
			{
				opt.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));
			});
	

			return services;
  }
	}
}
