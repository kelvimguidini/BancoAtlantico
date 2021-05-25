using Atlantico.Application.Interfaces;
using Atlantico.Application.Services;
using Atlantico.CrossCutting.Massages.Interfaces;
using Atlantico.CrossCutting.Messages.Models;
using Atlantico.Data.Context;
using Atlantico.Data.Repositories;
using Atlantico.Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Atlantico.WebApi.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IATMService, ATMService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IATMRepository, ATMRepository>();
            services.AddScoped<IATMBankNoteRepository, ATMBankNoteRepository>();

            services.AddScoped<INotificator, Notificator>();

            services.AddScoped<ContextDB>();
        }
    }
}
