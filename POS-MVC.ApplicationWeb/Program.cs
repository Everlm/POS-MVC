using POS_MVC.IOC.Dependency;
using System.Reflection;
using POS_MVC.ApplicationWeb.Utilities.Extensions;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
});
builder.Services.DependencyInjection(builder.Configuration);
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
var contextPDF = new CustomAssemblyLoadContext();
contextPDF.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "Utilities/PDFLibrary/libwkhtmltox.dll"));
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
