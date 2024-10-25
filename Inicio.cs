using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace pryAgendaGarcia
{
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {

        }

        private void agregarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Aquí se crea una nueva instancia del formulario Agregar_Contacto
            //ingresar la información de un nuevo contacto
            Agregar_Contacto frm = new Agregar_Contacto();
            frm.ShowDialog();
        }

        private void modificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Permite al usuario cambiar los detalles de un contacto existente.
            Modificar_Contacto frm = new Modificar_Contacto();
            frm.ShowDialog();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Eliminar un contacto
            Eliminar_Contacto frm = new Eliminar_Contacto();
            frm.ShowDialog();
        }
    }
}
