using CambioHuarcaya.Modales;
using CapaEntidad;
using CapaNegocio;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
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
    public partial class frmDetalleVenta : Form
    {

        public frmDetalleVenta()
        {
            InitializeComponent();
        }

        private void BtnLimpiarBus_Click(object sender, EventArgs e)
        {
            TxtFecha.Text = "";
            TxtTipoDocumento.Text = "";
            TxtUsuario.Text = "";
            TxtNumeroDocumento.Text = "";
            TxtNombreCliente.Text = "";

            DgvData.Rows.Clear();
            TxtTotalPagar.Text = "0.00";
            TxtPaga.Text = "0.00";
            TxtCambio.Text = "0.00";
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            using (var modal = new mdVenta())
            {
                var result = modal.ShowDialog();
                if (result == DialogResult.OK)
                {
                    TxtBuscar.Text = modal._Venta.NumeroDocumento.ToString();
                    TxtBuscar.BackColor = Color.Honeydew;
                    Venta oVenta = new CN_Venta().ObtenerVenta(TxtBuscar.Text);

                    if (oVenta.IdVenta != 0)
                    {
                        TxtIndiceCliente.Text = oVenta.NumeroDocumento;

                        TxtFecha.Text = oVenta.FechaRegistro;
                        TxtTipoDocumento.Text = oVenta.TipoDocumento;
                        TxtUsuario.Text = oVenta.oUsuario.NombreCompleto;

                        TxtNumeroDocumento.Text = oVenta.DocumentoCliente;
                        TxtNombreCliente.Text = oVenta.NombreCliente;

                        DgvData.Rows.Clear();
                        foreach (Detalle_Venta dv in oVenta.oDetalleVenta)
                        {
                            DgvData.Rows.Add(new object[] { dv.oMonera.Nombre, dv.PrecioVenta, dv.Cantidad, dv.SubTotal });
                        }

                        TxtTotalPagar.Text = oVenta.MontoTotal.ToString("0.00");
                        TxtPaga.Text = oVenta.MontoPago.ToString("0.00");
                        TxtCambio.Text = oVenta.MontoCambio.ToString("0.00");

                    }
                }
                else
                {
                    TxtBuscar.BackColor = Color.MistyRose;
                    TxtBuscar.Select();

                }
            }
        }  

        private void BtnPDF_Click(object sender, EventArgs e)
        {
            if (TxtTipoDocumento.Text == "")
            {
                MessageBox.Show("No se encontraron resultado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string Text_Html = Properties.Resources.PlantillaVenta.ToString();
            Negocio oDatos = new CN_Negocio().ObtenerDatos();

            Text_Html = Text_Html.Replace("@nombrenegocio", oDatos.Nombre.ToUpper());
            Text_Html = Text_Html.Replace("@docnegocio", oDatos.RFC);
            Text_Html = Text_Html.Replace("@direcnegocio", oDatos.Direccion);

            Text_Html = Text_Html.Replace("@tipodocumento", TxtTipoDocumento.Text.ToUpper());
            Text_Html = Text_Html.Replace("@numerodocumento", TxtIndiceCliente.Text);

            Text_Html = Text_Html.Replace("@doccliente", TxtNumeroDocumento.Text);
            Text_Html = Text_Html.Replace("@nombrecliente", TxtNombreCliente.Text);
            Text_Html = Text_Html.Replace("@fecharegistro", TxtFecha.Text);
            Text_Html = Text_Html.Replace("@usuarioregistro", TxtUsuario.Text);

            string filas = "<tr>"; // Iniciar una fila
            foreach (DataGridViewRow row in DgvData.Rows)
            {
                filas += "<td>" + row.Cells["Producto"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Precio"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Cantidad"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["SubTotal"].Value.ToString() + "</td>";
            }
            filas += "</tr>";

            Text_Html = Text_Html.Replace("@fila", filas);
            Text_Html = Text_Html.Replace("@montototal", TxtTotalPagar.Text);

            Text_Html = Text_Html.Replace("@pagocon", TxtPaga.Text);
            Text_Html = Text_Html.Replace("@cambio", TxtCambio.Text);

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = string.Format("Venta_{0}.pdf", TxtNumeroDocumento.Text);
            savefile.Filter = "Pdf Files | *.pdf";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create))
                {
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    bool obtenido = true;
                    byte[] byteImage = new CN_Negocio().ObtenerLogo(out obtenido);

                    if (obtenido)
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                        img.ScaleToFit(60, 60);
                        img.Alignment = iTextSharp.text.Image.UNDERLYING;
                        img.SetAbsolutePosition(pdfDoc.Left, pdfDoc.GetTop(51));
                        pdfDoc.Add(img);
                    }

                    using (StringReader sr = new StringReader(Text_Html))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }
                    pdfDoc.Close();
                    stream.Close();
                    MessageBox.Show("Documento Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void TxtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Venta oVenta = new CN_Venta().ObtenerVenta(TxtBuscar.Text);
                if (oVenta.IdVenta != 0)
                {
                    TxtBuscar.BackColor = Color.Honeydew;
                    TxtIndiceCliente.Text = oVenta.NumeroDocumento;
                    TxtFecha.Text = oVenta.FechaRegistro;
                    TxtTipoDocumento.Text = oVenta.TipoDocumento;
                    TxtUsuario.Text = oVenta.oUsuario.NombreCompleto;
                    TxtNumeroDocumento.Text = oVenta.DocumentoCliente;
                    TxtNombreCliente.Text = oVenta.NombreCliente;

                    DgvData.Rows.Clear();
                    foreach (Detalle_Venta dc in oVenta.oDetalleVenta)
                    {
                        DgvData.Rows.Add(new object[]
                        {
                        dc.oMonera.Nombre,
                        dc.PrecioVenta,
                        dc.Cantidad,
                        dc.SubTotal
                        });
                    }
                    TxtTotalPagar.Text = oVenta.MontoTotal.ToString("0.00");
                    TxtPaga.Text = oVenta.MontoPago.ToString("0.00");
                    TxtCambio.Text = oVenta.MontoCambio.ToString("0.00");
                }
                else
                {
                    TxtBuscar.BackColor = Color.MistyRose;
                    TxtIndiceCliente.Text = "";
                    TxtFecha.Text = "";
                    TxtTipoDocumento.Text = "";
                    TxtUsuario.Text = "";
                    TxtNumeroDocumento.Text = "";
                    TxtNombreCliente.Text = "";

                    DgvData.Rows.Clear();

                    TxtTotalPagar.Text = "0.00";
                    TxtPaga.Text = "0.00";
                    TxtCambio.Text = "0.00";
                }
            }
        }

        private void frmDetalleVenta_Load(object sender, EventArgs e)
        {
            TxtBuscar.Select();
        }
    }
}
