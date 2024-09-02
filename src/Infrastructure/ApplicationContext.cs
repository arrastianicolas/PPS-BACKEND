using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public DbSet<> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
         : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración del modelo, si es necesario
        }
    }
}
