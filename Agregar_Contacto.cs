using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pryAgendaGarcia
{
    public partial class Agregar_Contacto : Form
    {
        public Agregar_Contacto()
        {
            // Llama a un método que inicializa los componentes de la interfaz de usuario,
            // configurando controles como botones, cuadros de texto, el DataGridView, etc.
            InitializeComponent();
            //Crea una nueva instancia de la clase ConexionBD, que se utiliza para interactuar con la base de datos
            ConexionBD conexion = new ConexionBD();
            //Llama a un método que carga los contactos en un control TreeView, organizándolos por categorías
            CargarTreeView();
            //Llama a un método para llenar dgvContactos con la lista de contactos desde la base de datos
            conexion.listarContactos(dgvContactos);
        }

        private void Agregar_Contacto_Load(object sender, EventArgs e)
        {

        }

        //Estos diccionarios se utilizan para almacenar nodos de TreeView por su ID y por su categoría,
        //facilitando el acceso y la gestión de los nodos.
        Dictionary<int, TreeNode> nodosPorId = new Dictionary<int, TreeNode>();
        Dictionary<string, TreeNode> nodosPorCategoria = new Dictionary<string, TreeNode>();

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Contactos nuevoContacto = new Contactos();
            ConexionBD conexion = new ConexionBD();

            nuevoContacto.id = int.Parse(txtid.Text);
            nuevoContacto.Nombre = txtNombre.Text;
            nuevoContacto.Apellido = txtApellido.Text;
            nuevoContacto.Telefono = int.Parse(txtTelefono.Text);
            nuevoContacto.Correo = txtCorreo.Text;
            nuevoContacto.Categoria = cmbCategorias.Text;

            try
            {
                // Llama al método para volver a llenar el TreeView después de agregar el contacto
                CargarTreeView();
                //Llama a un método para agregar el contacto a la base de datos
                conexion.AgregarContactos(nuevoContacto);
                //Vuelve a llenar el DataGridView con la lista actualizada de contactos
                conexion.listarContactos(dgvContactos);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el contacto: " + ex.Message);
            }
        }
        public void CargarTreeView()
        {
            ConexionBD conexion = new ConexionBD();

            // Limpiar el TreeView antes de volver a llenarlo
            treeView.Nodes.Clear();

            // Obtener los datos de los contactos desde la base de datos
            DataTable tablaContactos = conexion.ObtenerContactosPorCategoria();

            // Diccionario para almacenar los nodos de las categorías
            Dictionary<string, TreeNode> nodosPorCategoria = new Dictionary<string, TreeNode>();

            // Crear los nodos del TreeView
            foreach (DataRow row in tablaContactos.Rows)
            {
                string categoria = row["Categoria"].ToString();
                string nombre = row["Nombre"].ToString();
                string apellido = row["Apellido"].ToString();

                // Obtener o crear el nodo de la categoría
                TreeNode nodoCategoria;
                if (!nodosPorCategoria.TryGetValue(categoria, out nodoCategoria))
                {
                    nodoCategoria = new TreeNode(categoria);
                    treeView.Nodes.Add(nodoCategoria);
                    nodosPorCategoria[categoria] = nodoCategoria;
                }

                // Crear el nodo del contacto y agregarlo al nodo de la categoría
                TreeNode nodoContacto = new TreeNode($"{nombre}, {apellido}");
                nodoCategoria.Nodes.Add(nodoContacto);
            }

            // Expandir todos los nodos del TreeView (opcional)
            treeView.ExpandAll();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Abre un formulario para eliminar un contacto
            Eliminar_Contacto frm = new Eliminar_Contacto();
            frm.ShowDialog();
        }

        private void modificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Abre un formulario para modificar un contacto existente
            Modificar_Contacto frm = new Modificar_Contacto();
            frm.ShowDialog();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
