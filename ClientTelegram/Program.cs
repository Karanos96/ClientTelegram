using ClientTelegram.Context;
using ClientTelegram.IService;
using ClientTelegram.Repository;
using ClientTelegram.Security;
using ClientTelegram.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        //----------Singleton for cryptography---------------------------------------
        builder.Services.AddSingleton<NonceCounterStore>(sp =>
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' mancante.");
            return new NonceCounterStore(connectionString);
        });

        builder.Services.AddSingleton<CounterNonceGenerator>(sp =>
        {
            var store = sp.GetRequiredService<NonceCounterStore>();
            return new CounterNonceGenerator(store.ReserveBlock);
        });

        //--------------------------------------------------------------------------

        builder.Services.AddSingleton<ITelegramOrchestrator , TelegramOrchestrator>();
        builder.Services.AddScoped<ISessionRepository, SessionRepository>();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        //test connection
        //app.Services.GetRequiredService<ITDLibService>();

        var orchestrator = app.Services.GetRequiredService<ITelegramOrchestrator>();
        await orchestrator.InitializeAsync();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}