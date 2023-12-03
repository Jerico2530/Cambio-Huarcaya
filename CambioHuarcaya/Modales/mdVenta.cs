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

namespace CambioHuarcaya.Modales
{
    public partial class mdVenta : Form
    {
        public Venta _Venta { get; set; }
        public mdVenta()
        {
            InitializeComponent();
        }

        private void btBusqueda_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cbBusqueda.SelectedItem).Valor.ToString();
            if (dgvDatos.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvDatos.Rows)
                {
                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
            }
        }

        private void btLimpiarbuscador_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
            cbBusqueda.SelectedIndex = 0;
        }

        private void mdVenta_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in dgvDatos.Columns)
            {
                if (column.Visible)
                {
                    cbBusqueda.Items.Add(new OpcionCombo()
                    {
                        Valor = column.Name,
                        Text = column.HeaderText
                    });
                }
            }
            cbBusqueda.DisplayMember = "Text";
            cbBusqueda.ValueMember = "Valor";
            cbBusqueda.SelectedIndex = 0;

            List<Venta> lsC = new CN_Venta().Listar();
            foreach (Venta item in lsC)
            {
                dgvDatos.Rows.Add(new object[]
                {
                    item.IdVenta,
                    item.NumeroDocumento,
                    item.TipoDocumento,
                    item.NombreCliente,
                    item.MontoPago,
                    item.MontoTotal,
                    item.MontoCambio,
                    item.FechaRegistro
                });
            }
        }


        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            int iCol = e.ColumnIndex;
            if (iRow >= 0 && iCol > 0)
            {
                _Venta = new Venta()
                {
                    IdVenta = Convert.ToInt32(dgvDatos.Rows[iRow].Cells["IdVenta"].Value.ToString()),
                    NumeroDocumento = dgvDatos.Rows[iRow].Cells["Codigo"].Value.ToString(),
                    TipoDocumento = dgvDatos.Rows[iRow].Cells["Tipo"].Value.ToString(),
                    MontoTotal = Convert.ToDecimal(dgvDatos.Rows[iRow].Cells["Total"].Value.ToString()),
                    FechaRegistro = dgvDatos.Rows[iRow].Cells["Fecha"].Value.ToString(),
                };
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
