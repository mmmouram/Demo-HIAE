using NUnit.Framework;
using backend.src.Repositories;
using backend.src.Config;
using backend.src.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace backend.tests.Repositories
{
    [TestFixture]
    public class PacienteRepositoryTests
    {
        private InternacaoContext _context;
        private PacienteRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<InternacaoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new InternacaoContext(options);
            _repository = new PacienteRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task ObterPacientesPorMedicoAsync_DeveRetornarSomentePacientesInternadosDoMedico()
        {
            // Arrange
            int idMedico = 1;
            _context.Pacientes.AddRange(new List<Paciente>
            {
                new Paciente { Id = 1, Nome = "Paciente 1", Internado = true, IdMedicoResponsavel = idMedico },
                new Paciente { Id = 2, Nome = "Paciente 2", Internado = false, IdMedicoResponsavel = idMedico },
                new Paciente { Id = 3, Nome = "Paciente 3", Internado = true, IdMedicoResponsavel = 2 }
            });
            await _context.SaveChangesAsync();

            // Act
            var pacientes = await _repository.ObterPacientesPorMedicoAsync(idMedico);

            // Assert
            Assert.AreEqual(1, pacientes.Count());
            Assert.AreEqual("Paciente 1", pacientes.First().Nome);
        }

        [Test]
        public async Task ObterPacienteComHistoricoAsync_DeveRetornarPacienteComHistoricos()
        {
            // Arrange
            int idPaciente = 1;
            var paciente = new Paciente {
                Id = idPaciente,
                Nome = "Paciente Teste",
                Internado = true,
                IdMedicoResponsavel = 1,
                Historicos = new List<Historico>
                {
                    new Historico { Id = 1, Descricao = "Registro 1", Tipo = "evolução", DataRegistro = DateTime.Now }
                }
            };

            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            // Act
            var pacienteObtido = await _repository.ObterPacienteComHistoricoAsync(idPaciente);

            // Assert
            Assert.IsNotNull(pacienteObtido);
            Assert.IsNotNull(pacienteObtido.Historicos);
            Assert.AreEqual(1, pacienteObtido.Historicos.Count);
        }
    }
}
