using CambioHuarcaya.Modales;
using CapaEntidad;
using CapaNegocio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CambioHuarcaya
{
    public partial class frmDetalleCompra : Form
    {
        public frmDetalleCompra()
        {
            InitializeComponent();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            using (var modal = new mdCompra())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    TxtBuscar.Text = modal._Compra.NumeroDocumento.ToString();
                    TxtBuscar.BackColor = Color.Honeydew;

                    Compra oCompra = new CN_Compra().ObtenerCompra(TxtBuscar.Text);
                    if (oCompra.IdCompra != 0)
                    {
                        TxtBuscar.BackColor = Color.Honeydew;
                        TxtIndiceCliente.Text = oCompra.NumeroDocumento;

                        TxtFecha.Text = oCompra.FechaRegistro;
                        TxtTipoDocumento.Text = oCompra.TipoDocumento;
                        TxtUsuario.Text = oCompra.oUsuario.NombreCompleto;

                        if (!string.IsNullOrEmpty(oCompra.oProveedor.Documento) && !string.IsNullOrEmpty(oCompra.oProveedor.Banco))
                        {
                            TxtNumeroDocumento.Text = oCompra.oProveedor.Documento;
                            TxtNombreCliente.Text = oCompra.oProveedor.Banco;
                        }
                        else if (!string.IsNullOrEmpty(oCompra.oCliente.Documento) && !string.IsNullOrEmpty(oCompra.oCliente.NombreCompleto))
                        {
                            TxtNumeroDocumento.Text = oCompra.oCliente.Documento;
                            TxtNombreCliente.Text = oCompra.oCliente.NombreCompleto;
                        }

                        DgvData.Rows.Clear();
                        foreach (Detalle_Compra dc in oCompra.oDetalleCompra)
                        {
                            DgvData.Rows.Add(new object[] { dc.oMonera.Nombre, dc.PrecioCompra,dc.PrecioVenta, dc.Cantidad, dc.MontoTotal });
                        }

                        TxtTotalPagar.Text = oCompra.MontoTotal.ToString("0.00");
                    }
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

            string Text_Html = Properties.Resources.PlantillaCompra.ToString();
            Negocio oDatos = new CN_Negocio().ObtenerDatos();

            Text_Html = Text_Html.Replace("@nombrenegocio", oDatos.Nombre.ToUpper());
            Text_Html = Text_Html.Replace("@docnegocio", oDatos.RFC);
            Text_Html = Text_Html.Replace("@direcnegocio", oDatos.Direccion);

            Text_Html = Text_Html.Replace("@tipodocumento", TxtTipoDocumento.Text.ToUpper());
            Text_Html = Text_Html.Replace("@numerodocumento", TxtIndiceCliente.Text);

            Text_Html = Text_Html.Replace("@docproveedor", TxtNumeroDocumento.Text);
            Text_Html = Text_Html.Replace("@nombreproveedor", TxtNombreCliente.Text);
            Text_Html = Text_Html.Replace("@fecharegistro", TxtFecha.Text);
            Text_Html = Text_Html.Replace("@usuarioregistro", TxtUsuario.Text);


            string filas = string.Empty;
            foreach (DataGridViewRow row in DgvData.Rows)
            {
                filas += "<tr>";
                filas += "<tr>" + row.Cells["Nombre"].Value.ToString() + "</td>";
                filas += "<tr>" + row.Cells["PrecioCompra"].Value.ToString() + "</td>";
                filas += "<tr>" + row.Cells["Cantidad"].Value.ToString() + "</td>";
                filas += "<tr>" + row.Cells["MontoTotal"].Value.ToString() + "</td>";
                filas += "</tr>";

            }
            Text_Html = Text_Html.Replace("@fila", filas);
            Text_Html = Text_Html.Replace("@montototal", TxtTotalPagar.Text);

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = string.Format("Compra_{0}.pdf", TxtNumeroDocumento.Text);
            savefile.Filter = "Pdf Files | *.pdf";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create))
                {
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 25, 25, 25, 25);

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
        private void BtnLimpiarBus_Click(object sender, EventArgs e)
        {
            TxtIndiceCliente.Text = "0";
            TxtBuscar.Text = "";
            TxtFecha.Text = "";
            TxtTipoDocumento.Text = "";
            TxtUsuario.Text = "";
            TxtNumeroDocumento.Text = "";
            TxtNombreCliente.Text = "";
            TxtTotalPagar.Text = "0.00";
            DgvData.Rows.Clear();


        }

        private void TxtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Compra oCompra = new CN_Compra().ObtenerCompra(TxtBuscar.Text);
                if (oCompra.IdCompra != 0)
                {
                    TxtBuscar.BackColor = Color.Honeydew;
                    TxtNumeroDocumento.Text = oCompra.NumeroDocumento;
                    TxtFecha.Text = oCompra.FechaRegistro;
                    TxtTipoDocumento.Text = oCompra.TipoDocumento;
                    TxtUsuario.Text = oCompra.oUsuario.NombreCompleto;
                    if (!string.IsNullOrEmpty(oCompra.oProveedor.Documento) && !string.IsNullOrEmpty(oCompra.oProveedor.Banco))
                    {
                        TxtNumeroDocumento.Text = oCompra.oProveedor.Documento;
                        TxtNombreCliente.Text = oCompra.oProveedor.Banco;
                    }
                    else if (!string.IsNullOrEmpty(oCompra.oCliente.Documento) && !string.IsNullOrEmpty(oCompra.oCliente.NombreCompleto))
                    {
                        TxtNumeroDocumento.Text = oCompra.oCliente.Documento;
                        TxtNombreCliente.Text = oCompra.oCliente.NombreCompleto;
                    }

                    DgvData.Rows.Clear();
                    foreach (Detalle_Compra dc in oCompra.oDetalleCompra)
                    {
                        DgvData.Rows.Add(new object[]
                        {
                        dc.oMonera.Nombre,
                        dc.PrecioCompra,
                        dc.Cantidad,
                        dc.MontoTotal
                        });
                    }
                    TxtTotalPagar.Text = oCompra.MontoTotal.ToString("0.00");
                }
                else
                {
                    TxtBuscar.BackColor = Color.MistyRose;
                    TxtNumeroDocumento.Text = "";
                    TxtFecha.Text = "";
                    TxtTipoDocumento.Text = "";
                    TxtUsuario.Text = "";
                    TxtNumeroDocumento.Text = ""; ;
                    TxtNombreCliente.Text = "";

                    DgvData.Rows.Clear();
                    TxtTotalPagar.Text = "0.00";
                }
            }
        }

        private void frmDetalleCompra_Load(object sender, EventArgs e)
        {
            TxtBuscar.Select();
        }
    }
}
