using MercaMetaBlazor.Data.Dto;
using MySql.Data.MySqlClient;

namespace MercaMetaBlazor.Data.Dao
{
    public class UsuarioDao
    {
        private readonly ConexionDb _conexionDb;

        public UsuarioDao(ConexionDb conexionDb)
        {
            _conexionDb = conexionDb;
        }

        public int InsertarUsuario(UsuarioDto usuarioDto)
        {
            string sql = "INSERT INTO usuario(email,paswd,rol) VALUES (@email, @paswd, @rol)";
            var MysqlConnection = _conexionDb.MysqlConnection;
            int filas = 0;
            using MySqlTransaction transaction = MysqlConnection.BeginTransaction();

            try
            {
                var command = MysqlConnection.CreateCommand();
                command.CommandText = sql;
                command.Parameters.AddWithValue("@email", usuarioDto.Email);
                command.Parameters.AddWithValue("@paswd", usuarioDto.Paswd);
                command.Parameters.AddWithValue("@rol", usuarioDto.Rol);


                filas = command.ExecuteNonQuery();
                if (filas > 0)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }


            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
            }



            return filas;
        }

        public int ObtenerIdUsuario(string email, string rol)
        {
            string sql = "SELECT id FROM usuario WHERE email=@email and rol=@rol";
            int id = 0;
            try
            {
                using MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@rol", rol);
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    id = reader.GetInt32("id");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }


            return id;
        }



    }
}
