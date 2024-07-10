using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using techwave.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<techwaveContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("techwaveContext") ?? throw new InvalidOperationException("Connection string 'techwaveContext' not found.")));


// Configura localização e moeda
var defaultDateCulture = "pt-BR";
var ci = new CultureInfo(defaultDateCulture);
ci.NumberFormat.CurrencySymbol = "R$";
ci.NumberFormat.CurrencyDecimalSeparator = ",";
ci.NumberFormat.CurrencyGroupSeparator = ".";
ci.NumberFormat.NumberDecimalSeparator = ",";
ci.NumberFormat.NumberGroupSeparator = ".";

CultureInfo.DefaultThreadCurrentCulture = ci;
CultureInfo.DefaultThreadCurrentUICulture = ci;


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//Atualizado para aceitar valores com ,
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(ci),
    SupportedCultures = new List<CultureInfo> { ci },
    SupportedUICultures = new List<CultureInfo> { ci }
});



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
