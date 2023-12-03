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
    public class CD_Permiso
    {
        public List<Permiso> Listar(int idusuario)
        {
            // Se crea una lista para almacenar objetos Permiso
            List<Permiso> lista = new List<Permiso>();

            // Se utiliza un bloque 'using' para gestionar la conexión a la base de datos
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    // Se construye la consulta SQL para obtener permisos de un usuario
                    StringBuilder query = new StringBuilder();
                    query.AppendLine(" SELECT p.IdRol, p.NombreMenu ,p.Estado FROM PERMISO p");
                    query.AppendLine("INNER JOIN ROL r ON r.IdRol = p.IdRol");
                    query.AppendLine("INNER JOIN USUARIO u ON u.IdRol = r.IdRol");
                    query.AppendLine("WHERE u.IdUsuario = @idusuario");

                    // Se crea el comando SQL con la consulta y la conexión
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@idusuario", idusuario);
                    cmd.CommandType = CommandType.Text;

                    // Se abre la conexión
                    oconexion.Open();

                    // Se ejecuta la consulta y se lee el resultado
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Se crea un objeto Permiso por cada fila y se agrega a la lista
                            lista.Add(new Permiso()
                            {
                                oRol = new Rol() { IdRol = Convert.ToInt32(reader["IdRol"]) },
                                NombreMenu = reader["NombreMenu"].ToString(),
                                Estado = Convert.ToBoolean(reader["Estado"].ToString())
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // En caso de error, se reinicializa la lista
                    lista = new List<Permiso>();
                }
            }

            // Se devuelve la lista de permisos
            return lista;
        }
        public List<Permiso> ListarPermisos(int IdRol)
        {
            List<Permiso> lista = new List<Permiso>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT P.IdRol, P.NombreMenu, p.Estado FROM PERMISO P ");
                    query.AppendLine("INNER JOIN ROL R ON R.IdRol = P.IdRol ");
                    query.AppendLine("WHERE r.IdRol = @IdRol");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("IdRol", IdRol);
                    cmd.CommandType = CommandType.Text;
                    oConexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Permiso()
                            {
                                oRol = new Rol()
                                {
                                    IdRol = Convert.ToInt32(reader["IdRol"].ToString())
                                },
                                NombreMenu = reader["NombreMenu"].ToString(),
                                Estado = Convert.ToBoolean(reader["Estado"].ToString())
                            });
                        }
                    }
                    oConexion.Close();
                }
                catch (Exception ex)
                {

                }
            }
            return lista;
        }
        public List<Permiso> ListarMenus()
        {
            List<Permiso> ls = new List<Permiso>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select DISTINCT NombreMenu from PERMISO ");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.CommandType = CommandType.Text;
                    oConexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ls.Add(new Permiso()
                            {
                                NombreMenu = reader["NombreMenu"].ToString()
                            });
                        }
                    }
                    oConexion.Close();
                }
                catch (Exception ex)
                {
                    ls = new List<Permiso>();
                }
            }
            return ls;
        }
        public int Registrar(Permiso oPermiso, out string Mensaje)
        {
            // @Descripcion varchar(50),
            int result = 0;
            Mensaje = String.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("insert into PERMISO(IdRol,NombreMenu, Estado) values ");
                    query.AppendLine("(@IdRol, @NombreMenu, @Estado) ");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("IdRol", oPermiso.oRol.IdRol);
                    cmd.Parameters.AddWithValue("NombreMenu", oPermiso.NombreMenu);
                    cmd.Parameters.AddWithValue("Estado", oPermiso.Estado);
                    cmd.CommandType = CommandType.Text;
                    oConexion.Open();
                    result = cmd.ExecuteNonQuery();
                    oConexion.Close();
                }
            }
            catch (Exception ex)
            {
                result = 0;
                Mensaje = ex.Message;
            }
            return result;
        }
        public int Editar(Permiso oPermiso, out string Mensaje)
        {
            int result = 0;
            Mensaje = String.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cadena))
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update PERMISO SET Estado = @Estado ");
                    query.AppendLine("Where IdRol = @IdRol AND NombreMenu = @NombreMenu ");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("IdRol", oPermiso.oRol.IdRol);
                    cmd.Parameters.AddWithValue("NombreMenu", oPermiso.NombreMenu);
                    cmd.Parameters.AddWithValue("Estado", oPermiso.Estado);
                    cmd.CommandType = CommandType.Text;
                    oConexion.Open();
                    result = cmd.ExecuteNonQuery();
                    oConexion.Close();
                }
            }
            catch (Exception ex)
            {
                result = 0;
                Mensaje = ex.Message;
            }
            return result;
        }
    }
}
