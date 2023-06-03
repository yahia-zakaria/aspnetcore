﻿using Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.Repository;
using Services;
using Services.Mapping;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

#region ConfigureService
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddTransient(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IPersonService, PersonService>();
//DbContext
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});
//Rotativa
RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
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