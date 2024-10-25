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
    public partial class Modificar_Contacto : Form
    {
        public Modificar_Contacto()
        {
            InitializeComponent();
            ConexionBD conexion = new ConexionBD();
            CargarTreeView();
            conexion.listarContactos(dgvContactos);
        }

        private void Modificar_Contacto_Load(object sender, EventArgs e)
        {

        }

        private void modificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Permite modificar contacto
            Modificar_Contacto frm = new Modificar_Contacto();
            frm.ShowDialog();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //permite eliminar contacto
            Eliminar_Contacto frm = new Eliminar_Contacto();
            frm.ShowDialog();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Contactos ContactoModificado = new Contactos();
            ConexionBD conexion = new ConexionBD();

            // Obtener el ID del producto a modificar (suponiendo que está en txtCodigo)
            int codigo;
            if (!int.TryParse(txtid.Text, out codigo))
            {
                MessageBox.Show("Por favor, ingrese un ID de producto válido.");
                return;
            }

            ContactoModificado.id = codigo;
            ContactoModificado.Nombre = txtNombre.Text;
            ContactoModificado.Apellido = txtApellido.Text;
            ContactoModificado.Telefono = int.Parse(txtTelefono.Text);
            ContactoModificado.Correo = txtCorreo.Text;
            ContactoModificado.Categoria = cmbCategorias.Text;

            try
            {
                conexion.ModificarContacto(ContactoModificado);
                CargarTreeView();
                conexion.listarContactos(dgvContactos);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el Contacto: " + ex.Message);
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



    }
}
