using System.ComponentModel.DataAnnotations;

namespace MercaMetaBlazor.Data.Dto
{
    public class EmpresaTableDto
    {
        public int Nit { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string UrlImagen { get; set; }
        public long Telefono { get; set; }
        public int IdRepresentante { get; set; } 
        public int IdUsuario { get; set; } 
    }
}
