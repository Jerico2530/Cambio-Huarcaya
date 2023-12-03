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
using System.Windows.Forms;

namespace CambioHuarcaya.Modales
{
    public partial class mdCompra : Form
    {
        public Compra _Compra { get; set; }
        public mdCompra()
        {
            InitializeComponent();
        }

        private void mdCompra_Load(object sender, EventArgs e)
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

            List<Compra> lsC = new CN_Compra().Listar();
            foreach (Compra item in lsC)
            {
                dgvDatos.Rows.Add(new object[]
                {
                    item.IdCompra,
                    item.NumeroDocumento,
                    item.TipoDocumento,
                    item.MontoTotal,
                    item.FechaRegistro
                });
            }

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


        private void txtBusqueda_TextChanged(object sender, EventArgs e)
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


        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            int iCol = e.ColumnIndex;
            if (iRow >= 0 && iCol > 0)
            {
                _Compra = new Compra()
                {
                    IdCompra = Convert.ToInt32(dgvDatos.Rows[iRow].Cells["IdCompra"].Value.ToString()),
                    NumeroDocumento = dgvDatos.Rows[iRow].Cells["Codigo"].Value.ToString(),
                    TipoDocumento = dgvDatos.Rows[iRow].Cells["Tipo"].Value.ToString(),
                    MontoTotal = Convert.ToDecimal(dgvDatos.Rows[iRow].Cells["Monto"].Value.ToString()),
                    FechaRegistro = dgvDatos.Rows[iRow].Cells["Fecha"].Value.ToString(),
                };
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
