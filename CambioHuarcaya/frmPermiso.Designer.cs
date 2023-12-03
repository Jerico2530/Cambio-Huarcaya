namespace CambioHuarcaya
{
    partial class frmPermiso
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
            System.Windows.Forms.Label label1;
            this.txtIdRol = new System.Windows.Forms.TextBox();
            this.txtRol = new System.Windows.Forms.TextBox();
            this.Descripción = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MENUACER = new System.Windows.Forms.CheckBox();
            this.MENURE = new System.Windows.Forms.CheckBox();
            this.MENUPROVE = new System.Windows.Forms.CheckBox();
            this.MENICLIENT = new System.Windows.Forms.CheckBox();
            this.MENUCOM = new System.Windows.Forms.CheckBox();
            this.MENUVEN = new System.Windows.Forms.CheckBox();
            this.MENUMANTE = new System.Windows.Forms.CheckBox();
            this.MENUUSARIO = new System.Windows.Forms.CheckBox();
            this.cbUusario = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.BtnGuardarRol = new FontAwesome.Sharp.IconButton();
            this.BtnGuardarPermiso = new FontAwesome.Sharp.IconButton();
            label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            label1.BackColor = System.Drawing.Color.White;
            label1.Font = new System.Drawing.Font("Modern No. 20", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            label1.Location = new System.Drawing.Point(96, 36);
            label1.Name = "label1";
            label1.Padding = new System.Windows.Forms.Padding(4, 6, 0, 0);
            label1.Size = new System.Drawing.Size(795, 489);
            label1.TabIndex = 84;
            // 
            // txtIdRol
            // 
            this.txtIdRol.Location = new System.Drawing.Point(378, 94);
            this.txtIdRol.Name = "txtIdRol";
            this.txtIdRol.Size = new System.Drawing.Size(37, 20);
            this.txtIdRol.TabIndex = 64;
            this.txtIdRol.Visible = false;
            // 
            // txtRol
            // 
            this.txtRol.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtRol.Location = new System.Drawing.Point(583, 169);
            this.txtRol.Name = "txtRol";
            this.txtRol.Size = new System.Drawing.Size(263, 26);
            this.txtRol.TabIndex = 62;
            // 
            // Descripción
            // 
            this.Descripción.AutoSize = true;
            this.Descripción.BackColor = System.Drawing.Color.White;
            this.Descripción.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.Descripción.Location = new System.Drawing.Point(578, 125);
            this.Descripción.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Descripción.Name = "Descripción";
            this.Descripción.Size = new System.Drawing.Size(131, 25);
            this.Descripción.TabIndex = 61;
            this.Descripción.Text = "Tipo usuario";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("Modern No. 20", 20.25F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(578, 67);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 29);
            this.label3.TabIndex = 60;
            this.label3.Text = "Roles";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.MENUACER);
            this.groupBox1.Controls.Add(this.MENURE);
            this.groupBox1.Controls.Add(this.MENUPROVE);
            this.groupBox1.Controls.Add(this.MENICLIENT);
            this.groupBox1.Controls.Add(this.MENUCOM);
            this.groupBox1.Controls.Add(this.MENUVEN);
            this.groupBox1.Controls.Add(this.MENUMANTE);
            this.groupBox1.Controls.Add(this.MENUUSARIO);
            this.groupBox1.Font = new System.Drawing.Font("Modern No. 20", 20.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(163, 226);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 196);
            this.groupBox1.TabIndex = 57;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Permisos Usuario";
            // 
            // MENUACER
            // 
            this.MENUACER.AutoSize = true;
            this.MENUACER.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.MENUACER.Location = new System.Drawing.Point(212, 140);
            this.MENUACER.Name = "MENUACER";
            this.MENUACER.Size = new System.Drawing.Size(128, 29);
            this.MENUACER.TabIndex = 8;
            this.MENUACER.Text = "Acerca de";
            this.MENUACER.UseVisualStyleBackColor = true;
            // 
            // MENURE
            // 
            this.MENURE.AutoSize = true;
            this.MENURE.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.MENURE.Location = new System.Drawing.Point(212, 105);
            this.MENURE.Name = "MENURE";
            this.MENURE.Size = new System.Drawing.Size(118, 29);
            this.MENURE.TabIndex = 6;
            this.MENURE.Text = "Reportes";
            this.MENURE.UseVisualStyleBackColor = true;
            // 
            // MENUPROVE
            // 
            this.MENUPROVE.AutoSize = true;
            this.MENUPROVE.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.MENUPROVE.Location = new System.Drawing.Point(7, 138);
            this.MENUPROVE.Name = "MENUPROVE";
            this.MENUPROVE.Size = new System.Drawing.Size(153, 29);
            this.MENUPROVE.TabIndex = 5;
            this.MENUPROVE.Text = "Proveedores";
            this.MENUPROVE.UseVisualStyleBackColor = true;
            // 
            // MENICLIENT
            // 
            this.MENICLIENT.AutoSize = true;
            this.MENICLIENT.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.MENICLIENT.Location = new System.Drawing.Point(7, 103);
            this.MENICLIENT.Name = "MENICLIENT";
            this.MENICLIENT.Size = new System.Drawing.Size(109, 29);
            this.MENICLIENT.TabIndex = 4;
            this.MENICLIENT.Text = "Clientes";
            this.MENICLIENT.UseVisualStyleBackColor = true;
            // 
            // MENUCOM
            // 
            this.MENUCOM.AutoSize = true;
            this.MENUCOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.MENUCOM.Location = new System.Drawing.Point(213, 70);
            this.MENUCOM.Name = "MENUCOM";
            this.MENUCOM.Size = new System.Drawing.Size(117, 29);
            this.MENUCOM.TabIndex = 3;
            this.MENUCOM.Text = "Compras";
            this.MENUCOM.UseVisualStyleBackColor = true;
            // 
            // MENUVEN
            // 
            this.MENUVEN.AutoSize = true;
            this.MENUVEN.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.MENUVEN.Location = new System.Drawing.Point(7, 68);
            this.MENUVEN.Name = "MENUVEN";
            this.MENUVEN.Size = new System.Drawing.Size(98, 29);
            this.MENUVEN.TabIndex = 2;
            this.MENUVEN.Text = "Ventas";
            this.MENUVEN.UseVisualStyleBackColor = true;
            // 
            // MENUMANTE
            // 
            this.MENUMANTE.AutoSize = true;
            this.MENUMANTE.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.MENUMANTE.Location = new System.Drawing.Point(213, 35);
            this.MENUMANTE.Name = "MENUMANTE";
            this.MENUMANTE.Size = new System.Drawing.Size(190, 29);
            this.MENUMANTE.TabIndex = 1;
            this.MENUMANTE.Text = "Registro Monera";
            this.MENUMANTE.UseVisualStyleBackColor = true;
            // 
            // MENUUSARIO
            // 
            this.MENUUSARIO.AutoSize = true;
            this.MENUUSARIO.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.MENUUSARIO.Location = new System.Drawing.Point(7, 33);
            this.MENUUSARIO.Name = "MENUUSARIO";
            this.MENUUSARIO.Size = new System.Drawing.Size(105, 29);
            this.MENUUSARIO.TabIndex = 0;
            this.MENUUSARIO.Text = "Usuario";
            this.MENUUSARIO.UseVisualStyleBackColor = true;
            // 
            // cbUusario
            // 
            this.cbUusario.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cbUusario.FormattingEnabled = true;
            this.cbUusario.Location = new System.Drawing.Point(193, 167);
            this.cbUusario.Margin = new System.Windows.Forms.Padding(2);
            this.cbUusario.Name = "cbUusario";
            this.cbUusario.Size = new System.Drawing.Size(222, 28);
            this.cbUusario.TabIndex = 56;
            this.cbUusario.SelectedIndexChanged += new System.EventHandler(this.cbUusario_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.White;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.label8.Location = new System.Drawing.Point(188, 125);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(131, 25);
            this.label8.TabIndex = 55;
            this.label8.Text = "Tipo usuario";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.Font = new System.Drawing.Font("Modern No. 20", 20.25F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(188, 67);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 29);
            this.label9.TabIndex = 54;
            this.label9.Text = "Permisos";
            // 
            // BtnGuardarRol
            // 
            this.BtnGuardarRol.BackColor = System.Drawing.Color.Green;
            this.BtnGuardarRol.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnGuardarRol.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Bold);
            this.BtnGuardarRol.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.BtnGuardarRol.IconColor = System.Drawing.Color.Black;
            this.BtnGuardarRol.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.BtnGuardarRol.IconSize = 20;
            this.BtnGuardarRol.Location = new System.Drawing.Point(583, 210);
            this.BtnGuardarRol.Name = "BtnGuardarRol";
            this.BtnGuardarRol.Size = new System.Drawing.Size(263, 34);
            this.BtnGuardarRol.TabIndex = 85;
            this.BtnGuardarRol.Text = "Guardar";
            this.BtnGuardarRol.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BtnGuardarRol.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnGuardarRol.UseVisualStyleBackColor = false;
            this.BtnGuardarRol.Click += new System.EventHandler(this.BtnGuardarRol_Click);
            // 
            // BtnGuardarPermiso
            // 
            this.BtnGuardarPermiso.BackColor = System.Drawing.Color.Green;
            this.BtnGuardarPermiso.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnGuardarPermiso.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Bold);
            this.BtnGuardarPermiso.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.BtnGuardarPermiso.IconColor = System.Drawing.Color.Black;
            this.BtnGuardarPermiso.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.BtnGuardarPermiso.IconSize = 20;
            this.BtnGuardarPermiso.Location = new System.Drawing.Point(163, 442);
            this.BtnGuardarPermiso.Name = "BtnGuardarPermiso";
            this.BtnGuardarPermiso.Size = new System.Drawing.Size(414, 34);
            this.BtnGuardarPermiso.TabIndex = 86;
            this.BtnGuardarPermiso.Text = "Guardar";
            this.BtnGuardarPermiso.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.BtnGuardarPermiso.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnGuardarPermiso.UseVisualStyleBackColor = false;
            this.BtnGuardarPermiso.Click += new System.EventHandler(this.BtnGuardarPermiso_Click);
            // 
            // frmPermiso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 603);
            this.Controls.Add(this.BtnGuardarPermiso);
            this.Controls.Add(this.BtnGuardarRol);
            this.Controls.Add(this.txtIdRol);
            this.Controls.Add(this.txtRol);
            this.Controls.Add(this.Descripción);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbUusario);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(label1);
            this.Name = "frmPermiso";
            this.Text = "frmPermiso";
            this.Load += new System.EventHandler(this.frmPermiso_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIdRol;
        private System.Windows.Forms.TextBox txtRol;
        private System.Windows.Forms.Label Descripción;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox MENUACER;
        private System.Windows.Forms.CheckBox MENURE;
        private System.Windows.Forms.CheckBox MENUPROVE;
        private System.Windows.Forms.CheckBox MENICLIENT;
        private System.Windows.Forms.CheckBox MENUCOM;
        private System.Windows.Forms.CheckBox MENUVEN;
        private System.Windows.Forms.CheckBox MENUMANTE;
        private System.Windows.Forms.CheckBox MENUUSARIO;
        private System.Windows.Forms.ComboBox cbUusario;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private FontAwesome.Sharp.IconButton BtnGuardarRol;
        private FontAwesome.Sharp.IconButton BtnGuardarPermiso;
    }
}