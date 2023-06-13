using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace MercaMetaBlazor.Data.Dto
{
    public class ProductoDto
    {
        [Required]
        public string Codigo { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public double Precio { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public FormFile Imagen { get; set; }
        [Required]
        public string  UrlImagen { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public string UnidadMedida { get; set; }
    }
}
