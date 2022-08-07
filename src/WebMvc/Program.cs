using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using WebMvc.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider")
    ?? throw new ArgumentNullException("databaseProvide", "Database Provider is not provided.");
var connectionStringsSection = builder.Configuration.GetRequiredSection("ConnectionStrings");

builder.Services.AddApplicationDbContext(databaseProvider, connectionStringsSection);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
