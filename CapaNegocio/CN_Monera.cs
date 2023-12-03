using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Monera
    {

        private CD_Monera objcd_Monera = new CD_Monera();

        public List<Monera> Listar()
        {
            return objcd_Monera.Listar();
        }

        public int Registrar(Monera obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Codigo == "")
            {
                Mensaje += "Es necesario el Codigo del Monera\n";
            }
            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario el Nombre del Monera\n";
            }
            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objcd_Monera.Registrar(obj, out Mensaje);
            }


        }

        public bool Editar(Monera obj, out string Mensaje)
        {
            Mensaje = string.Empty;


            if (obj.Codigo == "")
            {
                Mensaje += "Es necesario el Codigo del Monera\n";
            }
            if (obj.Nombre == "")
            {
                Mensaje += "Es necesario el Nombre del Monera\n";
            }
            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_Monera.Editar(obj, out Mensaje);
            }

        }

        public bool Eliminar(Monera obj, out string Mensaje)
        {
            return objcd_Monera.Eliminar(obj, out Mensaje);
        }
    }
}
