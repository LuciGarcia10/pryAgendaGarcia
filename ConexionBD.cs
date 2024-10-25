using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pryAgendaGarcia
{
    public class ConexionBD
    {
        OleDbConnection conexion;
        OleDbCommand comando;
        OleDbDataAdapter adaptador;

        string cadena;

        public ConexionBD()
        {
            cadena = "Provider=Microsoft.ACE.OLEDB.12.0 ;Data Source= AgendaGarcia.accdb";
        }
        public DataTable ObtenerContactosPorCategoria()
        {
            using (OleDbConnection conexion = new OleDbConnection(cadena))
            {
                using (OleDbCommand comando = new OleDbCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "SELECT * FROM Contacto ORDER BY Categoria"; // Ordenar por categoría para una mejor visualización

                    using (OleDbDataAdapter adaptador = new OleDbDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adaptador.Fill(tabla);
                        return tabla;
                    }
                }
            }
        }


        // Método para listar todos los contactos
        public void listarContactos(DataGridView dgvcontactos)
        {
            try
            {
                conexion = new OleDbConnection(cadena);
                comando = new OleDbCommand();

                comando.Connection = conexion;
                comando.CommandType = CommandType.Text;
                comando.CommandText = "SELECT * FROM Contacto";// Selecciona todos los contactos.

                DataTable tablaContactos = new DataTable(); // Crea un DataTable para almacenar los datos.

                adaptador = new OleDbDataAdapter(comando);// Adaptador para llenar el DataTable
                adaptador.Fill(tablaContactos); // Llenar el DataTable con datos de la base de datos

                dgvcontactos.DataSource = tablaContactos; // Asignar el DataTable al DataGridView para mostrar los productos.
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AgregarContactos(Contactos nuevoContacto)
        {
            using (OleDbConnection conexion = new OleDbConnection(cadena))
            {
                using (OleDbCommand comando = new OleDbCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandType = CommandType.Text;
                    comando.CommandText = "INSERT INTO Contacto (Nombre, Apellido, Telefono,Correo, Categoria) " +
                               "VALUES (@Nombre, @Apellido, @Telefono, @Correo, @Categoria)";

                    comando.Parameters.AddWithValue("@Nombre", nuevoContacto.Nombre);
                    comando.Parameters.AddWithValue("@Apellido", nuevoContacto.Apellido);
                    comando.Parameters.AddWithValue("@Telefono", nuevoContacto.Telefono);
                    comando.Parameters.AddWithValue("@Correo", nuevoContacto.Correo);
                    comando.Parameters.AddWithValue("@Categoria", nuevoContacto.Categoria);

                    try
                    {
                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Manejar la excepción (por ejemplo, mostrar un mensaje de error al usuario)
                        MessageBox.Show("Error al agregar el contacto: " + ex.Message);
                    }
                }
            }

        }

        public void ModificarContacto(Contactos contacto)
        {
            //Se crea una nueva conexión a la base de datos utilizando la cadena de conexión especificada por cadena.
            //El uso de using garantiza que la conexión se cierre automáticamente después de que el bloque de código haya terminado
            using (OleDbConnection conexion = new OleDbConnection(cadena))
            {

                //Crea un nuevo comando para ejecutar instrucciones SQL en la base de datos
                using (OleDbCommand comando = new OleDbCommand())
                {
                    comando.Connection = conexion;            //Asocia el comando con la conexión a la base de datos
                    comando.CommandType = CommandType.Text;   //Indica que el comando contiene una cadena SQL
                    //Define la instrucción SQL que se va a ejecutar, en este caso,
                    //un comando UPDATE que modifica los campos de un contacto en la tabla Contacto donde el Id coincide con el ID del contacto
                    comando.CommandText      
                         = @"UPDATE Contacto
                                 SET Nombre = @nuevoNombre,
                                     Apellido = @nuevoApellido,
                                     Telefono = @nuevoTelefono,
                                     Correo = @nuevoCorreo,
                                     Categoria = @nuevaCategoria
                                     WHERE Id = @id";

                    //Se agregan los parámetros a la consulta SQL para reemplazar los marcadores de posición (@nuevoNombre, @nuevoApellido, etc.)
                    //con los valores correspondientes del objeto contacto. Esto ayuda a prevenir inyecciones SQL
                    comando.Parameters.AddWithValue("@nuevoNombre", contacto.Nombre);
                    comando.Parameters.AddWithValue("@nuevoApellido", contacto.Apellido);
                    comando.Parameters.AddWithValue("@nuevoTelefono", contacto.Telefono);
                    comando.Parameters.AddWithValue("@nuevoCorreo", contacto.Correo);
                    comando.Parameters.AddWithValue("@nuevaCategoria", contacto.Categoria);
                    comando.Parameters.AddWithValue("@id", contacto.id);




                    try
                    {
                        //Abre la conexión a la base de datos
                        conexion.Open();
                        //Ejecuta el comando SQL. ExecuteNonQuery es adecuado para comandos que no devuelven filas,
                        //como INSERT, UPDATE o DELETE, y devuelve el número de filas afectadas
                        int filasAfectadas = comando.ExecuteNonQuery();

                        //Verifica si se modificaron filas. Si es mayor que cero, significa que al menos un contacto fue actualizado
                        if (filasAfectadas > 0)
                        {
                            //Muestra un mensaje de éxito
                            MessageBox.Show("Contacto modificado correctamente.");
                        }
                        else //Si no se modificaron filas, se muestra un mensaje indicando que no se encontró ningún contacto con el ID 
                        {
                            MessageBox.Show("No se encontró ningún producto con el ID especificado.");
                        }

                    }
                    catch (Exception ex)  //Captura cualquier excepción que ocurra durante la ejecución del bloque try
                    {
                        //Muestra un mensaje con el detalle del error, lo que puede ser útil para la depuración
                        MessageBox.Show("Error al modificar el producto: " + ex.Message);
                    }
                }
            }

        }

        public void EliminarContacto(int id)
        {

            //Se crea una nueva instancia de OleDbConnection, que establece la conexión a la base de datos.
            //La variable cadena contiene la cadena de conexión necesaria para conectarse a la base de datos.
            //La instrucción using garantiza que la conexión se cierre automáticamente una vez que se complete el bloque de código.
            using (OleDbConnection conexion = new OleDbConnection(cadena))
            {
                //Crea un nuevo comando de base de datos OleDbCommand, que se utilizará para ejecutar instrucciones SQL
                using (OleDbCommand comando = new OleDbCommand())
                {
                    //Asigna la conexión creada previamente al comando, lo que le permite ejecutar la instrucción SQL en esa conexión
                    comando.Connection = conexion;
                    //Indica que el comando contendrá una cadena SQL
                    comando.CommandType = CommandType.Text;
                    // Es un comando DELETE que elimina un contacto de la tabla Contacto donde el Id coincide con el parámetro @id
                    comando.CommandText = @"DELETE FROM Contacto WHERE Id = @id";

                    // Agregar el parámetro
                    comando.Parameters.AddWithValue("@id", id);

                    try
                    {

                        //Abre la conexión a la base de datos
                        conexion.Open();
                        //Se utiliza para ejecutar comandos SQL que no devuelven filas (como INSERT,
                        //UPDATE, o DELETE). Devuelve el número de filas afectadas por el comando
                        int filasAfectadas = comando.ExecuteNonQuery();

                        //Muestra un mensaje de éxito
                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Contacto eliminado correctamente.");

                        }  //Si no se eliminaron filas, se muestra un mensaje indicando que no se encontró ningún contacto con el ID 
                        else
                        {
                            MessageBox.Show("No se encontró ningún producto con el ID especificado.");
                        }

                        //Captura cualquier excepción que ocurra durante la ejecución del bloque try
                    }
                    catch (Exception ex)
                    {

                        //Muestra un mensaje con el detalle del error, lo que puede ayudar en la depuración
                        MessageBox.Show("Error al eliminar el producto: " + ex.Message);
                    }
                }
            }
        }
    }
}
