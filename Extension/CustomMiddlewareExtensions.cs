using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tutor1.Middlewares;

namespace tutor1.Extension
{
    public static class CustomMiddlewareExtensions
    {
        public static void UseRequestResponseLogging(this IApplicationBuilder app) => app.UseMiddleware<RequestResponseLoggerMiddleware>();
    }
}
