using MercaMetaBlazor.Data.Dao;
using MercaMetaBlazor.Data.Dto;
using MercaMetaBlazor.Data.ModelView;

namespace MercaMetaBlazor.Services
{
    public class EmpresaService
    {
        private readonly EmpresaDao _empresaDao;
        private readonly PersonaDao _personaDao;
        private readonly UsuarioDao _usuarioDao;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public EmpresaService(EmpresaDao empresaDao, PersonaDao personaDao, UsuarioDao usuarioDao, IWebHostEnvironment hostingEnvironment)
        {
            _empresaDao = empresaDao;
            _personaDao = personaDao;
            _usuarioDao = usuarioDao;
            _hostingEnvironment = hostingEnvironment;
        }

        public List<EmpresaDto> ObtenerEmpresas()
        { 
            var empresas = _empresaDao.ObtenerEmpresas();
            return empresas;
        }

        public int InsertarEmpresa(EmpresaModelView empresa)
        {

            var personaDto = new PersonaDto
            {
                Id = empresa.IdRepresentante,
                Nombre = empresa.NombreRepresentante,
                Apellido = empresa.ApellidoRepresentante,
                Direccion = empresa.DireccionRepresentante,
                Telefono = empresa.TelefonoRepresentante
            };



            
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(empresa.Password);
            var usuarioDto = new UsuarioDto
            {
                Email = empresa.Usuario,
                Paswd = hashPassword,
                Rol = "Empresa"

            };

            var personaInsertada = _personaDao.InsertarPersona(personaDto);
            if (personaInsertada == 0) return 0;


            var usuarioInsertado = _usuarioDao.InsertarUsuario(usuarioDto);
            if (usuarioInsertado == 0) return 0;

            int idUsuario = _usuarioDao.ObtenerIdUsuario(usuarioDto.Email, usuarioDto.Rol);
            if (idUsuario == 0) return 0;

            //guardar la imagen
            var imagen = empresa.Imagen;
            // Obtener el nombre del archivo
            string nombreArchivo = Path.GetFileName(imagen.FileName);

            // Crear la ruta de guardado
            string rutaDeGuardado = Path.Combine(_hostingEnvironment.WebRootPath, "img", nombreArchivo);
            string rutadb = "/img/" + nombreArchivo;

            // Crear un archivo en la ruta de guardado
            using (var stream = new FileStream(rutaDeGuardado, FileMode.Create))
            {
                // Copiar los datos de la imagen en el archivo
                imagen.CopyTo(stream);
            }

            var empresaTableDto = new EmpresaTableDto
            {
                Nit = empresa.Nit,
                Nombre = empresa.Nombre,
                Direccion = empresa.Direccion,
                Telefono = empresa.Telefono,
                UrlImagen = rutadb,
                IdRepresentante = empresa.IdRepresentante,
                IdUsuario = idUsuario
            };

            var empresaInsertada = _empresaDao.InsertarEmpresa(empresaTableDto);
            if (empresaInsertada == 0 ) return 0;

            return empresaInsertada;

        }

        public EmpresaEditarDto ObtenerEmpresaPorId(int idEmpresa)
        {
            var empresa = _empresaDao.ObtenerEmpresaPorId(idEmpresa);
            return empresa;
        
        }

        public int EditarEmpresa(EmpresaEditarDto empresa)
        {
            var representante = new PersonaDto
            {
                Id = empresa.IdRepresentante,
                Nombre = empresa.NombreRepresentante,
                Direccion = empresa.DireccionRepresentante,
                Telefono = empresa.TelefonoRepresentante,
                Apellido = empresa.ApellidoRepresentante
            };
            var id_persona_actual = _empresaDao.GetIdRepresentante(empresa.Nit);

            var filasSqlPersona = _personaDao.ActualizarPersonaAdmin(representante, id_persona_actual);
            if (filasSqlPersona == 0) return 0;
            
            EmpresaTableDto empresaDto = new EmpresaTableDto();

            if (empresa.Imagen != null)
            {

                var urlImagen = _empresaDao.GetUrlEmpresa(empresa.Nit);

                string rutaServer = _hostingEnvironment.WebRootPath;
                string rutaImagenEliminar = rutaServer + urlImagen.Replace("/img/", "\\img\\");

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
                var imagen = empresa.Imagen;

                string nombreArchivo = Path.GetFileName(imagen.FileName);

                string rutaDeGuardado = Path.Combine(rutaServer, "img", nombreArchivo);
                string rutadb = "/img/" + nombreArchivo;
                empresaDto.UrlImagen = rutadb;

                using (var stream = new FileStream(rutaDeGuardado, FileMode.Create))
                {

                    imagen.CopyTo(stream);
                }


            }
            empresaDto.Nit = empresa.Nit;
            empresaDto.Nombre = empresa.Nombre;
            empresaDto.Direccion = empresa.Direccion;
            empresaDto.Telefono = empresa.Telefono;



            int filasSqlEmpresa = _empresaDao.ActualizarEmpresaAdmin(empresaDto);

            return filasSqlEmpresa;
        }

        public int EliminarEmpresaAdmin(int idEmpresa)
        {
            string urlImagen = _empresaDao.GetUrlEmpresa(idEmpresa);
            int idPersona = _empresaDao.GetIdRepresentante(idEmpresa);
            int idUsuario = _empresaDao.GetIdUsuario(idEmpresa);

            var filasSqlEmpresa = _empresaDao.EliminarEmpresa(idEmpresa, idUsuario, idPersona);
            if (filasSqlEmpresa == 0) return 0;




            string rutaServer = _hostingEnvironment.WebRootPath;
            string rutaImagenEliminar = rutaServer + urlImagen.Replace("/img/", "\\img\\");


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

            return filasSqlEmpresa;

        }
    }

}
