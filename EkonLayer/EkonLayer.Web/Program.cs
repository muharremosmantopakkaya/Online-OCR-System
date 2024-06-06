using EkonLayer.Core.Repositories;
using EkonLayer.Core.Services;
using EkonLayer.Core.UnitOfWork;
using EkonLayer.Helpers.Models.Dtos.DbModelDtos;
using EkonLayer.Logging.Methods;
using EkonLayer.Repository.Context;
using EkonLayer.Repository.Repositories;
using EkonLayer.Repository.UnitOfWork;
using EkonLayer.Service.Mapping;
using EkonLayer.Service.Services;
using EkonLayer.Web.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ConnectionString değerini kontrol et
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new ArgumentException("The string argument 'ConnectionString' cannot be empty.");
}

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name);
    });
});

builder.Services.AddControllersWithViews(); // MVC hizmetlerini eklemek için
builder.Services.AddRazorPages(); // Razor Pages kullanıyorsanız bunu ekleyin

builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
builder.Services.AddHttpClient();

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

builder.Services.AddScoped<IUserLogService, UserLogService>();
builder.Services.AddScoped<IApplicationLogService, ApplicationLogService>();
builder.Services.AddScoped<IErrorLogService, ErrorLogService>();

builder.Services.AddSingleton<LogWorker>();
builder.Services.AddHostedService(provider => provider.GetService<LogWorker>());

builder.Services.AddSingleton<TimerWorker>();
builder.Services.AddHostedService(provider => provider.GetService<TimerWorker>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Razor Pages kullanıyorsanız bunu ekleyin

app.UseWebSockets();
app.UseMiddleware<WebSocketHandler>();

app.Run();
