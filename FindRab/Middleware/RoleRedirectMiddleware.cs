using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FindRab.Middleware
{
    public class RoleRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var roleClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (roleClaim != null)
                {
                    var path = context.Request.Path.Value.ToLower();
                    if (string.IsNullOrEmpty(path) || path == "/")
                    {
                        if (roleClaim == "1")
                        {
                            context.Response.Redirect("/AdminView/Index");
                            return;
                        }
                        else if (roleClaim == "2")
                        {
                            context.Response.Redirect("/Menu/Index");
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}

