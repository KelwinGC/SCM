using Microsoft.EntityFrameworkCore;
using SCM.ApiLoadProcess.Dominio.Entidad;
using SCM.ApiLoadProcess.Infraestructura.Repositorio.Interceptors;
using System.Reflection;

namespace SCM.ApiLoadProcess.Infraestructura.Repositorio.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

        public DbSet<CargaArchivo> CargasArchivo { get; set; }
        public DbSet<DataProcesada> DataProcesadas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CargaArchivo>().ToTable("CargaArchivo").HasKey(ca => ca.IdCarga);
            builder.Entity<DataProcesada>().ToTable("DataProcesada").HasKey(ca => ca.IdData);
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
