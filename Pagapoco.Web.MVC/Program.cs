using Microsoft.EntityFrameworkCore;
using Pagapoco.Application.Services;
using Pagapoco.Core.Interfaces;
using Pagapoco.Infrastructure.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

//  Configurar DbContext con tu cadena de conexión
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//  Inyectar tus servicios
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPublicationService, PublicationService>();
builder.Services.AddScoped<IImageService, ImageService>();

//  Agregar soporte para MVC/Razor
builder.Services.AddControllersWithViews();

var app = builder.Build();

//  Middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//  Rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
