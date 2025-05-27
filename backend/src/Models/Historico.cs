using System;
using System.ComponentModel.DataAnnotations;

namespace backend.src.Models
{
    /// <summary>
    /// Modelo que representa um registro no histórico de internação.
    /// </summary>
    public class Historico
    {
        [Key]
        public int Id { get; set; }

        // Data do registro
        public DateTime DataRegistro { get; set; }

        // Descrição ou anotação do registro
        public string Descricao { get; set; }

        // Tipo de registro (ex: evolução médica, resultado de exame, tratamento)
        public string Tipo { get; set; }

        // Chave estrangeira para o paciente
        public int PacienteId { get; set; }

        public Paciente Paciente { get; set; }
    }
}
