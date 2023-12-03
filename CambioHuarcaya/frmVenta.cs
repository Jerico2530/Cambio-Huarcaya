using CambioHuarcaya.Modales;
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
using System.Windows.Documents;
using System.Windows.Forms;

namespace CambioHuarcaya
{
    public partial class frmVenta : Form
    {
        private Usuario _Usuario;
        public frmVenta(Usuario oUsuario = null)
        {
            _Usuario = oUsuario;
            InitializeComponent();
        }

        private void frmVenta_Load(object sender, EventArgs e)
        {
            CbxDocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Text = "Boleta" });
            CbxDocumento.Items.Add(new OpcionCombo() { Valor = "Factura", Text = "Factura" });
            CbxDocumento.DisplayMember = "Text";
            CbxDocumento.ValueMember = "Valor";
            CbxDocumento.SelectedIndex = 0;

            TxtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            TxtIdMonera.Text = "0";
            TxtTotalPagar.Text = "0.00";

        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            using (var modal = new mdCliente())
            {
                var result = modal.ShowDialog();
                if (result == DialogResult.OK)
                {

                    TxtDocumentoCliente.Text = modal._Cliente.Documento.ToString();
                    TxtDocumentoCliente.BackColor = Color.Honeydew;
                    TxtNombreCompleto.Text = modal._Cliente.NombreCompleto.ToString();
                    TxtCodMonera.Select();

                }
                else
                {
                    TxtDocumentoCliente.BackColor = Color.Red;
                    TxtDocumentoCliente.Select();
                }

            }
        }

        private void BtnBuscarProducto_Click(object sender, EventArgs e)
        {
            using (var modal = new mdMonera())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    TxtIdMonera.Text = modal._Monera.IdMonera.ToString();
                    TxtCodMonera.Text = modal._Monera.Codigo.ToString();
                    TxtCodMonera.BackColor = Color.Honeydew;
                    TxtNombreMonera.Text = modal._Monera.Nombre.ToString();
                    TxtPrecio.Text = modal._Monera.PrecioVenta.ToString("0.00");
                    TxtDeposito.Text = modal._Monera.Stock.ToString();
                    TxtCantidad.Value = 1;
                    TxtPrecio.Select();

                }
                else
                {
                    TxtCodMonera.Select();
                }
            }
        }

        private void TxtCodProducto_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyData == Keys.Enter)
            {
                Monera oProducto = new CN_Monera().Listar().Where(p => p.Codigo == TxtCodMonera.Text && p.Estado == true).FirstOrDefault();

                if (oProducto != null)
                {
                    TxtCodMonera.BackColor = Color.Honeydew;
                    TxtIdMonera.Text = oProducto.IdMonera.ToString();
                    TxtNombreMonera.Text = oProducto.Nombre;
                    TxtPrecio.Text = oProducto.PrecioVenta.ToString();
                    TxtDeposito.Text = oProducto.Stock.ToString();
   
                }
                else
                {
                    TxtCodMonera.BackColor = Color.MistyRose;
                    TxtIdMonera.Text = "0";
                    TxtNombreMonera.Text = "";
                    TxtPrecio.Text = "";
                    TxtDeposito.Text = "";
                    TxtCantidad.Value = 1;
                    TxtNombreMonera.Select();

                }

            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            decimal precioVenta = 0;
            decimal precio = 0;
            bool precio_existe = false;



            if (int.Parse(TxtIdMonera.Text) == 0)
            {
                MessageBox.Show("Debe selecionar un Monera", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }



            if (!decimal.TryParse(TxtPrecio.Text, out precio))
            {
                MessageBox.Show("Precio - Formato moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                TxtPrecio.Select();
                return;
            }

            if (Convert.ToInt32(TxtDeposito.Text) < Convert.ToInt32(TxtCantidad.Value.ToString()))
            {
                MessageBox.Show("La Cantidad no puede ser mayor al stock", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (DgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow fila in DgvData.Rows)
                {
                    if (fila.Cells["IdMonera"].Value.ToString() == TxtIdMonera.Text)
                    {
                        precio_existe = true;
                        break;
                    }
                }
            }

            if (!precio_existe)

            {
                string mensaje = string.Empty;
                bool respuesta = new CN_Venta().RestarStock(Convert.ToInt32(TxtIdMonera.Text), Convert.ToInt32(TxtCantidad.Value));
                if (respuesta)
                {

                    DgvData.Rows.Add(new object[]
                {

                    TxtIdMonera.Text,
                    TxtNombreMonera.Text,
                    precio.ToString("0.00"),
                    TxtCantidad.Value.ToString(),
                    (TxtCantidad.Value*precio).ToString("0.00")
                 });
                    calcularTotal();
                    limpiarProducto();
                    TxtCodMonera.Select();
                }
            }
            else
            {
                MessageBox.Show("Ya existe este producto en esta compra", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            calcularTotal();
            TxtPaga.Text = "";
            TxtCambio.Text = "";
        }
        private void calcularTotal()
        {
            decimal total = 0;
            if (DgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in DgvData.Rows)
                    total += Convert.ToDecimal(row.Cells["SubTotal"].Value.ToString());
            }
            TxtTotalPagar.Text = total.ToString("0.00");
        }
        private void limpiarProducto()
        {

            TxtIdMonera.Text = "0";
            TxtCodMonera.Text = "";
            TxtCodMonera.BackColor = Color.White;
            TxtNombreMonera.Text = "";
            TxtPrecio.Text = "";
            TxtDeposito.Text = "";
            TxtCantidad.Value = 1;
        }

        private void DgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == DgvData.Columns["BtnEliminar"].Index)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                // Establece el FillWeight y Width deseado para la columna que contiene el bote_de_basura.
                int desiredFillWeight = 100;
                int desiredWidth = 100;

                // Ajusta el FillWeight de la columna correspondiente.
                DgvData.Columns["BtnEliminar"].FillWeight = desiredFillWeight;

                // Ajusta el Width de la columna correspondiente.
                DgvData.Columns["BtnEliminar"].Width = desiredWidth;

                var image = Properties.Resources.bote_de_basura; // Carga la imagen desde los recursos.

                var x = e.CellBounds.Left + (e.CellBounds.Width - image.Width) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - image.Height) / 2;

                e.Graphics.DrawImage(image, new Rectangle(x, y, image.Width, image.Height));
                e.Handled = true;
            }
        }

        private void DgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvData.Columns[e.ColumnIndex].Name == "BtnEliminar")
            {
                int index = e.RowIndex;

                if (index >= 0)
                {
                    bool respueta = new CN_Venta().SumarStock(
                        Convert.ToInt32(DgvData.Rows[index].Cells["IdMonera"].Value.ToString()),
                        Convert.ToInt32(DgvData.Rows[index].Cells["Cantidad"].Value.ToString()));

                    if (respueta)
                    {
                        DgvData.Rows.RemoveAt(index);
                        calcularTotal();
                    }
                }
            }
        }

        private void TxtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (TxtPrecio.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void TxtPaga_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (TxtPaga.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }
        private void CalcularCambio()
        {
            if (TxtPaga.Text.Trim() == string.Empty)
            {
                return;
            }
            decimal totalCompra = decimal.TryParse(TxtTotalPagar.Text.ToString(), out totalCompra) ? totalCompra : 0;
            decimal totalPago = decimal.TryParse(TxtPaga.Text.ToString(), out totalPago) ? totalPago : 0;
            decimal cambio = totalPago - totalCompra;
            TxtCambio.Text = cambio.ToString();

        }

        private void TxtPaga_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyData == Keys.Enter)
            {
                CalcularCambio();
            }
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            if (TxtPaga.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Debe ingresar el monto con el que se paga", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return; ;
            }
            if (TxtDocumentoCliente.Text.Trim() == string.Empty)
            {
                TxtDocumentoCliente.Text = "0000";
            }
            if (TxtNombreCompleto.Text.Trim() == string.Empty)
            {
                TxtNombreCompleto.Text = "Ventas Publico";
            }
            if (DgvData.Rows.Count < 1)
            {
                MessageBox.Show("Debe ingresar producto en la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable detalle_venta = new DataTable();

            detalle_venta.Columns.Add("IdMonera", typeof(int));
            detalle_venta.Columns.Add("PrecioVenta", typeof(decimal));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Subtotal", typeof(decimal));

            foreach (DataGridViewRow row in DgvData.Rows)
            {
                detalle_venta.Rows.Add(new object[]
                {
                    row.Cells["IdMonera"].Value.ToString(),
                    row.Cells["Precio"].Value.ToString(),
                    row.Cells["Cantidad"].Value.ToString(),
                    row.Cells["SubTotal"].Value.ToString(),

                });
            }

            int idcorrelativo = new CN_Venta().ObtenerCorrelativo();
            string numeroDocumento = string.Format("{0:00000}", idcorrelativo);
            CalcularCambio();

            Venta oVenta = new Venta()
            {

                oUsuario = new Usuario() { IdUsuario = _Usuario.IdUsuario },
                TipoDocumento = ((OpcionCombo)CbxDocumento.SelectedItem).Text,
                NumeroDocumento = numeroDocumento,
                DocumentoCliente = TxtDocumentoCliente.Text,
                NombreCliente = TxtNombreCompleto.Text,
                MontoPago = Convert.ToDecimal(TxtPaga.Text),
                MontoCambio = Convert.ToDecimal(TxtCambio.Text),
                MontoTotal = Convert.ToDecimal(TxtTotalPagar.Text)

            };

            String mensaje = string.Empty;
            bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out mensaje);

            if (respuesta)
            {
                var result = MessageBox.Show("Numero de venta generada\n" + numeroDocumento + "\n\n¿Desea copiar al portapapeles?", "mensaje",
                  MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                    Clipboard.SetText(numeroDocumento);
                TxtDocumentoCliente.Text = "";
                TxtNombreCompleto.Text = "";
                DgvData.Rows.Clear();
                calcularTotal();
                TxtPaga.Text = "";
                TxtCambio.Text = "";
            }
            else
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }
        
    }
}
