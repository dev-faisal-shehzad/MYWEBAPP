using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllersWithViews();

// Add services to the container.
// builder.Services.AddRazorPages();

var app = builder.Build();

// ✅ Move the seeding logic here, after `app` is built
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    Seed.Initialize(services); // ✅ Call the seed function
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
