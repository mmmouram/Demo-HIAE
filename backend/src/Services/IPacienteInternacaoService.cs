using backend.src.Models;

namespace backend.src.Services
{
    /// <summary>
    /// Interface para o serviço de consulta de pacientes em internação.
    /// </summary>
    public interface IPacienteInternacaoService
    {
        Task<IEnumerable<Paciente>> ObterPacientesPorMedicoAsync(int idMedico);
        Task<HistoricoResultado> ObterHistoricoPorPacienteAsync(int idPaciente, int idMedico);
    }

    public class HistoricoResultado
    {
        public bool PossuiPermissao { get; set; }
        public IEnumerable<Historico> Historicos { get; set; }
    }
}
