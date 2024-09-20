using Microsoft.EntityFrameworkCore;
using dotenv.net;

namespace prisma.Context
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                DotEnv.Load();
                optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_URL"));
            }
        }
    }
}
