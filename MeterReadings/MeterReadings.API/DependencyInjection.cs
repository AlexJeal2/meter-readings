using MeterReadings.Data.Repositories;
using MeterReadings.Services;

namespace MeterReadings.API
{
    public static class DependencyInjection
    {
        public static void Configure(IServiceCollection services)
        {
            //Repositories
            services.AddScoped<IAccountRepository, AccountRepository>();

            //Services
            services.AddScoped<IAccountService, AccountService>();
        }
    }
}
