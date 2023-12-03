using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CambioHuarcaya
{
    public partial class frmNegocio : Form
    {
        public frmNegocio()
        {
            InitializeComponent();
        }
        public Image byte2image(byte[] imageByte)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(imageByte, 0, imageByte.Length);
            Image image = new Bitmap(ms);

            return image;
        }
        private void frmNegocio_Load(object sender, EventArgs e)
        {
            int count = 0;
            bool obtenido = true;
            byte[] image = new CN_Negocio().ObtenerLogo(out obtenido);
            if (image.Length > 0)
                picLogo.Image = byte2image(image);
            Negocio oNegocio = new CN_Negocio().ObtenerDatos();
            txtNombre.Text = oNegocio.Nombre;
            txtRFC.Text = oNegocio.RFC;
            txtDireccion.Text = oNegocio.Direccion;
            txtTelefono.Text = oNegocio.Telefono;
            txtCorreo.Text = oNegocio.Correo;
        }

        private void btGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            Negocio oNegocio = new Negocio()
            {
                Nombre = txtNombre.Text,
                RFC = txtRFC.Text,
                Direccion = txtDireccion.Text,
                Telefono = txtTelefono.Text,
                Correo = txtCorreo.Text,
            };
            bool respuesta = new CN_Negocio().GuardarDatos(oNegocio, out mensaje);
            if (respuesta)
                MessageBox.Show("Datos Actualizados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btSubir_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Files| *.jpg;*.jpeg;*.png";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                byte[] byteImage = File.ReadAllBytes(openFile.FileName);
                bool respuesta = new CN_Negocio().ActualizarLogo(byteImage, out mensaje);
                if (respuesta)
                    picLogo.Image = byte2image(byteImage);
                else
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtRFC_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDireccion_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTelefono_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCorreo_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
