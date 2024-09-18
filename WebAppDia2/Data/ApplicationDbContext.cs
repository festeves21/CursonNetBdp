using Microsoft.EntityFrameworkCore;
using WebAppDia2.Entities;
using WebAppDia3.Entities;

namespace WebAppDia2.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }


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

            // Mapea Category a la tabla "tbCategories" y define la longitud del campo "Name"
            modelBuilder.Entity<Category>()
                .ToTable("tbCategories")
                .Property(c => c.Name)
                .HasMaxLength(128)
                .IsRequired(); // Campo obligatorio


            // Mapea Supplier a la tabla "tbSuppliers" y define la longitud del campo "Name"
            modelBuilder.Entity<Supplier>()
                .ToTable("tbSuppliers")
                .Property(s => s.Name)
                .HasMaxLength(128)
                .IsRequired(); // Campo obligatorio


            // Relación Product -> Category (muchos a uno)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);  // Configura como opcional

            // Relación Product -> Supplier (muchos a uno)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);  // Configura como opcional


            base.OnModelCreating(modelBuilder);
        }


    }
}
