using MySql.Data.MySqlClient;

namespace MercaMetaBlazor.Data
{
    public class ConexionDb
    {
        private readonly MySqlConnection  conexion;

        public ConexionDb(string cadenaConexion)
        {
            try
            {
                conexion = new MySqlConnection(cadenaConexion);
                conexion.Open();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public MySqlConnection MysqlConnection { get => conexion; }

    }
}
