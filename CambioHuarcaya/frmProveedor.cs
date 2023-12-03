using CambioHuarcaya.Utilizable;
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
using System.Windows.Forms;

namespace CambioHuarcaya
{
    public partial class frmProveedor : Form
    {
        public frmProveedor()
        {
            InitializeComponent();
        }

        private void frmProveedor_Load(object sender, EventArgs e)
        {
            CbxEstado.Items.Add(new OpcionCombo() { Valor = 1, Text = "Activo" });
            CbxEstado.Items.Add(new OpcionCombo() { Valor = 0, Text = "No Activo" });
            CbxEstado.DisplayMember = "Text";
            CbxEstado.ValueMember = "Valor";
            CbxEstado.SelectedIndex = 0;


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

            //MOSTRAR TODOS 
            List<Proveedor> listaProveedor = new CN_Proveedor().Listar();

            foreach (Proveedor item in listaProveedor)
            {
                DgvData.Rows.Add(new object[] { "",item.IdProveedor,item.Documento,item.Banco,item.Correo,item.Telefono,

                    item.Estado ==true ? 1 :0,
                    item.Estado ==true ? "Activo" :"No Activo"


            });
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            string Mensaje = string.Empty;
            Proveedor oProveedor = new Proveedor()
            {
                IdProveedor = Convert.ToInt32(TxtId.Text),
                Documento = TxtDocumento.Text,
                Banco = TxtBanco.Text,
                Correo = TxtCorreo.Text,
                Telefono = TxtTelefono.Text,
                Estado = Convert.ToInt32(((OpcionCombo)CbxEstado.SelectedItem).Valor) == 1 ? true : false
            };
            int IdProveedorgenerado = 0;
            bool respuesta = false;
            if (oProveedor.IdProveedor == 0)
            {
                IdProveedorgenerado = new CN_Proveedor().Registrar(oProveedor, out Mensaje);
                TxtId.Text = IdProveedorgenerado.ToString();
                if (IdProveedorgenerado != 0)
                {
                    DgvData.Rows.Add(new object[]
                    {
                        "",
                        TxtId.Text,
                        TxtDocumento.Text,
                        TxtBanco.Text,
                        TxtCorreo.Text,
                        TxtTelefono.Text,
                        ((OpcionCombo)CbxBuscar.SelectedItem).Valor.ToString(),
                        ((OpcionCombo)CbxBuscar.SelectedItem).Text.ToString()
                    });
                    MessageBox.Show("Proveedor Agregado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }
                else
                    MessageBox.Show(Mensaje, "Mensaje", MessageBoxButtons.OK);
            }
            else
            {
                respuesta = new CN_Proveedor().Editar(oProveedor, out Mensaje);
                if (respuesta)
                {
                    DataGridViewRow row = DgvData.Rows[Convert.ToInt32(TxtIndice.Text)];
                    row.Cells["Id"].Value = TxtId.Text;
                    row.Cells["Documento"].Value = TxtDocumento.Text;
                    row.Cells["Banco"].Value = TxtBanco.Text;
                    row.Cells["Correo"].Value = TxtCorreo.Text;
                    row.Cells["Telefono"].Value = TxtTelefono.Text;
                    row.Cells["EstadoValor"].Value = ((OpcionCombo)CbxEstado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombo)CbxEstado.SelectedItem).Text.ToString();
                    MessageBox.Show("Proveedor con el ID " + TxtId.Text.ToString() + " actualizado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    limpiar();
                }
                else
                    MessageBox.Show(Mensaje, "Mensaje", MessageBoxButtons.OK);
            }
        }
        private void limpiar()
        {
            TxtIndice.Text = "-1";
            TxtId.Text = "0";
            TxtDocumento.Text = "";
            TxtBanco.Text = "";
            TxtCorreo.Text = "";
            TxtTelefono.Text = "";
            CbxBuscar.SelectedIndex = 0;
            TxtDocumento.Select();
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
                    TxtIndice.Text = indice.ToString();
                    TxtId.Text = DgvData.Rows[indice].Cells["Id"].Value.ToString();
                    TxtDocumento.Text = DgvData.Rows[indice].Cells["Documento"].Value.ToString();
                    TxtBanco.Text = DgvData.Rows[indice].Cells["Banco"].Value.ToString();
                    TxtCorreo.Text = DgvData.Rows[indice].Cells["Correo"].Value.ToString();
                    TxtTelefono.Text = DgvData.Rows[indice].Cells["Telefono"].Value.ToString();

                    foreach (OpcionCombo oc in CbxEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(DgvData.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            int indice_combo = CbxEstado.Items.IndexOf(oc);
                            CbxEstado.SelectedIndex = indice_combo;
                            break;
                        }

                    }

                }

            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            string Mensaje = string.Empty;
            if (Convert.ToInt32(TxtId.Text) != 0)
            {
                if (MessageBox.Show(
                    "¿Desea eliminar el Proveedor?",
                    "Mensaje", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                    ) == DialogResult.Yes)
                {
                    Proveedor oProveedor = new Proveedor()
                    {
                        IdProveedor = Convert.ToInt32(TxtId.Text),
                    };
                    bool respuesta = new CN_Proveedor().Eliminar(oProveedor, out Mensaje);
                    if (respuesta)
                    {
                        DgvData.Rows.RemoveAt(Convert.ToInt32(TxtIndice.Text));
                        limpiar();
                    }
                    else
                    {
                        MessageBox.Show(Mensaje, "Mensaje", MessageBoxButtons.OK);
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
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
            }
        }

        private void BtnLimpiarBus_Click(object sender, EventArgs e)
        {
            TxtBuscar.Text = "";
            foreach (DataGridViewRow row in DgvData.Rows)
            {
                row.Visible = true;
            }
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
