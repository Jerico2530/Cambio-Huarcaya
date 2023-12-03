using CambioHuarcaya.Modales;
using CapaEntidad;
using CapaNegocio;
using FontAwesome.Sharp;
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
    public partial class Inicio : Form
    {
        private static Usuario usuarioActual;
        private static IconMenuItem MenuActivo = null;
        private static Form FormularioActivo = null;
        public Inicio(Usuario objusuario)
        {

            usuarioActual = objusuario;
            InitializeComponent();

        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            Negocio oNegocio = new CN_Negocio().ObtenerDatos();
            lblSistema.Text = oNegocio.Nombre != null ? oNegocio.Nombre : "CASA CAMBIO HUARCAYA";
            List<Permiso> listaPermisos = new CN_Permiso().Listar(usuarioActual.IdUsuario);
            foreach (IconMenuItem iconmenu in menu.Items)
            {
                bool encontrado = listaPermisos.Any(m => m.NombreMenu == iconmenu.Name && m.Estado == true);

                if (!encontrado)
                {
                    iconmenu.Visible = false;
                }
            }

            LblUsuario2.Text = usuarioActual.NombreCompleto;
        }

        private void AbrirFormulario(IconMenuItem menu, Form formulario)
        {
            if (MenuActivo != null)
            {
                MenuActivo.BackColor = Color.White;

            }
            menu.BackColor = Color.Silver;
            MenuActivo = menu;

            if (FormularioActivo != null)
            {
                FormularioActivo.Close();
            }

            FormularioActivo = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            formulario.BackColor = ColorTranslator.FromHtml("#dddddd");

            Contenedor.Controls.Add(formulario);
            formulario.Show();
        }

       

        private void subCategoria_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MENUMANTE, new frmCategoria());
        }

        private void subProducto_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MENUMANTE, new frmMoneda());
        }


        private void subMenuRegistrarV_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MENUVEN, new frmVenta(usuarioActual));
        }

        private void SubManuVer_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MENUVEN, new frmDetalleVenta());
        }

        private void RegistarCom_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MENUCOM, new frmCompra(usuarioActual));
        }

        private void VerCompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MENUCOM, new frmDetalleCompra());
        }

        private void MENICLIENT_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmCliente());
        }

        private void MENUPROVE_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmProveedor());
        }

        private void SubCompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MENURE, new frmReporteCompra());
        }

        private void SubVenta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MENURE, new frmReporteVenta());
        }

        private void MENUACER_Click(object sender, EventArgs e)
        {
            mdInformacion md = new mdInformacion();
            md.ShowDialog();
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("¿Deseas Salie?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MENUUSARIO, new frmUsuario());
        }

        private void permisoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MENUUSARIO, new frmPermiso());
        }

        private void subPerfil_Click(object sender, EventArgs e)
        {
            AbrirFormulario(MENUUSARIO, new frmNegocio());
        }
    }
}
