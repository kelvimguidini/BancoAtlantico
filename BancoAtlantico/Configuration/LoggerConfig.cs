using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KissLog;
using KissLog.AspNetCore;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System;
using System.Diagnostics;

namespace flights.api.Configuration
{
    public static class LoggerConfig
    {
        public static void AddLoggingConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ILogger>((context) =>
            {
                return Logger.Factory.Get();
            });

            services.AddLogging(logging =>
            {
                logging.AddKissLog();
            });

        }

    }
}
