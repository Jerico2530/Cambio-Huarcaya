using CapaDatos;
using CapaNegocio;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CambioHuarcaya
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_ingresar_Click(object sender, EventArgs e)
        {
            Usuario ousuario = new CN_Usuario().Listar().Where(u => u.Documento == TxtDocumento.Text && u.Clave == TxtContraseña.Text).FirstOrDefault();

            if (ousuario != null)
            {
                Inicio form=new Inicio(ousuario);

                form.Show();
                this.Hide();

                form.FormClosing += frm_closing;

            }
            else
            {
                MessageBox.Show("No se encuentra el usuario", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
        private void frm_closing(object sender, FormClosingEventArgs e)
        {
            TxtDocumento.Text = "";
            TxtContraseña.Text = "";
            this.Show();
        }
    }
}
