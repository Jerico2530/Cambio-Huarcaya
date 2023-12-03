using CambioHuarcaya.Utilizable;
using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;
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
    public partial class frmMoneda : Form
    {
        public frmMoneda()
        {
            InitializeComponent();
        }

        private void frmMoneda_Load(object sender, EventArgs e)
        {

            CbxEstado.Items.Add(new OpcionCombo() { Valor = 1, Text = "Activo" });
            CbxEstado.Items.Add(new OpcionCombo() { Valor = 0, Text = "No Activo" });
            CbxEstado.DisplayMember = "Text";
            CbxEstado.ValueMember = "Valor";
            CbxEstado.SelectedIndex = 0;

            List<Categoria> listaCategoria = new CN_Categoria().Listar();

            foreach (Categoria item in listaCategoria)
            {
                CbxCategoria.Items.Add(new OpcionCombo() { Valor = item.IdCategoria, Text = item.Pais });
            }
            CbxCategoria.DisplayMember = "Text";
            CbxCategoria.ValueMember = "Valor";
            CbxCategoria.SelectedIndex = 0;


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
            List<Monera> listaMonera = new CN_Monera().Listar();

            foreach (Monera item in listaMonera)
            {
                DgvData.Rows.Add(new object[] {
                    "",
                    item.IdMonera,
                    item.Codigo,
                    item.Nombre,
                    item.Descripcion,
                    item.oCategoria.IdCategoria,
                    item.oCategoria.Pais,
                    item.Stock,
                    item.PrecioCompra,
                    item.PrecioVenta,
                    item.Estado ==true ? 1 :0,
                    item.Estado ==true ? "Activo" :"No Activo"


            });
            }
            TxtCodigo.Select();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            Monera objMonera = new Monera()
            {
                IdMonera = Convert.ToInt32(TxtId.Text),
                Codigo = TxtCodigo.Text,
                Nombre = TxtNombre.Text,
                Descripcion = TxtDescripcion.Text,
                oCategoria = new Categoria() { IdCategoria = Convert.ToInt32(((OpcionCombo)CbxCategoria.SelectedItem).Valor), Pais = ((OpcionCombo)CbxCategoria.SelectedItem).Text },
                Estado = Convert.ToInt32(((OpcionCombo)CbxEstado.SelectedItem).Valor) == 1 ? true : false,
            };

            int IdMoneragenerado = 0;
            bool respuesta = false;
            if (objMonera.IdMonera == 0)
            {
                IdMoneragenerado = new CN_Monera().Registrar(objMonera, out mensaje);
                if (IdMoneragenerado != 0)
                {
                    DgvData.Rows.Add(new object[]
                    {
                        "",
                        TxtId.Text,
                        TxtCodigo.Text,
                        TxtNombre.Text,
                        TxtDescripcion.Text,
                        ((OpcionCombo)CbxCategoria.SelectedItem).Valor.ToString(),
                        ((OpcionCombo)CbxCategoria.SelectedItem).Text.ToString(),
                        "0",
                        "0.00",
                        "0.00",
                        ((OpcionCombo)CbxEstado.SelectedItem).Valor.ToString(),
                        ((OpcionCombo)CbxEstado.SelectedItem).Text.ToString()
                    });
                    MessageBox.Show("Monera agregado", "Mensaje", MessageBoxButtons.OK);
                    Limpiar();
                }
                else
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK);
            }
            else
            {
                respuesta = new CN_Monera().Editar(objMonera, out mensaje);
                if (respuesta)
                {
                    DataGridViewRow row = DgvData.Rows[Convert.ToInt32(TxtIndice.Text)];
                    row.Cells["Id"].Value = TxtId.Text;
                    row.Cells["Codigo"].Value = TxtCodigo.Text;
                    row.Cells["Nombre"].Value = TxtNombre.Text;
                    row.Cells["Descripcion"].Value = TxtDescripcion.Text;
                    row.Cells["IdCategoria"].Value = ((OpcionCombo)CbxCategoria.SelectedItem).Valor.ToString();
                    row.Cells["Descripcion"].Value = ((OpcionCombo)CbxCategoria.SelectedItem).Text.ToString();
                    row.Cells["EstadoValor"].Value = ((OpcionCombo)CbxEstado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombo)CbxEstado.SelectedItem).Text.ToString();
                    MessageBox.Show("Monera con el ID " + TxtId.Text.ToString() + " actualizado", "Mensaje", MessageBoxButtons.OK);
                    Limpiar();
                }
                else
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK);
            }
            
        }
        private void Limpiar()
        {
            TxtIndice.Text = "-1";
            TxtId.Text = "0";
            TxtCodigo.Text = "";
            TxtNombre.Text = "";
            TxtDescripcion.Text = "";
            CbxCategoria.SelectedIndex = 0;
            CbxEstado.SelectedIndex = 0;
            TxtCodigo.Select();


        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(TxtId.Text) != 0)
            {
                if (MessageBox.Show("¿Deseas Eliminar el Monera?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    Monera objMonera = new Monera()
                    {
                        IdMonera = Convert.ToInt32(TxtId.Text),
                    };
                    bool respuesta = new CN_Monera().Eliminar(objMonera, out mensaje);
                    if (respuesta)
                    {
                        DgvData.Rows.RemoveAt(Convert.ToInt32(TxtIndice.Text));

                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    Limpiar();
                }
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
                    TxtIndice.Text = indice.ToString();
                    TxtId.Text = DgvData.Rows[indice].Cells["Id"].Value.ToString();
                    TxtCodigo.Text = DgvData.Rows[indice].Cells["Codigo"].Value.ToString();
                    TxtNombre.Text = DgvData.Rows[indice].Cells["Nombre"].Value.ToString();
                    TxtDescripcion.Text = DgvData.Rows[indice].Cells["Descripcion"].Value.ToString();


                    foreach (OpcionCombo oc in CbxCategoria.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(DgvData.Rows[indice].Cells["IdCategoria"].Value))
                        {
                            int indice_combo = CbxCategoria.Items.IndexOf(oc);
                            CbxCategoria.SelectedIndex = indice_combo;
                            break;
                        }
                    }
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

        private void BtnExcel_Click(object sender, EventArgs e)
        {

            if (DgvData.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos por Exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                DataTable dt = new DataTable();

                foreach (DataGridViewColumn column in DgvData.Columns)
                {
                    if (column.HeaderText != "" && column.Visible)
                        dt.Columns.Add(column.HeaderText, typeof(string));
                }

                foreach (DataGridViewRow row in DgvData.Rows)
                {
                    if (row.Visible)
                        dt.Rows.Add(new object[]
                        {
                            row.Cells[2].Value.ToString(),
                            row.Cells[3].Value.ToString(),
                            row.Cells[4].Value.ToString(),
                            row.Cells[6].Value.ToString(),
                            row.Cells[7].Value.ToString(),
                            row.Cells[8].Value.ToString(),
                            row.Cells[9].Value.ToString(),
                            row.Cells[11].Value.ToString(),

                        });
                }

                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = "ReporteProducto_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss").Replace("_", "-");
                savefile.Filter = "Excel Files | *.xlsx";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        var hoja = wb.Worksheets.Add(dt, "Informe");
                        hoja.ColumnsUsed().AdjustToContents();
                        wb.SaveAs(savefile.FileName);

                        MessageBox.Show("Reporte Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {

                        MessageBox.Show("Error en Generar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

            }
        }
    }
}
