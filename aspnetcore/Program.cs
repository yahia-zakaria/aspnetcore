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
using aspnetcore.Extensions;
using aspnetcore.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration, builder.Host);

//Rotativa
if (builder.Environment.IsEnvironment("Test") == false)
	RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

var app = builder.Build();
app.UseSerilogRequestLogging();

#region Pipeline
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseExceptionHandlingMiddleware();
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