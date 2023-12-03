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
    public partial class frmCategoria : Form
    {
        public frmCategoria()
        {
            InitializeComponent();
        }

        private void frmCategoria_Load(object sender, EventArgs e)
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
            List<Categoria> lista = new CN_Categoria().Listar();

            foreach (Categoria item in lista)
            {
                DgvData.Rows.Add(new object[] { "",
                    item.IdCategoria,
                    item.Pais,
                    item.Estado ==true ? 1 :0,
                    item.Estado ==true ? "Activo" :"No Activo"


            });
            }
            TxtPais.Select();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;


            Categoria obj = new Categoria()
            {
                IdCategoria = Convert.ToInt32(TxtId.Text),
                Pais = TxtPais.Text,
                Estado = Convert.ToInt32(((OpcionCombo)CbxEstado.SelectedItem).Valor) == 1 ? true : false,

            };
            int IdCategoriagenerado = 0;
            bool respuesta = false;
            if (obj.IdCategoria == 0)
            {
                IdCategoriagenerado = new CN_Categoria().Registrar(obj, out mensaje);
                if (IdCategoriagenerado != 0)
                {
                    DgvData.Rows.Add(new object[]
                    {
                        "",
                        TxtId.Text,
                        TxtPais.Text,
                        ((OpcionCombo)CbxEstado.SelectedItem).Valor.ToString(),
                        ((OpcionCombo)CbxEstado.SelectedItem).Text.ToString()
                    });
                    MessageBox.Show("Categoría agregada", "Mensaje", MessageBoxButtons.OK);
                    Limpiar();
                }
                else
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK);
            }
            else
            {
                respuesta = new CN_Categoria().Editar(obj, out mensaje);
                if (respuesta)
                {
                    DataGridViewRow row = DgvData.Rows[Convert.ToInt32(lblIndice.Text)];
                    row.Cells["Id"].Value = TxtId.Text;
                    row.Cells["Pais"].Value = TxtPais.Text;
                    row.Cells["EstadoValor"].Value = ((OpcionCombo)CbxEstado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombo)CbxEstado.SelectedItem).Text.ToString();
                    MessageBox.Show("Categoria con el ID " + TxtId.Text.ToString() + " actualizada", "Mensaje", MessageBoxButtons.OK);
                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK);
                }
                    
            }
        }
        private void Limpiar()
        {
            lblIndice.Text = "-1";
            TxtId.Text = "0";
            TxtPais.Text = "";
            CbxEstado.SelectedIndex = 0;

            TxtPais.Select();

        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(TxtId.Text) != 0)
            {
                if (MessageBox.Show("¿Deseas Eliminar la Categoria?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    Categoria obj = new Categoria()
                    {
                        IdCategoria = Convert.ToInt32(TxtId.Text),
                    };
                    bool respuesta = new CN_Categoria().Eliminar(obj, out mensaje);
                    if (respuesta)
                    {
                        DgvData.Rows.RemoveAt(Convert.ToInt32(lblIndice.Text));
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                Limpiar();
            }
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
                    TxtPais.Text = DgvData.Rows[indice].Cells["Pais"].Value.ToString();


                    foreach (OpcionCombo oc in CbxEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(DgvData.Rows[indice].Cells["EstadoValor"].Value.ToString()))
                        {
                            int indice_combo = CbxEstado.Items.IndexOf(oc);
                            CbxEstado.SelectedIndex = indice_combo;
                            break;
                        }

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
            foreach (DataGridViewRow row in DgvData.Rows)
            {
                row.Visible = true;
            }
        }
    }
   
}
