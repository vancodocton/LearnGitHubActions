using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WebMvc.Configurations
{
    public static class ServicesExtensions
    {
        public enum DatabaseProviders
        {
            None,
            Sqlite,
            SqlServer,
            PostgreSql,
        }

        public static DatabaseProviders DatabaseProvider { get; } = DatabaseProviders.None;
        // public static IConfigurationSection ConnectionStrings { get; }

        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, string databaseProvider, IConfigurationSection connectionStrings)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                switch (databaseProvider)
                {
                    case "Sqlite":
                        options.UseSqlite(connectionStrings["SqliteConnection"], options => options.MigrationsAssembly("Infrastructure.Sqlite"));
                        break;
                    case "SqlServer":
                        options.UseSqlServer(connectionStrings["LocalSqlServer"], options => options.MigrationsAssembly("Infrastructure.SqlServer"));
                        break;
                    case "PostgreSql":
                        options.UseNpgsql(connectionStrings["PostgreSqlConnection"], options => options.MigrationsAssembly("Infrastructure.PostgreSql"));
                        break;
                    default:
                        throw new ArgumentException("Database Provider is not supported.", nameof(databaseProvider));
                }
            });
            
            return services;
        }
    }
}