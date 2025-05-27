using backend.src.Models;
using backend.src.Repositories;

namespace backend.src.Services
{
    /// <summary>
    /// Implementação do serviço de consulta de pacientes em internação.
    /// </summary>
    public class PacienteInternacaoService : IPacienteInternacaoService
    {
        private readonly IPacienteRepository _pacienteRepository;

        public PacienteInternacaoService(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        /// <summary>
        /// Retorna a lista de pacientes internados vinculados ao médico.
        /// </summary>
        public async Task<IEnumerable<Paciente>> ObterPacientesPorMedicoAsync(int idMedico)
        {
            return await _pacienteRepository.ObterPacientesPorMedicoAsync(idMedico);
        }

        /// <summary>
        /// Retorna o histórico de internação e valida a permissão de acesso.
        /// </summary>
        public async Task<HistoricoResultado> ObterHistoricoPorPacienteAsync(int idPaciente, int idMedico)
        {
            var paciente = await _pacienteRepository.ObterPacienteComHistoricoAsync(idPaciente);
            var resultado = new HistoricoResultado();

            if (paciente == null || paciente.IdMedicoResponsavel != idMedico)
            {
                resultado.PossuiPermissao = false;
                resultado.Historicos = Enumerable.Empty<Historico>();
                return resultado;
            }

            resultado.PossuiPermissao = true;
            resultado.Historicos = paciente.Historicos ?? Enumerable.Empty<Historico>();
            return resultado;
        }
    }
}
