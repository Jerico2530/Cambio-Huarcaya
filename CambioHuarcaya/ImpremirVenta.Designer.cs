namespace CambioHuarcaya
{
    partial class ImpremirVenta
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btImprimir = new FontAwesome.Sharp.IconButton();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // btImprimir
            // 
            this.btImprimir.IconChar = FontAwesome.Sharp.IconChar.Print;
            this.btImprimir.IconColor = System.Drawing.Color.Black;
            this.btImprimir.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btImprimir.IconSize = 20;
            this.btImprimir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btImprimir.Location = new System.Drawing.Point(213, 16);
            this.btImprimir.Name = "btImprimir";
            this.btImprimir.Size = new System.Drawing.Size(75, 34);
            this.btImprimir.TabIndex = 4;
            this.btImprimir.Text = "Imprimir";
            this.btImprimir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btImprimir.UseVisualStyleBackColor = true;
            this.btImprimir.Click += new System.EventHandler(this.btImprimir_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(85, 64);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(343, 521);
            this.webBrowser1.TabIndex = 3;
            // 
            // ImpremirVenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 608);
            this.Controls.Add(this.btImprimir);
            this.Controls.Add(this.webBrowser1);
            this.Name = "ImpremirVenta";
            this.Text = "ImpremirVenta";
            this.Load += new System.EventHandler(this.ImpremirVenta_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ImpremirVenta_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private FontAwesome.Sharp.IconButton btImprimir;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}