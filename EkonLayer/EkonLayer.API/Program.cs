using EkonLayer.Core.Repositories;
using EkonLayer.Core.Services;
using EkonLayer.Core.UnitOfWork;
using EkonLayer.Helpers.Models.Dtos.DbModelDtos;
using EkonLayer.Logging.Methods;
using EkonLayer.Logging.Middleware;
using EkonLayer.Repository.Context;
using EkonLayer.Repository.Repositories;
using EkonLayer.Repository.UnitOfWork;
using EkonLayer.Service.Mapping;
using EkonLayer.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Check ConnectionString
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new ArgumentException("The string argument 'ConnectionString' cannot be empty.");
}

// Add services to the container
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name);
    });
});

builder.Services.AddControllers();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MapProfile));

// Add Scoped Services
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));

// Add HttpClient
builder.Services.AddHttpClient();

// Configure ApplicationDto
builder.Services.Configure<ApplicationDto>(options =>
{
    options.Name = Assembly.GetEntryAssembly().GetName().Name;

    var dns = Dns.GetHostAddresses(Dns.GetHostName());
    if (dns.Length > 0)
    {
        options.Ip = dns[dns.Length - 1].ToString();
        if (dns.Length > 1 && options.Ip.Length > 20)
        {
            options.Ip = dns[0].ToString();
        }
    }
});

// Add Log Services
builder.Services.AddScoped<IUserLogService, UserLogService>();
builder.Services.AddScoped<IApplicationLogService, ApplicationLogService>();
builder.Services.AddScoped<IErrorLogService, ErrorLogService>();

// Add Singleton Services
builder.Services.AddSingleton<LogWorker>();
builder.Services.AddHostedService(provider => provider.GetService<LogWorker>());

builder.Services.AddSingleton<TimerWorker>();
builder.Services.AddHostedService(provider => provider.GetService<TimerWorker>());

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EkonLayer.API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EkonLayer.API v1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRouting();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
