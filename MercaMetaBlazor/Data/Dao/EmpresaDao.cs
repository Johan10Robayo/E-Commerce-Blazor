using MercaMetaBlazor.Data.Dto;
using MySql.Data.MySqlClient;

namespace MercaMetaBlazor.Data.Dao
{
    public class EmpresaDao
    {
        private readonly ConexionDb _conexionDb;
        public EmpresaDao(ConexionDb conexionDb)
        {
            _conexionDb= conexionDb;    
        }


        public List<EmpresaDto> ObtenerEmpresas()
        {
            string sql = "select emp.nit, emp.nombre, emp.direccion, emp.url_imagen,emp.telefono, " +
                "per.nombre as nombre_representante, per.id as id_representante,per.apellido, per.telefono as telefono_representante, " +
                "per.direccion as direccion_representante from empresa emp inner join " +
                "persona per on emp.id_representante = per.id";

            var listaEmpresas = new List<EmpresaDto>();
            try
            {
                
                using MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection);
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    var empresaDto = new EmpresaDto
                    {
                        Nit = reader.GetInt32("nit"),
                        Nombre = reader.GetString("nombre"),
                        Direccion = reader.GetString("direccion"),
                        UrlImagen = reader.GetString("url_imagen"),
                        Telefono = reader.GetInt64("telefono"),
                        Representante = new PersonaDto
                        {   
                            Id = reader.GetInt32("id_representante"),
                            Nombre = reader.GetString("nombre_representante"),
                            Apellido = reader.GetString("apellido"),
                            Telefono = reader.GetInt64("telefono_representante"),
                            Direccion = reader.GetString("direccion_representante")                      
                        }


                    };

                    listaEmpresas.Add(empresaDto);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return null;
            }

            if (listaEmpresas.Count == 0) return null;

            return listaEmpresas;
        }

        public int ObtenerIdRepresentante(int idEmpresa)
        {
            string sql = "select per.id from empresa emp inner join persona per " +
                        "on per.id = emp.id_representante where emp.nit = @id_empresa";
            int id_representante = 0;


            try
            {
                using MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection);
                command.Parameters.AddWithValue("@id_empresa", idEmpresa);
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id_representante = reader.GetInt32("id");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return 0;
            }


