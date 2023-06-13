using MercaMetaBlazor.Data.Dao;
using MercaMetaBlazor.Data.Dto;
using Microsoft.AspNetCore.Components.Forms;


namespace MercaMetaBlazor.Services
{
    public class ProductoService
    {
        private readonly ProductoDao _productoDao;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ProductoService(ProductoDao productoDao, IWebHostEnvironment hostingEnvironment)
        {
            _productoDao = productoDao;
            _hostingEnvironment = hostingEnvironment;

        }

        public List<ProductoDto> ObtenerProductosPorEmpresa(int idEmpresa)
        {
            var productos = _productoDao.ObtenerProductosPorEmpresa(idEmpresa);

            return productos;   
        }

        public int ActualizarProducto(ProductoDto producto)
        {
            if (producto.Imagen != null)
            {
                var urlImagen = producto.UrlImagen;

                string rutaServer = _hostingEnvironment.WebRootPath;
                string rutaImagenEliminar = rutaServer + urlImagen.Replace("/img/productos/", "\\img\\productos\\");

                //eliminar imagen anterior
                try
                {
                    if (File.Exists(rutaImagenEliminar))
                    {
                        File.Delete(rutaImagenEliminar);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.Message);
                }


                //crear nueva imagen
                FormFile imagen = producto.Imagen;

                string nombreArchivo = Path.GetFileName(imagen.FileName);

                string rutaDeGuardado = Path.Combine(rutaServer, "img", "productos", nombreArchivo);
                string rutadb = "/img/productos/" + nombreArchivo;
                producto.UrlImagen = rutadb;

                using (var stream = new FileStream(rutaDeGuardado, FileMode.Create))
                {

                    imagen.CopyTo(stream);
                }

            }

            

            int filasSql = _productoDao.ActualizarProducto(producto);

            return filasSql;

        }

        public int EliminarProducto(string codigoProducto)
        {
            var urlImagen = _productoDao.ObtenerUrlImagenProducto(codigoProducto);
            int resultado = _productoDao.EliminarProducto(codigoProducto);

            string rutaServer = _hostingEnvironment.WebRootPath;
            string rutaImagenEliminar = rutaServer + urlImagen.Replace("/img/productos/", "\\img\\productos\\");

            //eliminar imagen anterior
            try
            {
                if (File.Exists(rutaImagenEliminar))
                {
                    File.Delete(rutaImagenEliminar);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
            }

            return resultado;
        }

        public ProductoDto ObtenerProductoPorCodigo(string codigo)
        {

            var producto = _productoDao.ObtenerProductoPorCodigo(codigo);

            return producto;
        }




    }
}
