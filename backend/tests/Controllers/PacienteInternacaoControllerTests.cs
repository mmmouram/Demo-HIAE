using NUnit.Framework;
using Moq;
using backend.src.Services;
using backend.src.Controllers;
using backend.src.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace backend.tests.Controllers
{
    [TestFixture]
    public class PacienteInternacaoControllerTests
    {
        private Mock<IPacienteInternacaoService> _serviceMock;
        private PacienteInternacaoController _controller;

        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IPacienteInternacaoService>();
            _controller = new PacienteInternacaoController(_serviceMock.Object);
        }

        [Test]
        public async Task ObterPacientesInternados_DeveRetornarListaDePacientes()
        {
            // Arrange
            int idMedico = 1;
            var pacientes = new List<Paciente>
            {
                new Paciente { Id = 1, Nome = "Paciente 1", Internado = true, IdMedicoResponsavel = 1 },
                new Paciente { Id = 2, Nome = "Paciente 2", Internado = true, IdMedicoResponsavel = 1 }
            };

            _serviceMock.Setup(s => s.ObterPacientesPorMedicoAsync(idMedico))
                .ReturnsAsync(pacientes);

            // Act
            var result = await _controller.ObterPacientesInternados(idMedico) as OkObjectResult;
            var resultPacientes = result.Value as IEnumerable<Paciente>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(2, resultPacientes.Count());
        }

        [Test]
        public async Task ObterHistoricoPaciente_ComHistorico_DeveRetornarOkComDados()
        {
            // Arrange
            int idMedico = 1;
            int idPaciente = 10;
            var historicos = new List<Historico>
            {
                new Historico { Id = 1, Descricao = "Evolução 1", Tipo = "evolução", DataRegistro = DateTime.Now, PacienteId = idPaciente }
            };
            var resultado = new HistoricoResultado { PossuiPermissao = true, Historicos = historicos };

            _serviceMock.Setup(s => s.ObterHistoricoPorPacienteAsync(idPaciente, idMedico))
                .ReturnsAsync(resultado);

            // Act
            var actionResult = await _controller.ObterHistoricoPaciente(idPaciente, idMedico) as OkObjectResult;
            var resultHistoricos = actionResult.Value as IEnumerable<Historico>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(200, actionResult.StatusCode);
            Assert.AreEqual(1, resultHistoricos.Count());
        }

        [Test]
        public async Task ObterHistoricoPaciente_SemHistorico_DeveRetornarNotFound()
        {
            // Arrange
            int idMedico = 1;
            int idPaciente = 10;
            var resultado = new HistoricoResultado { PossuiPermissao = true, Historicos = new List<Historico>() };

            _serviceMock.Setup(s => s.ObterHistoricoPorPacienteAsync(idPaciente, idMedico))
                .ReturnsAsync(resultado);

            // Act
            var actionResult = await _controller.ObterHistoricoPaciente(idPaciente, idMedico) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(404, actionResult.StatusCode);
            Assert.AreEqual("Não há registros disponíveis para este paciente.", actionResult.Value);
        }

        [Test]
        public async Task ObterHistoricoPaciente_SemPermissao_DeveRetornarForbid()
        {
            // Arrange
            int idMedico = 1;
            int idPaciente = 10;
            var resultado = new HistoricoResultado { PossuiPermissao = false, Historicos = new List<Historico>() };

            _serviceMock.Setup(s => s.ObterHistoricoPorPacienteAsync(idPaciente, idMedico))
                .ReturnsAsync(resultado);

            // Act
            var actionResult = await _controller.ObterHistoricoPaciente(idPaciente, idMedico);

            // Assert
            // ForbidResult does not allow setting a status code, so we simply checar se é do tipo ForbidResult
            Assert.IsInstanceOf<ForbidResult>(actionResult);
        }

        [Test]
        public void ObterHistoricoPaciente_QuandoExceptionOcorre_DevePropagarException()
        {
            // Arrange
            int idMedico = 1;
            int idPaciente = 10;

            _serviceMock.Setup(s => s.ObterHistoricoPorPacienteAsync(idPaciente, idMedico))
                .ThrowsAsync(new Exception("Falha de conexão"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _controller.ObterHistoricoPaciente(idPaciente, idMedico));
        }
    }
}
