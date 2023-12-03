using CambioHuarcaya.Utilizable;
using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace CambioHuarcaya
{
    public partial class frmUsuario : Form
    {
        private Usuario _Usuario;

        public frmUsuario(Usuario oUsuario = null)
        {
            InitializeComponent();
            _Usuario = oUsuario;

        }

        private void frmUsuario_Load(object sender, EventArgs e)
        {
            CbxEstado.Items.Add(new OpcionCombo() { Valor = 1, Text = "Activo" });
            CbxEstado.Items.Add(new OpcionCombo() { Valor = 0, Text = "No Activo" });
            CbxEstado.DisplayMember = "Text";
            CbxEstado.ValueMember = "Valor";
            CbxEstado.SelectedIndex = 0;

            List<Rol> listaRol = new CN_Rol().Listar();

            foreach(Rol item in listaRol)
            {
                CbxRol.Items.Add(new OpcionCombo() { Valor = item.IdRol, Text = item.Descripcion });
            }
            CbxRol.DisplayMember = "Text";
            CbxRol.ValueMember = "Valor";
            CbxRol.SelectedIndex = 0;


            foreach (DataGridViewColumn column in DgvData.Columns)
            {
                if (column.Visible == true && column.Name != "BtnSelecionar")
                {
                    CbxBuscar.Items.Add(new OpcionCombo() { Valor = column.Name, Text = column.HeaderText });
                }
            }
            CbxBuscar.DisplayMember = "Text";
            CbxBuscar.ValueMember = "Valor";
            CbxBuscar.SelectedIndex = 0;

            //MOSTRAR TODOS LOS USUARIO
            List<Usuario> listaUsuario = new CN_Usuario().Listar();

            foreach (Usuario item in listaUsuario)
            {
                DgvData.Rows.Add(new object[] { "",item.IdUsuario,item.Documento,item.NombreCompleto,item.Correo,item.Clave,
                    item.oRol.IdRol,
                    item.oRol.Descripcion,
                    item.Estado ==true ? 1 :0,
                    item.Estado ==true ? "Activo" :"No Activo"


            });
            }
            TxtDocu.Select();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            Usuario oUsuario = new Usuario()
            {
                IdUsuario = Convert.ToInt32(TxtId.Text),
                Documento = TxtDocu.Text,
                NombreCompleto = TxtNom.Text,
                Correo = TxtCorreo.Text,
                Clave = TxtContra.Text,
                oRol = new Rol() { IdRol = Convert.ToInt32(((OpcionCombo)CbxRol.SelectedItem).Valor),
                Descripcion = ((OpcionCombo)CbxRol.SelectedItem).Text},
                Estado = Convert.ToInt32(((OpcionCombo)CbxEstado.SelectedItem).Valor) == 1 ? true : false,

            };
            int IdUsuariogenerado = 0;
            bool respuesta = false;

            if (oUsuario.IdUsuario == 0)
            {
                if (TxtContra.Text != TxtConfCon.Text)
                    MessageBox.Show("Las contraseñas no coinciden", "Mensaje", MessageBoxButtons.OK);
                else
                    IdUsuariogenerado = new CN_Usuario().Registrar(oUsuario, out mensaje);
                if (IdUsuariogenerado != 0)
                {
                    DgvData.Rows.Add(new object[]
                    {
                        "",
                        IdUsuariogenerado.ToString(),
                        TxtDocu.Text,
                        TxtNom.Text,
                        TxtCorreo.Text,
                        TxtContra.Text,
                        ((OpcionCombo)CbxRol.SelectedItem).Valor.ToString(),
                        ((OpcionCombo)CbxRol.SelectedItem).Text.ToString(),
                        ((OpcionCombo)CbxEstado.SelectedItem).Valor.ToString(),
                        ((OpcionCombo)CbxEstado.SelectedItem).Text.ToString()
                    });
                    MessageBox.Show("Usuario Agregado", "Mensaje", MessageBoxButtons.OK);
                    Limpiar();
                }
                else
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK);
            }
            else
            {
                if (TxtContra.Text != TxtConfCon.Text)
                    MessageBox.Show("Las contraseñas no coinciden", "Mensaje", MessageBoxButtons.OK);
                else
                    respuesta = new CN_Usuario().Editar(oUsuario, out mensaje);
                if (respuesta)
                {
                    DataGridViewRow row = DgvData.Rows[Convert.ToInt32(lblIndice.Text)];
                    row.Cells["Id"].Value = TxtId.Text;
                    row.Cells["Documento"].Value = TxtDocu.Text;
                    row.Cells["NombreCompleto"].Value = TxtNom.Text;
                    row.Cells["Correo"].Value = TxtCorreo.Text;
                    row.Cells["Clave"].Value = TxtContra.Text;
                    row.Cells["IdRol"].Value = ((OpcionCombo)CbxRol.SelectedItem).Valor.ToString();
                    row.Cells["Rol"].Value = ((OpcionCombo)CbxRol.SelectedItem).Text.ToString();
                    row.Cells["EstadoValor"].Value = ((OpcionCombo)CbxEstado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombo)CbxEstado.SelectedItem).Text.ToString();
                    MessageBox.Show("Usuario con el ID " + TxtId.Text.ToString() + " actualizado", "Mensaje", MessageBoxButtons.OK);
                    Limpiar();
                }
                else
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK);
            }

        }
        private void Limpiar()
        {
            lblIndice.Text = "-1";
            TxtId.Text = "0";
            TxtDocu.Text = "";
            TxtNom.Text = "";
            TxtCorreo.Text = "";
            TxtContra.Text = "";
            TxtConfCon.Text = "";
            CbxRol.SelectedIndex = 0;
            CbxEstado.SelectedIndex = 0;
            TxtDocu.Select();

        }

        private void DgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                // Establece el FillWeight y Width deseado para la columna que contiene el check.
                int desiredFillWeight = 100;
                int desiredWidth = 100;

                // Ajusta el FillWeight de la columna correspondiente.
                DgvData.Columns[e.ColumnIndex].FillWeight = desiredFillWeight;

                // Ajusta el Width de la columna correspondiente.
                DgvData.Columns[e.ColumnIndex].Width = desiredWidth;

                var image = Properties.Resources.check; // Carga la imagen desde los recursos.

                var x = e.CellBounds.Left + (e.CellBounds.Width - image.Width) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - image.Height) / 2;

                e.Graphics.DrawImage(image, new Rectangle(x, y, image.Width, image.Height));
                e.Handled = true;
            }
        }

        private void DgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvData.Columns[e.ColumnIndex].Name == "BtnSelecionar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    lblIndice.Text = indice.ToString();
                    TxtId.Text = DgvData.Rows[indice].Cells["Id"].Value.ToString();
                    TxtDocu.Text = DgvData.Rows[indice].Cells["Documento"].Value.ToString();
                    TxtNom.Text = DgvData.Rows[indice].Cells["NombreCompleto"].Value.ToString();
                    TxtCorreo.Text = DgvData.Rows[indice].Cells["Correo"].Value.ToString();
                    TxtContra.Text = DgvData.Rows[indice].Cells["Clave"].Value.ToString();
                    TxtConfCon.Text = DgvData.Rows[indice].Cells["Clave"].Value.ToString();

                    foreach (OpcionCombo oc in CbxRol.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(DgvData.Rows[indice].Cells["IdRol"].Value.ToString()))
                        {
                            int indiceCombo = CbxRol.Items.IndexOf(oc);
                            CbxRol.SelectedIndex = indiceCombo;
                            break;
                        }
                    }
                    foreach (OpcionCombo oc in CbxEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(DgvData.Rows[indice].Cells["EstadoValor"].Value.ToString()))
                        {
                            int indiceCombo = CbxEstado.Items.IndexOf(oc);
                            CbxEstado.SelectedIndex = indiceCombo;
                            break;
                        }

                    }

                }

            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            string Mensaje = string.Empty;
            if (Convert.ToInt32(TxtId.Text) != 0)
            {
                if (MessageBox.Show("¿Deseas Eliminar el Usuario?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    Usuario objusuario = new Usuario()
                    {
                        IdUsuario = Convert.ToInt32(TxtId.Text),
                    };
                    bool respuesta = new CN_Usuario().Eliminar(objusuario, out Mensaje);
                    if (respuesta)
                    {
                        DgvData.Rows.RemoveAt(Convert.ToInt32(lblIndice.Text));
                        Limpiar();

                    }
                    else
                    {
                        MessageBox.Show(Mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
  
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)CbxBuscar.SelectedItem).Valor.ToString();

            if (DgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in DgvData.Rows)
                {
                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(TxtBuscar.Text.Trim().ToUpper()))
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
            }
        }

        private void BtnLimpiarBus_Click(object sender, EventArgs e)
        {
            TxtBuscar.Text = "";
           
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)CbxBuscar.SelectedItem).Valor.ToString();
            if (DgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in DgvData.Rows)
                {
                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(TxtBuscar.Text.Trim().ToUpper()))
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
            }
        }
    }
}
