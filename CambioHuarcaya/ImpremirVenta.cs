using CambioHuarcaya.Utilizable;
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
    public partial class ImpremirVenta : Form
    {
        private string _codigoVenta = string.Empty;
        public ImpremirVenta(string codigoVenta)
        {
            InitializeComponent();
            _codigoVenta = codigoVenta;
            string titulo = ("Ticket de venta " + _codigoVenta);
            this.Text = titulo;
        }

        private void ImpremirVenta_Load(object sender, EventArgs e)
        {
            webBrowser1.DocumentText = CrearTicket.crearTicketVenta(_codigoVenta);
            btImprimir.Select();
        }

        private void btImprimir_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();
        }

        private void ImpremirVenta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                webBrowser1.ShowPrintDialog();
            }
        }
    }
}
