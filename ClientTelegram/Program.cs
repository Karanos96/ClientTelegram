using ClientTelegram.Context;
using ClientTelegram.IService;
using ClientTelegram.Repository;
using ClientTelegram.Service;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

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