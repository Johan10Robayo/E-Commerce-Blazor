using System.ComponentModel.DataAnnotations;

namespace MercaMetaBlazor.Data.Dto
{
    public class EmpresaEditarDto
    {
        [Required]
        public int Nit { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Direccion { get; set; }


        public IFormFile Imagen { get; set; }
        [Required]
        public long Telefono { get; set; }
        [Required]
        public int IdRepresentante { get; set; }
        [Required]
        public string NombreRepresentante { get; set; }
        [Required]
        public string ApellidoRepresentante { get; set; }
        [Required]
        public long TelefonoRepresentante { get; set; }
        [Required]
        public string DireccionRepresentante { get; set; }

    }
}