            return id_representante;
        }

        public int InsertarEmpresa(EmpresaTableDto empresa)
        {
            string sql = "INSERT INTO empresa VALUES (@nit, @nombre,@direccion,@url_imagen,@telefono,@id_representante,@id_usuario)";
            var MysqlConnection = _conexionDb.MysqlConnection;
            int filas = 0;
       
            try
            {
                using (MySqlCommand command = new MySqlCommand(sql, MysqlConnection))
                {
                    command.Parameters.AddWithValue("@nit", empresa.Nit);
                    command.Parameters.AddWithValue("@nombre", empresa.Nombre);
                    command.Parameters.AddWithValue("@direccion", empresa.Direccion);
                    command.Parameters.AddWithValue("@url_imagen", empresa.UrlImagen);
                    command.Parameters.AddWithValue("@telefono", empresa.Telefono);
                    command.Parameters.AddWithValue("@id_representante", empresa.IdRepresentante);
                    command.Parameters.AddWithValue("@id_usuario", empresa.IdUsuario);



                    filas = command.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.Write(ex.Message);
            }

            return filas;
        }

        public EmpresaEditarDto ObtenerEmpresaPorId(int idEmpresa)
        {
            string sql = "select emp.nit, emp.nombre, emp.telefono, emp.direccion, per.id, " +
                        "per.nombre as nombre_rep, per.apellido as apellido_rep, per.telefono " +
                        "as telefono_rep, per.direccion as direccion_rep " +
                        "from empresa emp inner join persona per on emp.id_representante = per.id " +
                        "where emp.nit = @id_empresa";

            EmpresaEditarDto empresa = new EmpresaEditarDto();
            
            try
            {
                using MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection);
                command.Parameters.AddWithValue("@id_empresa", idEmpresa);
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    empresa.Nit = reader.GetInt32("nit");
                    empresa.Nombre = reader.GetString("nombre");
                    empresa.Telefono = reader.GetInt64("telefono");
                    empresa.Direccion = reader.GetString("direccion");
                    empresa.IdRepresentante = reader.GetInt32("id");
                    empresa.NombreRepresentante = reader.GetString("nombre_rep");
                    empresa.ApellidoRepresentante = reader.GetString("apellido_rep");
                    empresa.TelefonoRepresentante = reader.GetInt64("telefono_rep");
                    empresa.DireccionRepresentante = reader.GetString("direccion_rep");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return null;
            }


            return empresa;
        }

        public int ActualizarEmpresaAdmin(EmpresaTableDto empresa)
        {
            int filas = 0;
            try
            {
                bool bandera = false;
                string SqlImagen = "";
                if (empresa.UrlImagen != null)
                {
                    SqlImagen = ", url_imagen=@url_imagen";
                    bandera = true;
                }

                string sql = "update empresa set nombre=@nombre, direccion=@direccion, " +
                    "telefono=@telefono" + SqlImagen +
                    " where nit=@id_empresa";
                using (MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection))
                {
                    command.Parameters.AddWithValue("@nombre", empresa.Nombre);
                    command.Parameters.AddWithValue("@direccion", empresa.Direccion);
                    command.Parameters.AddWithValue("@telefono", empresa.Telefono);
                    command.Parameters.AddWithValue("@id_empresa", empresa.Nit);
                    if (bandera) command.Parameters.AddWithValue("@url_imagen", empresa.UrlImagen);


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

        public string GetUrlEmpresa(int idEmpresa)
        {
            string sql = "select url_imagen from empresa where nit=@id_empresa";
            string urlImagen = "";


            try
            {
                using MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection);
                command.Parameters.AddWithValue("@id_empresa", idEmpresa);
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
        public int GetIdRepresentante(int idEmpresa)
        {
            string sql = "select per.id from empresa emp inner join persona per " +
                        "on per.id = emp.id_representante where emp.nit = @id_empresa";
            int id_representante = 0;


            try
            {
                using MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection);
                command.Parameters.AddWithValue("@id_empresa", idEmpresa);
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id_representante = reader.GetInt32("id");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return 0;
            }


            return id_representante;
        }

        public int EliminarEmpresa(int idEmpresa, int idUsuario, int idPersona)
        {
            int filas = 0;
            int filasUsuario = 0;
            int filasPersona = 0;
            MySqlConnection Mysqlconnection = _conexionDb.MysqlConnection;
            using MySqlTransaction transaction = Mysqlconnection.BeginTransaction();
            string sql = "DELETE FROM empresa WHERE nit=@id_empresa";
            string sqlUsuario = "delete from usuario where id=@id_usuario";
            string sqlPersona = "delete from persona where id=@id_persona";

            try
            {

                var command = Mysqlconnection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id_empresa", idEmpresa);
                filas = command.ExecuteNonQuery();

                command = Mysqlconnection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = sqlUsuario;
                command.Parameters.AddWithValue("@id_usuario", idUsuario);
                filasUsuario = command.ExecuteNonQuery();

                command = Mysqlconnection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = sqlPersona;
                command.Parameters.AddWithValue("@id_persona", idPersona);
                filasPersona = command.ExecuteNonQuery();

                if (filas > 0 && filasUsuario > 0 && filasPersona > 0)
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
                Console.WriteLine(ex.Message);
                return 0;
            }

            return filas;
        }

        public int GetIdUsuario(int idEmpresa)
        {
            string sql = "select id_usuario from empresa where nit=@id_empresa";
            int idUsuario = 0;


            try
            {
                using MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection);
                command.Parameters.AddWithValue("@id_empresa", idEmpresa);
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    idUsuario = reader.GetInt32("id_usuario");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return idUsuario;
            }


            return idUsuario;
        }

    }
}
