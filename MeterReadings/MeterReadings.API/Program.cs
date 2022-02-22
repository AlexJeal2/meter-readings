using MeterReadings.API;
using MeterReadings.Data;
using MeterReadings.Services;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings;

public class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        builder.Services.AddControllers();

        ConfigureSqlServer(builder);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Configure DI
        DependencyInjection.Configure(builder.Services);
        //Configure AutoMapper
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        //Ensure migrations have been run before proceeding

        //Seed the database here
        var serviceScopeFactory = app.Services.GetService<IServiceScopeFactory>()!;

        using (var scope = serviceScopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
        }

        using (var scope = serviceScopeFactory.CreateScope())
        {
            var accountService = scope.ServiceProvider.GetService<IAccountService>()!;
            SeedAccountData(accountService);
        }

        app.Run();
    }

    static void SeedAccountData(IAccountService accountService)
    {
        accountService.SeedDatabaseAsync().Wait();
    }

    static void ConfigureSqlServer(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
    }
}
