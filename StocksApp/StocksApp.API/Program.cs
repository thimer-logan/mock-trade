using Serilog;
using StocksApp.Application.Interfaces.Finnhub;
using StocksApp.Application.Services.Finnhub;
using StocksApp.Domain.Interfaces;
using StocksApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{

    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration) //read configuration settings from built-in IConfiguration
    .ReadFrom.Services(services); //read out current app's services and make them available to serilog
});

builder.Services.AddControllers();

builder.Services.AddTransient<IFinnhubCompanyProfileService, FinnhubCompanyProfileService>();
builder.Services.AddTransient<IFinnhubStockPriceQuoteService, FinnhubStockPriceQuoteService>();
builder.Services.AddTransient<IFinnhubStockSearchService, FinnhubStockSearchService>();
builder.Services.AddTransient<IFinnhubStocksService, FinnhubStocksService>();
builder.Services.AddTransient<IFinnhubRepository, FinnhubRepository>();
builder.Services.AddHttpClient<IFinnhubRepository, FinnhubRepository>(client =>
{
    var baseUrl = builder.Configuration["Finnhub:BaseUrl"]; client.BaseAddress = new Uri(baseUrl ?? "https://finnhub.io");
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
