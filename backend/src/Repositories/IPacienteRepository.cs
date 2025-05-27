using backend.src.Models;

namespace backend.src.Repositories
{
    /// <summary>
    /// Interface do repositório de pacientes.
    /// </summary>
    public interface IPacienteRepository
    {
        Task<IEnumerable<Paciente>> ObterPacientesPorMedicoAsync(int idMedico);
        Task<Paciente> ObterPacienteComHistoricoAsync(int idPaciente);
    }
}
