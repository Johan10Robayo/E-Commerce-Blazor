using MercaMetaBlazor.Data.Dao;
using MercaMetaBlazor.Data.Dto;

namespace MercaMetaBlazor.Services
{
    public class PersonaService
    {
            private readonly PersonaDao _personaDao;
            private readonly EmpresaDao _empresaDao;
            public PersonaService(PersonaDao personaDto, EmpresaDao empresaDao)
            {
                _personaDao = personaDto; 
                _empresaDao= empresaDao;
            }

        public PersonaDto ObenerRepresentanteDeEmpresa(int idEmpresa)
        {

            var representante = _personaDao.ObtenerRepresentanteDeEmpresa(idEmpresa);
            return representante;
        }

        public int ActualizarPersona(PersonaDto representante, int idEmpresa)
        {
            int id_actual = _empresaDao.ObtenerIdRepresentante(idEmpresa);
            int filas = _personaDao.ActualizarPersonaAdmin(representante,id_actual);


            return filas;
        }

        public List<PersonaDto> ObtenerEmpleadosDeEmpresa(int IdEmpresa)
        {
            var empleados = _personaDao.ObtenerEmpleadosDeEmpresa(IdEmpresa);

            return empleados;
        
        }

    }
}
