using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Compra
    {
        public int ObtenerCorrelativo()
        {
            int idcorrelativo = 0;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT COUNT(*) + 1 FROM COMPRA");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    idcorrelativo = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    idcorrelativo = 0;
                }
            }
            return idcorrelativo;
        }

        public bool Registrar(Compra obj, DataTable DetalleCompra, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                   
                    
                        SqlCommand cmd = new SqlCommand("sp_RegistrarComprass", oconexion);
                        cmd.Parameters.AddWithValue("IdUsuario", obj.oUsuario.IdUsuario);
                        cmd.Parameters.AddWithValue("IdProveedor", obj.oProveedor != null ? obj.oProveedor.IdProveedor : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("IdCliente", obj.oCliente != null ? obj.oCliente.IdCliente : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("TipoDocumento", obj.TipoDocumento);
                        cmd.Parameters.AddWithValue("NumeroDocumento", obj.NumeroDocumento);
                        cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                        cmd.Parameters.AddWithValue("DetalleCompra", DetalleCompra);
                        cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        cmd.CommandType = CommandType.StoredProcedure;
                        oconexion.Open();
                        cmd.ExecuteNonQuery();
                        Resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                        Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                    
                }
                catch (Exception ex)
                {
                    Resultado = false;
                    Mensaje = ex.Message;
                }
            }
            return Resultado;
        }
        public Compra ObtenerCompra(string numero)
        {
            Compra obj = new Compra();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT C.IdCompra, u.NombreCompleto, pr.Documento AS DocumentoProveedor, pr.Banco,");
                    query.AppendLine("cl.Documento AS DocumentoCliente, cl.NombreCompleto AS NombreCompletoCliente,");
                    query.AppendLine("c.TipoDocumento, c.NumeroDocumento, c.MontoTotal, CONVERT(char(10), c.FechaRegistro, 103) AS FechaRegistro");
                    query.AppendLine("FROM COMPRA c");
                    query.AppendLine("INNER JOIN USUARIO u ON u.IdUsuario = c.IdUsuario");
                    query.AppendLine("LEFT JOIN PROVEEDOR pr ON pr.IdProveedor = c.IdProveedor");
                    query.AppendLine("LEFT JOIN CLIENTE cl ON cl.IdCliente = c.IdCliente");
                    query.AppendLine("WHERE c.NumeroDocumento = @numero");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@numero", numero);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            obj = new Compra()
                            {
                                IdCompra = Convert.ToInt32(reader["IdCompra"].ToString()),
                                oUsuario = new Usuario()
                                {
                                    NombreCompleto = reader["NombreCompleto"].ToString()
                                },
                                oProveedor = new Proveedor()
                                {
                                    Documento = reader["DocumentoProveedor"].ToString(),
                                    Banco = reader["Banco"].ToString(),
                                },
                                oCliente = new Cliente()
                                {
                                    Documento = reader["DocumentoCliente"].ToString(),
                                    NombreCompleto = reader["NombreCompletoCliente"].ToString(),
                                },
                                TipoDocumento = reader["TipoDocumento"].ToString(),
                                NumeroDocumento = reader["NumeroDocumento"].ToString(),
                                MontoTotal = Convert.ToDecimal(reader["MontoTotal"].ToString()),
                                FechaRegistro = reader["FechaRegistro"].ToString(),
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    obj = new Compra();
                }
            }
            return obj;
        }

        public List<Compra> Listar()
        {
            List<Compra> oCompra = new List<Compra>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select IdCompra, TipoDocumento, NumeroDocumento, MontoTotal, CONVERT(CHAR(10),FechaRegistro,103) FechaRegistro from compra");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            oCompra.Add(new Compra()
                            {

                                IdCompra = Convert.ToInt32(reader["IdCompra"].ToString()),
                                TipoDocumento = reader["TipoDocumento"].ToString(),
                                NumeroDocumento = reader["NumeroDocumento"].ToString(),
                                MontoTotal = Convert.ToDecimal(reader["MontoTotal"].ToString()),
                                FechaRegistro = reader["FechaRegistro"].ToString(),

                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    oCompra = new List<Compra>();
                }
            }
            return oCompra;
        }
        public List<Detalle_Compra> ObtenerDetalle(int idcompra)
        {
            List<Detalle_Compra> oLista = new List<Detalle_Compra>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT p.Nombre, dc.PrecioCompra, dc.Cantidad, dc.MontoTotal");
                    query.AppendLine("from DETALLE_COMPRA dc");
                    query.AppendLine("inner join MONERA p on p.IdMonera=dc.IdMonera");
                    query.AppendLine("where dc.IdMonera  = @idcompra");


                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("idcompra", idcompra);
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new Detalle_Compra()
                            {
                                oMonera = new Monera() { Nombre = dr["Nombre"].ToString() },
                                PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"].ToString()),
                                Cantidad = Convert.ToInt32(dr["Cantidad"].ToString()),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"].ToString()),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oLista = new List<Detalle_Compra>();
            }
            return oLista;     
        }
    }        
}





