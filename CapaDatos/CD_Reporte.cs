using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Reporte
    {
        public List<ReporteCompra> Compra(string fechainicio, string fechafin, int idproveedor)
        {
            List<ReporteCompra> lista = new List<ReporteCompra>();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();

                    SqlCommand cmd = new SqlCommand("sp_ReporteCompras", conexion);
                    cmd.Parameters.AddWithValue("fechainicio", fechainicio);
                    cmd.Parameters.AddWithValue("fechafin", fechafin);
                    cmd.Parameters.AddWithValue("idproveedor", idproveedor);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ReporteCompra()
                            {

                                FechaRegistro = reader["FechaRegistro"].ToString(),
                                TipoDocumento = reader["TipoDocumento"].ToString(),
                                NumeroDocumento = reader["NumeroDocumento"].ToString(),
                                MontoTotal = reader["MontoTotal"].ToString(),
                                UsuarioRegistro = reader["UsuarioRegistro"].ToString(),
                                DocumentoProveedor = reader["DocumentoProveedor"].ToString(),
                                Banco = reader["Banco"].ToString(),
                                CodigoProducto = reader["CodigoProducto"].ToString(),
                                NombreProducto = reader["NombreProducto"].ToString(),
                                Categoria = reader["Categoria"].ToString(),
                                PrecioCompra = reader["PrecioCompra"].ToString(),
                                PrecioVenta = reader["PrecioVenta"].ToString(),
                                Cantidad = reader["Cantidad"].ToString(),
                                Subtotal = reader["Subtotal"].ToString(),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<ReporteCompra>();
                }
            }
            return lista;
        }

        public List<ReporteVenta> Venta(string fechainicio, string fechafin)
        {
            List<ReporteVenta> lista = new List<ReporteVenta>();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();

                    SqlCommand cmd = new SqlCommand("sp_ReporteVentas", conexion);
                    cmd.Parameters.Add("@fechainicio", SqlDbType.VarChar, 10).Value = fechainicio;
                    cmd.Parameters.Add("@fechafin", SqlDbType.VarChar, 10).Value = fechafin;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ReporteVenta()
                            {

                                FechaRegistro = reader["FechaRegistro"].ToString(),
                                TipoDocumento = reader["TipoDocumento"].ToString(),
                                NumeroDocumento = reader["NumeroDocumento"].ToString(),
                                MontoTotal = reader["MontoTotal"].ToString(),
                                UsuarioRegistro = reader["UsuarioRegistro"].ToString(),
                                DocumentoCliente = reader["DocumentoCliente"].ToString(),
                                NombreCliente = reader["NombreCliente"].ToString(),
                                CodigoProducto = reader["CodigoProducto"].ToString(),
                                NombreProducto = reader["NombreProducto"].ToString(),
                                Pais = reader["Pais"].ToString(),
                                PrecioVenta = reader["PrecioVenta"].ToString(),
                                Cantidad = reader["Cantidad"].ToString(),
                                Subtotal = reader["Subtotal"].ToString(),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error en la capa de datos: " + ex.Message);
                    throw;
                }
            }
            return lista;
        }
    }
}
