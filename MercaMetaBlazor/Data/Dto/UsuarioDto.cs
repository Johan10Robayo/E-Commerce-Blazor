using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MercaMetaBlazor.Data.Dto
{
    public class UsuarioDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Paswd { get; set; }
        [Required]
        public string Rol { get; set; }
    }
}
