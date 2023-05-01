using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace aspnetcore
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            string username = httpContext.Request.Query.ContainsKey("username") ?
                httpContext.Request.Query["username"].ToString() : string.Empty;
            string pass = httpContext.Request.Query.ContainsKey("pass") ?
             httpContext.Request.Query["pass"].ToString() : string.Empty;
            if (username == "admin" && pass == "123")
            {
                await httpContext.Response.WriteAsync("Logged in successfully!");
            }
            else
            {
                await httpContext.Response.WriteAsync("Username or password is invalid");
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoginMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogin(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoginMiddleware>();
        }
    }
}
