using backend.src.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.src.Config
{
    /// <summary>
    /// Contexto do Entity Framework para o domínio de internações.
    /// </summary>
    public class InternacaoContext : DbContext
    {
        public InternacaoContext(DbContextOptions<InternacaoContext> options) : base(options) {}

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Historico> Historicos { get; set; }
        public DbSet<Medico> Medicos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configurações adicionais do modelo, se necessário
        }
    }
}
