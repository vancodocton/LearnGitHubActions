using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider")
    ?? throw new ArgumentNullException("databaseProvide", "Database Provider is not provided.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    string connectionString;
    switch (databaseProvider)
    {
        case "Sqlite":
            connectionString = builder.Configuration.GetConnectionString("SqliteConnection");
            options.UseSqlite(connectionString, options => options.MigrationsAssembly("Infrastructure.Sqlite"));
            break;
        case "SqlServer":
            connectionString = builder.Configuration.GetConnectionString("LocalSqlServer");
            options.UseSqlServer(connectionString, options => options.MigrationsAssembly("Infrastructure.SqlServer"));
            break;
        case "PostgreSql":
            connectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");
            options.UseNpgsql(connectionString, options => options.MigrationsAssembly("Infrastructure.PostgreSql"));
            break;
        default:
            throw new ArgumentException("Database Provider is not supported.", nameof(databaseProvider));
    }
});
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
