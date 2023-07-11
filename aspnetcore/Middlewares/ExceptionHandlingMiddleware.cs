using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;

namespace aspnetcore.Middlewares
{
	// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;
		private readonly IDiagnosticContext _diagnosticContext;

		public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, 
			IDiagnosticContext diagnosticContext)
		{
			_next = next;
			_logger = logger;
			_diagnosticContext = diagnosticContext;
		}

		public async Task Invoke(HttpContext context)
		{

			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				if (ex.InnerException is not null)
				{
					_logger.LogError("ExceptionType: {ExceptionType}, ExceptionMessage: {ExceptionMessage}", ex.InnerException.GetType().ToString(), ex.InnerException.Message);
				}
				else
				{
					_logger.LogError("ExceptionType: {ExceptionType}, ExceptionMessage: {ExceptionMessage}", ex.GetType().ToString(), ex.Message);
				}

				throw;
			}
		}
	}

	// Extension method used to add the middleware to the HTTP request pipeline.
	public static class ExceptionHandlingMiddlewareExtensions
	{
		public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<ExceptionHandlingMiddleware>();
		}
	}
}
