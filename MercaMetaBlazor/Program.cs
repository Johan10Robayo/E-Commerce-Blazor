using MercaMetaBlazor.Data;
using MercaMetaBlazor.Data.Dao;
using MercaMetaBlazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<ConexionDb>(
    new ConexionDb(builder.Configuration.GetConnectionString("MercaMetaConnection"))
    );

builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", option => {
    option.Cookie.Name = "MyCookieAuth";
    option.LoginPath = "/Login"; //la ruta donde está el login 
    option.AccessDeniedPath = "/Site/AccesoDenegado";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

builder.Services.AddSingleton<EmpresaDao>();
builder.Services.AddSingleton<EmpresaService>();
builder.Services.AddSingleton<ProductoDao>();
builder.Services.AddSingleton<ProductoService>();
builder.Services.AddSingleton<PersonaDao>();
builder.Services.AddSingleton<UsuarioDao>();
builder.Services.AddSingleton<PersonaService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
