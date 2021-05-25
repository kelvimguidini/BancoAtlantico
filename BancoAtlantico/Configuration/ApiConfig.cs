using Atlantico.CrossCutting.Utils;
using Atlantico.Data.Context;
using Atlantico.Domain;
using Atlantico.WebApi.Configuration.HubConfig;
using AutoMapper;
using KissLog.AspNetCore;
using KissLog.CloudListeners.RequestLogsListener;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Atlantico.WebApi.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSignalR();

            services.AddControllers();

            services.AddMemoryCache();

            services.AddDbContext<ContextDB>(options =>
                options.UseInMemoryDatabase(databaseName: "Test"));


            services.AddHealthChecks()
                .AddCheck("Caixa Eletrônico", () =>
                    HealthCheckResult.Healthy("Está OK!"));



            #region popular banco inMemory FAKE
            DataCreate.createDataInMemory();
            #endregion

            services.AddCors(options =>
            {
                options.AddPolicy("Cors", builder => builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });

            services.Configure<ApiBehaviorOptions>(option =>
            {
                option.SuppressModelStateInvalidFilter = true;
            });

            var key = Encoding.ASCII.GetBytes(configuration.GetSection("Jwt:Secret").Value);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Application.Mapper.AutoMapperSetup());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

        }

        public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("Cors");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseKissLogMiddleware(options =>
            {
                ConfigureKissLog(options);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotesAlertHub>("/hub");
                endpoints.MapHealthChecks("healthcheck", new HealthCheckOptions
                {
                    ResponseWriter = async (context, report) =>
                    {
                        var result = JsonSerializer.Serialize(
                            new
                            {
                                status = report.Status.ToString(),
                                monitors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
                            });
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });
            });
        }


        private static void ConfigureKissLog(IOptionsBuilder options)
        {
            // optional KissLog configuration
            options.Options
                .AppendExceptionDetails((Exception ex) =>
                {
                    StringBuilder sb = new StringBuilder();

                    if (ex is System.NullReferenceException nullRefException)
                    {
                        sb.AppendLine("Important: check for null references");
                    }

                    return sb.ToString();
                });

            // KissLog internal logs
            options.InternalLog = (message) =>
            {
                Debug.WriteLine(message);
            };

            // register logs output
            RegisterKissLogListeners(options);
        }

        private static void RegisterKissLogListeners(IOptionsBuilder options)
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();

            options.Listeners.Add(new RequestLogsApiListener(new KissLog.CloudListeners.Auth.Application(
                root.GetSection("KissLog.OrganizationId").Value,
               root.GetSection("KissLog.ApplicationId").Value)
            )
            {
                ApiUrl = root.GetSection("KissLog.ApiUrl").Value
            });
        }
    }
}