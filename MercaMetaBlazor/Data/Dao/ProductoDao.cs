using MercaMetaBlazor.Data.Dto;
using MySql.Data.MySqlClient;

namespace MercaMetaBlazor.Data.Dao
{
    public class ProductoDao
    {

        private readonly ConexionDb _conexionDb;
        public ProductoDao(ConexionDb conexionDb)
        {
            _conexionDb = conexionDb;
        }

        public List<ProductoDto> ObtenerProductosPorEmpresa(int idEmpresa)
        {
            string sql = "select codigo, nombre, precio, descripcion, url_imagen, cantidad, unidad_medida" +
                " from producto where id_empresa = @id_empresa";

            var listaProductos = new List<ProductoDto>();
            try
            {
                using MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection);
                command.Parameters.AddWithValue("@id_empresa", idEmpresa);
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    var productoDto = new ProductoDto
                    {
                        Codigo = reader.GetString("codigo"),
                        Nombre = reader.GetString("nombre"),
                        Precio = reader.GetDouble("precio"),
                        Descripcion = reader.GetString("descripcion"),
                        UrlImagen = reader.GetString("url_imagen"),
                        Cantidad = reader.GetInt32("cantidad"),
                        UnidadMedida = reader.GetString("unidad_medida")

                    };

                    listaProductos.Add(productoDto);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }

            if (listaProductos.Count == 0) return null;

            return listaProductos;
        }

        public int ActualizarProducto(ProductoDto producto)
        {
            int filas = 0;
            try
            {
                bool bandera = false;
                string SqlImagen = "";
                if (producto.UrlImagen != null)
                {
                    SqlImagen = ", url_imagen=@url_imagen";
                    bandera = true;
                }

                string sql = "update producto set nombre=@nombre, precio=@precio, descripcion=@descripcion, " +
                    "cantidad=@cantidad, unidad_medida=@unidad_medida" + SqlImagen +
                    " where codigo=@codigo";
                using (MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection))
                {
                    command.Parameters.AddWithValue("@nombre", producto.Nombre);
                    command.Parameters.AddWithValue("@precio", producto.Precio);
                    command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    command.Parameters.AddWithValue("@cantidad", producto.Cantidad);
                    command.Parameters.AddWithValue("@unidad_medida", producto.UnidadMedida);
                    command.Parameters.AddWithValue("@codigo", producto.Codigo);
                    if (bandera) command.Parameters.AddWithValue("@url_imagen", producto.UrlImagen);


                    filas = command.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return 0;
            }

            return filas;
        }

        public string ObtenerUrlImagenProducto(string codigo)
        {

            string sql = "select url_imagen from producto where codigo=@codigo";
            string urlImagen = "";


            try
            {
                using MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection);
                command.Parameters.AddWithValue("@codigo", codigo);
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    urlImagen = reader.GetString("url_imagen");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return urlImagen;
            }


            return urlImagen;
        }

        public int EliminarProducto(string codigoProducto)
        {
            int filas = 0;
            try
            {
                string sql = "delete from producto where codigo = @codigo";
                using MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection);
                command.Parameters.AddWithValue("@codigo", codigoProducto);
                filas = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }

            return filas;
        }

        public ProductoDto ObtenerProductoPorCodigo(string codigoProducto)
        {
            string sql = "select codigo, nombre, precio, descripcion, url_imagen, cantidad, unidad_medida " +
                "from producto where codigo=@codigo_producto";

            ProductoDto producto = new ProductoDto();

            try
            {
                using MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection);
                command.Parameters.AddWithValue("@codigo_producto", codigoProducto);
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    producto.Codigo = reader.GetString("codigo");
                    producto.Nombre = reader.GetString("nombre");
                    producto.Precio = reader.GetDouble("precio");
                    producto.Descripcion = reader.GetString("descripcion");
                    producto.UrlImagen = reader.GetString("url_imagen");
                    producto.Cantidad = reader.GetInt32("cantidad");
                    producto.UnidadMedida = reader.GetString("unidad_medida");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return null;
            }

            

            return producto;
        }

    }
}
