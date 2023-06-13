using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MercaMetaBlazor.Data.Dto
{
    public class PersonaDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public long Telefono { get; set; }
        [Required]
        public string Direccion { get; set; }
    }
}
