using MercaMetaBlazor.Data.Dto;
using MySql.Data.MySqlClient;

namespace MercaMetaBlazor.Data.Dao
{
    public class PersonaDao
    {

        private readonly ConexionDb _conexionDb;

        public PersonaDao(ConexionDb conexionDb)
        {
            _conexionDb = conexionDb;
        }

        public PersonaDto ObtenerRepresentanteDeEmpresa(int idEmpresa)
        {
            string sql = "select per.id as id_persona, per.nombre as nombre_persona, per.apellido as apellido_persona, " +
                "per.direccion as direccion_persona, per.telefono as telefono_persona from persona per " +
                "inner join empresa emp on emp.id_representante = per.id " +
                "where emp.nit = @id_empresa";

            PersonaDto personaDto = new PersonaDto();   
            try
            {
                using MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection);
                command.Parameters.AddWithValue("@id_empresa", idEmpresa);
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    personaDto.Id = reader.GetInt32("id_persona");
                    personaDto.Nombre = reader.GetString("nombre_persona");
                    personaDto.Apellido = reader.GetString("apellido_persona");
                    personaDto.Direccion = reader.GetString("direccion_persona");
                    personaDto.Telefono = reader.GetInt64("telefono_persona");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }

            
            return personaDto;
        }

        public int ActualizarPersonaAdmin(PersonaDto persona, int idPersonaActual)
        {
            int filas = 0;
            try
            {
                string sql = "update persona set id=@id, nombre=@nombre, apellido=@apellido," +
                    " telefono=@telefono, direccion=@direccion where id=@id_persona_actual";
                using MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection);
                command.Parameters.AddWithValue("@id", persona.Id);
                command.Parameters.AddWithValue("@nombre", persona.Nombre);
                command.Parameters.AddWithValue("@apellido", persona.Apellido);
                command.Parameters.AddWithValue("@telefono", persona.Telefono);
                command.Parameters.AddWithValue("@direccion", persona.Direccion);
                command.Parameters.AddWithValue("@id_persona_actual", idPersonaActual);

                filas = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }

            return filas;
        }

        public List<PersonaDto> ObtenerEmpleadosDeEmpresa(int idEmpresa)
        {
            string sql = "select per.id, per.nombre, per.apellido, per.direccion, per.telefono " +
                "from empleados empl inner join persona per on empl.id_persona = per.id " +
                "where empl.id_empresa = @id_empresa";

            List<PersonaDto> empleados = new List<PersonaDto>();
            try
            {
                using MySqlCommand command = new MySqlCommand(sql, _conexionDb.MysqlConnection);
                command.Parameters.AddWithValue("@id_empresa", idEmpresa);
                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PersonaDto persona = new PersonaDto
                    {
                        Id = reader.GetInt32("id"),
                        Nombre = reader.GetString("nombre"),
                        Apellido = reader.GetString("apellido"),
                        Direccion = reader.GetString("direccion"),
                        Telefono = reader.GetInt64("telefono")
                    };
                    empleados.Add(persona);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return null;
            }

            if (empleados.Count == 0) return null;

            return empleados;
        }

        public int InsertarPersona(PersonaDto personaDto)
        {
            string sql = "INSERT INTO persona VALUES (@id, @nombre, @apellido, @telefono, @direccion)";
            var MysqlConnection = _conexionDb.MysqlConnection;
            int filas = 0;
            using MySqlTransaction transaction = MysqlConnection.BeginTransaction();

            try
            {

                var command = MysqlConnection.CreateCommand();
                command.CommandText = sql;
                command.Parameters.AddWithValue("@id", personaDto.Id);
                command.Parameters.AddWithValue("@nombre", personaDto.Nombre);
                command.Parameters.AddWithValue("@apellido", personaDto.Apellido);
                command.Parameters.AddWithValue("@telefono", personaDto.Telefono);
                command.Parameters.AddWithValue("@direccion", personaDto.Direccion);

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
    }
}
