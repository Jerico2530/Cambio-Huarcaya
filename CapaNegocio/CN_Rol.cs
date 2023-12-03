using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Rol
    {
        private CD_Rol objcd_rol = new CD_Rol();

        public List<Rol> Listar()
        {
            return objcd_rol.Listar();
        }
        public int Registrar(Rol rol, out string mensaje)
        {
            mensaje = string.Empty;
            if (rol.Descripcion == string.Empty)
            {
                mensaje += "Ingresa un rol";
                return 0;
            }
            else
            {
                return objcd_rol.Registrar(rol, out mensaje);
            }
        }
        public int IdRol()
        {
            return objcd_rol.IdRol();
        }
    }
}
