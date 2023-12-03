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
    public class CD_Usuario
    {
        // Método para listar usuarios
        public List<Usuario> Listar()
        {
            // Se crea una lista para almacenar usuarios
            List<Usuario> lista = new List<Usuario>();

            // Se establece la conexión a la base de datos
            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    // Se construye la consulta SQL para obtener usuarios y sus roles
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT u.IdUsuario, u.Documento, u.NombreCompleto, u.Correo, u.Clave, u.Estado, r.IdRol, r.Descripcion");
                    query.AppendLine("FROM USUARIO u");
                    query.AppendLine("INNER JOIN ROL r ON r.IdRol = u.IdRol");

                    // Se crea el comando SQL con la consulta y la conexión
                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.CommandType = CommandType.Text;

                    // Se abre la conexión
                    conexion.Open();

                    // Se ejecuta la consulta y se lee el resultado
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Se crea un objeto Usuario por cada fila y se agrega a la lista
                            lista.Add(new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                Documento = reader["Documento"].ToString(),
                                NombreCompleto = reader["NombreCompleto"].ToString(),
                                Correo = reader["Correo"].ToString(),
                                Clave = reader["Clave"].ToString(),
                                Estado = Convert.ToBoolean(reader["Estado"]),
                                oRol = new Rol() { IdRol = Convert.ToInt32(reader["IdRol"]), Descripcion = reader["Descripcion"].ToString() }
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // En caso de error, se reinicializa la lista
                    lista = new List<Usuario>();
                }
            }

            // Se devuelve la lista de usuarios
            return lista;
        }

        // Método para registrar un nuevo usuario
        public int Registrar(Usuario obj, out string Mensaje)
        {
            // Se inicializan variables
            int idusuariogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                // Se establece la conexión a la base de datos
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    // Se crea el comando SQL para ejecutar el procedimiento almacenado de registro
                    SqlCommand cmd = new SqlCommand("sp_RegistroUsuario", conexion);

                    // Se agregan parámetros al comando con los valores del objeto Usuario
                    cmd.Parameters.AddWithValue("Documento", obj.Documento);
                    cmd.Parameters.AddWithValue("NombreCompleto", obj.NombreCompleto);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("IdRol", obj.oRol.IdRol);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);
                    cmd.Parameters.Add("IdUsuarioResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Se abre la conexión
                    conexion.Open();

                    // Se ejecuta el comando
                    cmd.ExecuteNonQuery();

                    // Se obtiene el ID del usuario generado y cualquier mensaje asociado
                    idusuariogenerado = Convert.ToInt32(cmd.Parameters["IdUsuarioResultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                   
                }
            }
            catch (Exception ex)
            {
                // En caso de error, se asignan valores apropiados a las variables de salida
                idusuariogenerado = 0;
                Mensaje = ex.Message;
            }

            // Se devuelve el ID del usuario generado
            return idusuariogenerado;
        }

        // Método para editar un usuario existente
        public bool Editar(Usuario obj, out string Mensaje)
        {
            // Se inicializan variables
            bool Respuesta = false;
            Mensaje = string.Empty;

            try
            {
                // Se establece la conexión a la base de datos
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    // Se crea el comando SQL para ejecutar el procedimiento almacenado de edición
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", conexion);

                    // Se agregan parámetros al comando con los valores del objeto Usuario
                    cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("Documento", obj.Documento);
                    cmd.Parameters.AddWithValue("NombreCompleto", obj.NombreCompleto);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("IdRol", obj.oRol.IdRol);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);
                    cmd.Parameters.Add("Respuesta", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Se abre la conexión
                    conexion.Open();

                    // Se ejecuta el comando
                    cmd.ExecuteNonQuery();

                    // Se obtiene la respuesta y cualquier mensaje asociado
                    Respuesta = Convert.ToBoolean(cmd.Parameters["Respuesta"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                // En caso de error, se asignan valores apropiados a las variables de salida
                Respuesta = false;
                Mensaje = ex.Message;
            }

            // Se devuelve la respuesta
            return Respuesta;
        }

        // Método para eliminar un usuario
        public bool Eliminar(Usuario obj, out string Mensaje)
        {
            // Se inicializan variables
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                // Se establece la conexión a la base de datos
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    // Se crea el comando SQL para ejecutar el procedimiento almacenado de eliminación
                    SqlCommand cmd = new SqlCommand("sp_EliminarUsuario", conexion);

                    // Se agrega el parámetro con el ID del usuario a eliminar
                    cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                    cmd.Parameters.Add("Respuesta", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Se abre la conexión
                    conexion.Open();

                    // Se ejecuta el comando
                    cmd.ExecuteNonQuery();

                    // Se obtiene la respuesta y cualquier mensaje asociado
                    respuesta = Convert.ToBoolean(cmd.Parameters["Respuesta"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                // En caso de error, se asignan valores apropiados a las variables de salida
                respuesta = false;
                Mensaje = ex.Message;
            }

            // Se devuelve la respuesta
            return respuesta;
        }
    }
}
