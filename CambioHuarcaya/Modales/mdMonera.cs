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
    public partial class mdMonera : Form
    {
        public Monera _Monera { get; set; }
        public mdMonera()
        {
            InitializeComponent();
        }

        private void mdMonera_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in DgvData.Columns)
            {
                if (column.Visible == true)
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

                    item.IdMonera,
                    item.Codigo,
                    item.Nombre,
                    item.oCategoria.Pais,
                    item.Stock,
                    item.PrecioCompra,
                    item.PrecioVenta
            });
            }
        }

        private void DgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            int iColum = e.ColumnIndex;

            if (iRow >= 0 && iColum > 0)
            {
                _Monera = new Monera()
                {
                    IdMonera = Convert.ToInt32(DgvData.Rows[iRow].Cells["Id"].Value.ToString()),
                    Codigo = DgvData.Rows[iRow].Cells["Codigo"].Value.ToString(),
                    Nombre = DgvData.Rows[iRow].Cells["Nombre"].Value.ToString(),
                    Stock = Convert.ToInt32(DgvData.Rows[iRow].Cells["Stock"].Value.ToString()),
                    PrecioCompra = Convert.ToDecimal(DgvData.Rows[iRow].Cells["PrecioCompra"].Value.ToString()),
                    PrecioVenta = Convert.ToDecimal(DgvData.Rows[iRow].Cells["PrecioVenta"].Value.ToString()),

                };

                this.DialogResult = DialogResult.OK;
                this.Close();
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
