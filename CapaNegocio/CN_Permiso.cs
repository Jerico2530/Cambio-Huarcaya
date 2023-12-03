using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Permiso
    {
        private CD_Permiso objcd_permiso = new CD_Permiso();

        public List<Permiso> Listar(int IdUsuario)
        {
            return objcd_permiso.Listar(IdUsuario);
        }
        public List<Permiso> ListarPermisos(int IdRol)
        {
            return objcd_permiso.ListarPermisos(IdRol);
        }
        public List<Permiso> ListarMenus()
        {
            return objcd_permiso.ListarMenus();
        }
        public int Registrar(Permiso oPermiso, out string mensaje)
        {
            return objcd_permiso.Registrar(oPermiso, out mensaje);
        }
        public int Editar(Permiso oPermiso, out string mensaje)
        {
            return objcd_permiso.Editar(oPermiso, out mensaje);
        }
    }
}
