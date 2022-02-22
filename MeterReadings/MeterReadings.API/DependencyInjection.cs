using MeterReadings.Data.Repositories;

namespace MeterReadings.API
{
    public static class DependencyInjection
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
        }
    }
}
