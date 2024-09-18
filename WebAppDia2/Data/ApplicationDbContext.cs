using Microsoft.EntityFrameworkCore;
using WebAppDia2.Entities;

namespace WebAppDia2.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configurar el campo Name en la tabla Products
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("tbProducts");
                entity.Property(p => p.Name)
                      .IsRequired()          // Campo requerido
                      .HasMaxLength(128);    // Longitud máxima de 128 caracteres

                // Configurar la precisión y escala del campo Price
                entity.Property(p => p.Price)
                      .HasPrecision(18, 2);  // Hasta 18 dígitos con 2 decimales

            });


            base.OnModelCreating(modelBuilder);
        }


    }
}
