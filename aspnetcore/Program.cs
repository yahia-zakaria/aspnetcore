using Entities;
using Infrastructure;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.Repository;
using Services;
using Services.Mapping;
using System.Reflection;
using Serilog;
using aspnetcore.Filters.ActionFilters;

var builder = WebApplication.CreateBuilder(args);

#region ConfigureService
builder.Services.AddControllersWithViews(options =>
{
    var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ReponseHeaderActionFilter>>();
    options.Filters.Add(new ReponseHeaderActionFilter(logger, "X-Custom-Key-FromGlobal", "Custom-Value-FromGlobal"));
});
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddTransient(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddHttpLogging(opt => { opt.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders | HttpLoggingFields.ResponsePropertiesAndHeaders; });

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services);
});

//DbContext
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});
//Rotativa
if (builder.Environment.IsEnvironment("Test") == false)
    RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
#endregion

var app = builder.Build();
app.UseSerilogRequestLogging();

#region Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseHttpLogging();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Persons}/{action=Index}/{id?}");


app.Run();

#endregion

public partial class Program { }