using MeterReadings.Data.Repositories;
using MeterReadings.Services;
using System.IO.Abstractions;

namespace MeterReadings.API
{
    public static class DependencyInjection
    {
        public static void Configure(IServiceCollection services)
        {
            //System.IO.Abstractions allows for mocking of the fileSystem
            services.AddSingleton<IFileSystem, FileSystem>();

            //Repositories
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IMeterReadingRepository, MeterReadingRepository>();

            //Services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMeterReadingService, MeterReadingService>();
        }
    }
}
