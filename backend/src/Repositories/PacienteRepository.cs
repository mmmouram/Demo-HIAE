using backend.src.Config;
using backend.src.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.src.Repositories
{
    /// <summary>
    /// Implementação do repositório de pacientes utilizando Entity Framework.
    /// </summary>
    public class PacienteRepository : IPacienteRepository
    {
        private readonly InternacaoContext _context;

        public PacienteRepository(InternacaoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém os pacientes internados vinculados ao médico.
        /// </summary>
        public async Task<IEnumerable<Paciente>> ObterPacientesPorMedicoAsync(int idMedico)
        {
            return await _context.Pacientes
                .Where(p => p.IdMedicoResponsavel == idMedico && p.Internado)
                .ToListAsync();
        }

        /// <summary>
        /// Obtém um paciente e seus registros históricos.
        /// </summary>
        public async Task<Paciente> ObterPacienteComHistoricoAsync(int idPaciente)
        {
            return await _context.Pacientes
                .Include(p => p.Historicos)
                .FirstOrDefaultAsync(p => p.Id == idPaciente);
        }
    }
}
