using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MercaMetaBlazor.Data.Dto
{
    public class EmpresaDto
    {
        [Required]
        public int Nit { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Direccion { get; set; }
        
        public string UrlImagen { get; set; }
       
        public IFormFile Imagen { get; set; }
        [Required]
        public long Telefono { get; set; }
        [Required]
        public PersonaDto Representante { get; set; } = new PersonaDto();
        [Required]
        public UsuarioDto Usuario { get; set; } = new UsuarioDto(); 
    }
}
