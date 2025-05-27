using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.src.Models
{
    /// <summary>
    /// Modelo que representa um paciente em internação.
    /// </summary>
    public class Paciente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        // Indica se o paciente está internado
        public bool Internado { get; set; }

        // Identificador do médico responsável pelo paciente
        public int IdMedicoResponsavel { get; set; }

        // Lista de históricos de internação
        public ICollection<Historico> Historicos { get; set; }
    }
}
