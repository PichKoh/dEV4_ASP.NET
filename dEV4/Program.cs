using dEV4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("books.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile("profiles.json", optional: true, reloadOnChange: true);

builder.Services.Configure<Books>(builder.Configuration.GetSection("Books"));
builder.Services.Configure<Profiles>(builder.Configuration.GetSection("Profiles"));

var app = builder.Build();

app.UseMiddleware<BooksMiddleware>();

app.Map("/Library", () => "Welcome to library!");
app.Map("/Library/Profile/{id:int?}", (int? id, IOptions<Profiles> options) =>
{
    var profiles = options.Value;
    StringBuilder result = new StringBuilder();
    foreach (Profile profile in profiles.profiles)
    {
        var Id = profile.Id;
        var Name = profile.Name;
        var City = profile.City;
        var Year = profile.Year;
        if (id == Id || id == null)
        {
            result.Append($"Id: {Id}; Name: {Name}; City: {City}; Year: {Year}\n");
        }
    }
    var resultString = result.ToString();
    if (resultString.Length == 0)
    {
        return "No records found!";
    }
    return resultString;
});

app.MapGet("/Library/Profile", () =>
{
    // Виведення інформації про самого користувача (без вказівки id)
    var profileInfo = new Profile
    {
        Id = 0,        // Припустимо, що id самого користувача - 0
        Name = "Amina",
        City = "Mykolaiv",
        Year = "2002"
    };
    return $"Id: {profileInfo.Id}; Name: {profileInfo.Name}; City: {profileInfo.City}; Year: {profileInfo.Year}\n";
});

app.Use(async (context, next) =>
{
    await next.Invoke();
    if (context.Response.StatusCode == 404)
        await context.Response.WriteAsync("Page not found!");
});

app.Run();
