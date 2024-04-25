using ExpressVoitures.Data;
using ExpressVoitures.Models.Repositories;
using ExpressVoitures.Models.Services;
using ExpressVoitures.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Register Car as a scoped service
builder.Services.AddScoped<Car>();
builder.Services.AddScoped<CarsController>();

builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<ICarRepository, CarRepository>();


builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services.AddMvc();

var app = builder.Build();

// Resolve UserManager from the service provider
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        // Assuming DataSeeder is your class and SeedData is a static method within it
        // Adjust this line according to your actual DataSeeder implementation
        DataSeeder.SeedData(userManager).Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.UseStaticFiles();

app.MapGet("Identity/Account/Register", context =>
{
    context.Response.Redirect("/");
    return Task.CompletedTask;
});

app.MapGet("Identity/Account/Manage", context =>
{
    context.Response.Redirect("/");
    return Task.CompletedTask;
});

app.MapGet("Identity/Account/ForgotPassword", context =>
{
    context.Response.Redirect("/");
    return Task.CompletedTask;
});

app.MapGet("Identity/Account/ResetPassword", context =>
{
    context.Response.Redirect("/");
    return Task.CompletedTask;
});

app.MapGet("Identity/Account/ConfirmEmail", context =>
{
    context.Response.Redirect("/");
    return Task.CompletedTask;
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
