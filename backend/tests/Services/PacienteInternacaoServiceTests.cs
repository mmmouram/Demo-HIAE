using NUnit.Framework;
using Moq;
using backend.src.Services;
using backend.src.Repositories;
using backend.src.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace backend.tests.Services
{
    [TestFixture]
    public class PacienteInternacaoServiceTests
    {
        private Mock<IPacienteRepository> _repositoryMock;
        private PacienteInternacaoService _service;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IPacienteRepository>();
            _service = new PacienteInternacaoService(_repositoryMock.Object);
        }

        [Test]
        public async Task ObterPacientesPorMedicoAsync_DeveRetornarPacientesInternadosDoMedico()
        {
            // Arrange
            int idMedico = 1;
            var pacientes = new List<Paciente>
            {
                new Paciente { Id = 1, Nome = "Paciente 1", Internado = true, IdMedicoResponsavel = 1 },
                new Paciente { Id = 2, Nome = "Paciente 2", Internado = true, IdMedicoResponsavel = 1 }
            };

            _repositoryMock.Setup(r => r.ObterPacientesPorMedicoAsync(idMedico))
                .ReturnsAsync(pacientes);

            // Act
            var result = await _service.ObterPacientesPorMedicoAsync(idMedico);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task ObterHistoricoPorPacienteAsync_ComPermissao_DeveRetornarHistoricos()
        {
            // Arrange
            int idMedico = 1;
            int idPaciente = 10;
            var historicos = new List<Historico>
            {
                new Historico { Id = 1, Descricao = "Evolução", Tipo = "evolução", DataRegistro = DateTime.Now, PacienteId = idPaciente }
            };
            var paciente = new Paciente { Id = idPaciente, Nome = "Paciente", Internado = true, IdMedicoResponsavel = idMedico, Historicos = historicos };

            _repositoryMock.Setup(r => r.ObterPacienteComHistoricoAsync(idPaciente))
                .ReturnsAsync(paciente);

            // Act
            var resultado = await _service.ObterHistoricoPorPacienteAsync(idPaciente, idMedico);

            // Assert
            Assert.IsTrue(resultado.PossuiPermissao);
            Assert.AreEqual(1, resultado.Historicos.Count());
        }

        [Test]
        public async Task ObterHistoricoPorPacienteAsync_SemPermissao_DeveRetornarHistoricosVazios()
        {
            // Arrange
            int idMedico = 1;
            int idPaciente = 10;
            // Paciente com historico mas de outro médico
            var historicos = new List<Historico>
            {
                new Historico { Id = 1, Descricao = "Evolução", Tipo = "evolução", DataRegistro = DateTime.Now, PacienteId = idPaciente }
            };
            var paciente = new Paciente { Id = idPaciente, Nome = "Paciente", Internado = true, IdMedicoResponsavel = 2, Historicos = historicos };

            _repositoryMock.Setup(r => r.ObterPacienteComHistoricoAsync(idPaciente))
                .ReturnsAsync(paciente);

            // Act
            var resultado = await _service.ObterHistoricoPorPacienteAsync(idPaciente, idMedico);

            // Assert
            Assert.IsFalse(resultado.PossuiPermissao);
            Assert.AreEqual(0, resultado.Historicos.Count());
        }

        [Test]
        public async Task ObterHistoricoPorPacienteAsync_PacienteInexistente_DeveRetornarSemPermissao()
        {
            // Arrange
            int idMedico = 1;
            int idPaciente = 10;

            _repositoryMock.Setup(r => r.ObterPacienteComHistoricoAsync(idPaciente))
                .ReturnsAsync((Paciente)null);

            // Act
            var resultado = await _service.ObterHistoricoPorPacienteAsync(idPaciente, idMedico);

            // Assert
            Assert.IsFalse(resultado.PossuiPermissao);
            Assert.IsEmpty(resultado.Historicos);
        }
    }
}
