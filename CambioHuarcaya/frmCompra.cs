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
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace CambioHuarcaya
{
    public partial class frmCompra : Form
    {
        private Usuario _Usuario;
        public frmCompra(Usuario oUsuario = null)
        {
            _Usuario = oUsuario;
            InitializeComponent();
        }

        private void frmCompra_Load(object sender, EventArgs e)
        {
            CbxDocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Text = "Boleta" });
            CbxDocumento.Items.Add(new OpcionCombo() { Valor = "Factura", Text = "Factura" });
            CbxDocumento.DisplayMember = "Text";
            CbxDocumento.ValueMember = "Valor";
            CbxDocumento.SelectedIndex = 0;

            TxtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

            TxtIndiceProveedor.Text = "0";

            TxtIndiceP.Text = "0";
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            using (var modal = new mdProveedor())
            {
                var result = modal.ShowDialog();
                if (result == DialogResult.OK)
                {
                    TxtIndiceProveedor.Text = modal._Proveedor.IdProveedor.ToString();
                    TxtDocumentoProveedor.Text = modal._Proveedor.Documento;
                    TxtRazonSocial.Text = modal._Proveedor.Banco;
                    TxtCodProducto.Select();

                }
                else
                {
                    TxtDocumentoProveedor.Select();
                }

            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {

            using (var modal = new mdMonera())
            {
                var result = modal.ShowDialog();
                if (result == DialogResult.OK)
                {
                    TxtIndiceP.Text = modal._Monera.IdMonera.ToString();
                    TxtCodProducto.Text = modal._Monera.Codigo;
                    TxtMonera.Text = modal._Monera.Nombre;
                    TxtPrecioCompra.Select();

                }
                else
                {
                    TxtCodProducto.Select();
                }
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            decimal precioCompra = 0;
            decimal precioVenta = 0;
            decimal total = 0;
            bool producto_existe = false;
            

            if (!decimal.TryParse(TxtPrecioCompra.Text, out precioCompra))
            {
                MessageBox.Show("Precio compra formato incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                TxtPrecioCompra.Select();
                return;
            }
            if (!decimal.TryParse(TxtPrecioVenta.Text, out precioVenta))
            {
                MessageBox.Show("Precio venta formato incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                TxtPrecioCompra.Select();
                return;
            }
            if (DgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in DgvData.Rows)
                {
                    if (row.Cells["IdMonera"].Value.ToString() == TxtIndiceP.Text.ToString())
                    {
                        producto_existe = true;
                        break;
                    }
                }
            }
            if (!producto_existe)
            {
                DgvData.Rows.Add(new object[]
                {
                    TxtIndiceP.Text,
                    TxtMonera.Text,
                    precioCompra.ToString("0.00"),
                    precioVenta.ToString("0.00"),
                    TxtCantidad.Value.ToString(),
                    (TxtCantidad.Value * precioCompra).ToString("0.00")
                });
                limpiarProducto();
            }
            else
            {
                MessageBox.Show("Ya existe este Monera en esta compra", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            calcularTotal();

        }
        private void limpiarProducto()
        {
            TxtIndiceP.Text = "0";
            TxtMonera.Text = "";
            TxtCodProducto.BackColor = Color.White;
            TxtCodProducto.Text = "";
            TxtPrecioCompra.Text = "";
            TxtPrecioVenta.Text = "";
            TxtDocumentoCliente.Text = "";
            TxtNombreCliente.Text = "";
            TxtDocumentoProveedor.Text = "";
            TxtRazonSocial.Text = "";
            TxtCantidad.Value = 0;
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

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {


            if (DgvData.Rows.Count < 1)
            {
                MessageBox.Show("Debe seleccionar al menos un Monera", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            DataTable detalle_compra = new DataTable();
            detalle_compra.Columns.Add("IdMonera", typeof(int));
            detalle_compra.Columns.Add("PrecioCompra", typeof(Decimal));
            detalle_compra.Columns.Add("PrecioVenta", typeof(Decimal));
            detalle_compra.Columns.Add("Cantidad", typeof(int));
            detalle_compra.Columns.Add("MontoTotal", typeof(Decimal));

            foreach (DataGridViewRow row in DgvData.Rows)
            {
                detalle_compra.Rows.Add(
                    new object[]
                {
                    
                     Convert.ToInt32(row.Cells["IdMonera"].Value),
        Convert.ToDecimal(row.Cells["PrecioCompra"].Value),
        Convert.ToDecimal(row.Cells["PrecioVenta"].Value),
        Convert.ToInt32(row.Cells["Cantidad"].Value),
        Convert.ToDecimal(row.Cells["SubTotal"].Value)

                });
            }
            int IdCorrelativo = new CN_Compra().ObtenerCorrelativo();
            string numerodocumento = string.Format("{0:00000}", IdCorrelativo);

            Compra oCompra = new Compra()
            {   
                oUsuario = new Usuario() { IdUsuario = _Usuario.IdUsuario },
                TipoDocumento = ((OpcionCombo)CbxDocumento.SelectedItem).Text,
                NumeroDocumento = numerodocumento,
                MontoTotal = Convert.ToDecimal(TxtTotalPagar.Text)

            };
            if (Convert.ToInt32(TxtIndiceProveedor.Text) != 0)
            {
                oCompra.oProveedor = new Proveedor() { IdProveedor = Convert.ToInt32(TxtIndiceProveedor.Text) };
            }

            else  if(Convert.ToInt32(TxtIndiceCliente.Text) != 0)
            {
                oCompra.oCliente = new Cliente() { IdCliente = Convert.ToInt32(TxtIndiceCliente.Text) };
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Compra().Registrar(oCompra, detalle_compra, out mensaje);
            if (respuesta)
            {
                var result =
                    MessageBox.Show("Número de compra generada:\n" + numerodocumento + "\n\n¿Desea copiar al portapapeles?",
                    "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                    Clipboard.SetText(numerodocumento);
                TxtIndiceProveedor.Text = "0";
                TxtDocumentoProveedor.Text = "";
                TxtRazonSocial.Text = "";
                TxtDocumentoCliente.Text = "";
                TxtNombreCliente.Text = "";
                TxtDocumentoCliente.BackColor=Color.Blue; 
                TxtDocumentoProveedor.BackColor = Color.White;
                DgvData.Rows.Clear();
                calcularTotal();
                limpiarProducto();
            }
            else
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void TxtCodProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Monera oMonera = new CN_Monera().Listar().Where(p => p.Codigo == TxtCodProducto.Text && p.Estado == true).FirstOrDefault();
                if (oMonera != null)
                {
                    TxtCodProducto.BackColor = Color.Honeydew;
                    TxtIndiceP.Text = oMonera.IdMonera.ToString();
                    TxtMonera.Text = oMonera.Nombre.ToString();
                    TxtPrecioCompra.Text = oMonera.PrecioCompra.ToString();
                    TxtPrecioVenta.Text = oMonera.PrecioVenta.ToString();
                    TxtCantidad.Value = Convert.ToInt32(oMonera.Stock.ToString());
                    TxtPrecioCompra.Select();
                }
                else
                {
                    TxtCodProducto.BackColor = Color.MistyRose;
                    TxtIndiceP.Text = "0";
                    TxtMonera.Text = "";
                    TxtCodProducto.Text = "";
                    TxtPrecioCompra.Text = "0.00";
                    TxtCantidad.Text = "0.00";
                    TxtCantidad.Value = 0;
                    TxtPrecioCompra.Select();
                }
            }
        }

        private void TxtDocumentoProveedor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Proveedor oProveedor = new CN_Proveedor().Listar().Where(p => p.Documento == TxtDocumentoProveedor.Text && p.Estado == true).FirstOrDefault();
                if (oProveedor != null)
                {
                    TxtDocumentoProveedor.BackColor = Color.Honeydew;
                    TxtIndiceProveedor.Text = oProveedor.IdProveedor.ToString();
                    TxtDocumentoProveedor.Text = oProveedor.Documento.ToString();
                    TxtRazonSocial.Text = oProveedor.Banco.ToString();
                    TxtCodProducto.Select();
                }
                else
                {
                    TxtDocumentoProveedor.BackColor = Color.MistyRose;
                    TxtIndiceProveedor.Text = "0";
                    TxtDocumentoProveedor.Text = "";
                    TxtRazonSocial.Text = "";
                    TxtDocumentoProveedor.Select();
                }
            }
        }

        private void DgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 6)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                // Establece el FillWeight y Width deseado para la columna que contiene el bote_de_basura.
                int desiredFillWeight = 100;
                int desiredWidth = 100;

                // Ajusta el FillWeight de la columna correspondiente.
                DgvData.Columns[e.ColumnIndex].FillWeight = desiredFillWeight;

                // Ajusta el Width de la columna correspondiente.
                DgvData.Columns[e.ColumnIndex].Width = desiredWidth;

                var image = Properties.Resources.bote_de_basura; // Carga la imagen desde los recursos.

                var x = e.CellBounds.Left + (e.CellBounds.Width - image.Width) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - image.Height) / 2;

                e.Graphics.DrawImage(image, new Rectangle(x, y, image.Width, image.Height));
                e.Handled = true;
            }
        }

        private void DgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DgvData.Columns[e.ColumnIndex].Name == "Vacio")
            {
                int indice = e.RowIndex;
                if (indice >= 0)
                {
                    DgvData.Rows.RemoveAt(indice);
                    calcularTotal();
                }
            }
        }

        private void TxtPrecioCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (TxtPrecioCompra.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
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

        private void TxtPrecioVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (TxtPrecioVenta.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
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

        private void BtnBuscarCliente_Click(object sender, EventArgs e)
        {
            using (var modal = new mdCliente())
            {
                var result = modal.ShowDialog();
                if (result == DialogResult.OK)
                {
                    TxtIndiceCliente.Text = modal._Cliente.IdCliente.ToString();
                    TxtDocumentoCliente.Text = modal._Cliente.Documento;
                    TxtNombreCliente.Text = modal._Cliente.NombreCompleto;
                    TxtCodProducto.Select();

                }
                else
                {
                    TxtDocumentoProveedor.Select();
                }

            }
        }

        private void TxtDocumentoCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Cliente oCliente = new CN_Cliente().Listar().Where(p => p.Documento == TxtDocumentoCliente.Text && p.Estado == true).FirstOrDefault();
                if (oCliente != null)
                {
                    TxtDocumentoCliente.BackColor = Color.Honeydew;
                    TxtIndiceCliente.Text = oCliente.IdCliente.ToString();
                    TxtDocumentoCliente.Text = oCliente.Documento.ToString();
                    TxtNombreCliente.Text = oCliente.NombreCompleto.ToString();
                    TxtCodProducto.Select();
                }
                else
                {
                    TxtDocumentoCliente.BackColor = Color.MistyRose;
                    TxtIndiceCliente.Text = "0";
                    TxtDocumentoCliente.Text = "";
                    TxtNombreCliente.Text = "";
                    TxtCodProducto.Select();
                }
            }
        }
    }
}
