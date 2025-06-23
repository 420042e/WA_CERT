using Microsoft.EntityFrameworkCore;
using WA_CERT.Model;

namespace WA_CERT.Data // Asegúrate de usar el namespace de tu proyecto
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Esta propiedad representa tu tabla. La usarás para hacer consultas.
        // El nombre de la propiedad será el que uses en tu código para acceder a la tabla.
        public DbSet<Servidor> Servidor { get; set; }
    }
}