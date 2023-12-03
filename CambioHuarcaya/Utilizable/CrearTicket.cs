using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace CambioHuarcaya.Utilizable
{
    public static class CrearTicket
    {

        static StringBuilder linea = new StringBuilder();
        static int maxCant = 50;
        private static void AgregarCaracter(string c)
        {
            string texto = "";
            for (int i = 0; i < maxCant; i++)
            {
                texto += c;
            }
            linea.AppendLine(texto);
        }
        private static void AgregarTextoCentro(string texto)
        {

            if (texto.Length > maxCant)
            {

            }
            else
            {
                decimal agregarespacio = Math.Truncate(Convert.ToDecimal((maxCant - texto.Length) / 2));
                string espacios = "";
                for (int i = 0; i < agregarespacio; i++)
                {
                    espacios += " ";
                }
                linea.AppendLine(espacios + texto);
            }
        }

        private static void AgregarDosColumnas(string texto1, string texto2)
        {
            int cantidadtexto = texto1.Length + texto2.Length;
            if (cantidadtexto > maxCant)
            {

            }
            else
            {
                int cantidadespacio = maxCant - cantidadtexto;
                string espacios = "";
                for (int i = 0; i < cantidadespacio; i++)
                {
                    espacios += " ";
                }
                linea.AppendLine(texto1 + espacios + texto2);
            }

        }
        public static string crearTicketVenta(string _codigoVenta)
        {
            string tickettexto = Properties.Resources.Ticket.ToString();
            Venta oVenta = new CN_Venta().ObtenerVenta(_codigoVenta);
            Negocio oNegocio = new CN_Negocio().ObtenerDatos();

            tickettexto = tickettexto.Replace("¡nombreempresa!", oNegocio.Nombre.ToUpper());
            tickettexto = tickettexto.Replace("¡documentoempresa!", oNegocio.RFC);
            tickettexto = tickettexto.Replace("¡correoempresa!", oNegocio.Correo);
            tickettexto = tickettexto.Replace("!telefonoempresa¡", oNegocio.Telefono);

            tickettexto = tickettexto.Replace("¡tipodocumento!", oVenta.TipoDocumento);
            tickettexto = tickettexto.Replace("¡numerodocumento!", oVenta.NumeroDocumento);
            tickettexto = tickettexto.Replace("¡fechaventa!", oVenta.FechaRegistro);

            StringBuilder tr = new StringBuilder();
            foreach (Detalle_Venta dv in oVenta.oDetalleVenta)
            {
                tr.AppendLine("<tr>");
                tr.AppendLine("<td width=\"20\">" + dv.Cantidad + "</td>");
                tr.AppendLine("<td width=\"180\">" + dv.oMonera.Nombre + "</td>");
                tr.AppendLine("<td style=\"font-size:14px\">" + dv.PrecioVenta.ToString("0.00", new CultureInfo("es-PE")) + "</td>");
                tr.AppendLine("<td style=\"font-size:14px\">" + dv.SubTotal.ToString("0.00", new CultureInfo("es-PE")) + "</td>");
                tr.AppendLine("</tr>");
            }

            tickettexto = tickettexto.Replace("¡detalleventa!", tr.ToString());

            tickettexto = tickettexto.Replace("¡totalpagar!", oVenta.MontoTotal.ToString("$ #,##0.00", new CultureInfo("es-PE")));
            tickettexto = tickettexto.Replace("¡pagocon!", oVenta.MontoPago.ToString("$ #,##0.00", new CultureInfo("es-PE")));
            tickettexto = tickettexto.Replace("¡cambio!", oVenta.MontoCambio.ToString("$ #,##0.00", new CultureInfo("es-PE")));

            return tickettexto;
        }
    }
}
