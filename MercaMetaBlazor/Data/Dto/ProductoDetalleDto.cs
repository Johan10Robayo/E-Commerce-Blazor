using System.ComponentModel.DataAnnotations;

namespace MercaMetaBlazor.Data.Dto
{
    public class ProductoDetalleDto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public double ValorUnitario { get; set; }
        public string Descripcion { get; set; }
        public string UnidadMedida { get; set; }
        public int CantidadSolicitada { get; set; }
        public double Total{ get; set; }
    }
}
