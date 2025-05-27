using backend.src.Models;
using backend.src.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteInternacaoController : ControllerBase
    {
        private readonly IPacienteInternacaoService _pacienteInternacaoService;

        public PacienteInternacaoController(IPacienteInternacaoService pacienteInternacaoService)
        {
            _pacienteInternacaoService = pacienteInternacaoService;
        }

        /// <summary>
        /// Endpoint para obter a lista de pacientes internados vinculados ao médico logado.
        /// Para efeito de exemplo, o id do médico é passado como parâmetro de query (em cenários reais estará no token de autenticação).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObterPacientesInternados([FromQuery] int idMedico)
        {
            var pacientes = await _pacienteInternacaoService.ObterPacientesPorMedicoAsync(idMedico);
            return Ok(pacientes);
        }

        /// <summary>
        /// Endpoint para obter o histórico de internação de um paciente selecionado.
        /// O parâmetro idMedico representa o médico logado (exemplo simplificado).
        /// </summary>
        [HttpGet("{idPaciente}/historico")]
        public async Task<IActionResult> ObterHistoricoPaciente(int idPaciente, [FromQuery] int idMedico)
        {
            var resultado = await _pacienteInternacaoService.ObterHistoricoPorPacienteAsync(idPaciente, idMedico);
            if (!resultado.PossuiPermissao)
            {
                return Forbid("Acesso negado. O médico não tem permissão para acessar os dados deste paciente.");
            }
            
            if (resultado.Historicos == null || !resultado.Historicos.Any())
            {
                return NotFound("Não há registros disponíveis para este paciente.");
            }
            
            return Ok(resultado.Historicos);
        }
    }
}
