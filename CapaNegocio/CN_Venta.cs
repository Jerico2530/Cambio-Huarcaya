using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Venta
    {
        private CD_Venta objcd_Venta = new CD_Venta();

        public bool RestarStock(int idMonera, int cantidad)
        {
            return objcd_Venta.RestarStock(idMonera, cantidad);
        }

        public bool SumarStock(int idMonera, int cantidad)
        {
            return objcd_Venta.SumarStock(idMonera, cantidad);
        }

        public int ObtenerCorrelativo()
        {
            return objcd_Venta.ObtenerCorrelativo();
        }

        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            return objcd_Venta.Registrar(obj, DetalleVenta, out Mensaje);

        }

        public Venta ObtenerVenta(string numero)
        {
            Venta oVenta = objcd_Venta.ObtenerVenta(numero);

            if (oVenta.IdVenta != 0)
            {
                List<Detalle_Venta> oDetalleVenta = objcd_Venta.ObtenerDetalleVenta(oVenta.IdVenta);
                oVenta.oDetalleVenta = oDetalleVenta;
            }
            return oVenta;
        }
        public List<Venta> Listar()
        {
            return objcd_Venta.Listar();
        }
    }
}
