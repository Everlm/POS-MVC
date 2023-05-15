using POS_MVC.IOC.Dependency;
using System.Reflection;
using POS_MVC.ApplicationWeb.Utilities.Extensions;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Drawing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
