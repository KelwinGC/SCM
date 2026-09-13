using Microsoft.EntityFrameworkCore;
using SCM.ApiControl.Dominio.Entidad;
using SCM.ApiControl.Infraestructura.Repositorio.Interceptors;
using System.Reflection;

namespace SCM.ApiControl.Infraestructura.Repositorio.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

        public DbSet<CargaArchivo> CargasArchivo { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CargaArchivo>().ToTable("CargaArchivo").HasKey(ca => ca.IdCarga);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
