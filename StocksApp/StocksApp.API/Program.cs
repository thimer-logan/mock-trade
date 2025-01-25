using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StocksApp.Application.Interfaces.Authentication;
using StocksApp.Application.Interfaces.Finnhub;
using StocksApp.Application.Services.Authentication;
using StocksApp.Application.Services.Finnhub;
using StocksApp.Domain.Entities.Identity;
using StocksApp.Domain.Interfaces;
using StocksApp.Infrastructure.DbContext;
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

builder.Services.AddControllers(options =>
{
    //options.Filters.Add(new ProducesAttribute("application/json"));
    //options.Filters.Add(new ConsumesAttribute("application/json"));

    // Authorization policy
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddTransient<IFinnhubCompanyProfileService, FinnhubCompanyProfileService>();
builder.Services.AddTransient<IFinnhubStockPriceQuoteService, FinnhubStockPriceQuoteService>();
builder.Services.AddTransient<IFinnhubStockSearchService, FinnhubStockSearchService>();
builder.Services.AddTransient<IFinnhubStocksService, FinnhubStocksService>();
builder.Services.AddTransient<IFinnhubRepository, FinnhubRepository>();
builder.Services.AddHttpClient<IFinnhubRepository, FinnhubRepository>(client =>
{
    var baseUrl = builder.Configuration["Finnhub:BaseUrl"]; client.BaseAddress = new Uri(baseUrl ?? "https://finnhub.io");
});
builder.Services.AddTransient<IJwtService, JwtService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>()
  .AddDefaultTokenProviders()
  .AddUserStore<UserStore<ApplicationUser, ApplicationRole, AppDbContext, Guid>>()
  .AddRoleStore<RoleStore<ApplicationRole, AppDbContext, Guid>>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters()
     {
         ValidateAudience = false,
         //ValidAudience = builder.Configuration["Jwt:Audience"],
         ValidateIssuer = true,
         ValidIssuer = builder.Configuration["Jwt:Issuer"],
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
     };
 });

builder.Services.AddAuthorization(options =>
{
});

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
