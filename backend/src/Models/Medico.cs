using System.ComponentModel.DataAnnotations;

namespace backend.src.Models
{
    /// <summary>
    /// Modelo que representa um médico no sistema.
    /// </summary>
    public class Medico
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Nome { get; set; }
        
        // Outros campos relevantes, como especialidade ou credenciais, podem ser adicionados
    }
}
