using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using Services.Mapping;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

#region ConfigureService
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IPersonService, PersonService>();
//DbContext
builder.Services.AddDbContext<PersonsDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});
#endregion

var app = builder.Build();

#region Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    
}
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Persons}/{action=Index}/{id?}");


app.Run();

#endregion