namespace CapaPresentacion.Forms
{
    partial class frmAcercaDe
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAcercaDe));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            lbVersion = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            richTextBox1 = new RichTextBox();
            guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lbNumeroSerie = new Guna.UI2.WinForms.Guna2HtmlLabel();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).BeginInit();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackColor = Color.LimeGreen;
            guna2Panel1.CustomizableEdges = customizableEdges1;
            guna2Panel1.Dock = DockStyle.Top;
            guna2Panel1.FillColor = Color.FromArgb(45, 45, 45);
            guna2Panel1.Location = new Point(0, 0);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Panel1.Size = new Size(500, 2);
            guna2Panel1.TabIndex = 2;
            // 
            // guna2PictureBox1
            // 
            guna2PictureBox1.BackColor = Color.Transparent;
            guna2PictureBox1.CustomizableEdges = customizableEdges3;
            guna2PictureBox1.Image = (Image)resources.GetObject("guna2PictureBox1.Image");
            guna2PictureBox1.ImageRotate = 0F;
            guna2PictureBox1.Location = new Point(12, 12);
            guna2PictureBox1.Name = "guna2PictureBox1";
            guna2PictureBox1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2PictureBox1.Size = new Size(102, 100);
            guna2PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            guna2PictureBox1.TabIndex = 4;
            guna2PictureBox1.TabStop = false;
            // 
            // lbVersion
            // 
            lbVersion.AutoSize = false;
            lbVersion.BackColor = Color.Transparent;
            lbVersion.Font = new Font("Segoe UI Semibold", 9F);
            lbVersion.ForeColor = Color.Gainsboro;
            lbVersion.Location = new Point(12, 118);
            lbVersion.Name = "lbVersion";
            lbVersion.Size = new Size(480, 20);
            lbVersion.TabIndex = 7;
            lbVersion.Text = "Version 1.0.0 Stable";
            // 
            // guna2HtmlLabel1
            // 
            guna2HtmlLabel1.AutoSize = false;
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Font = new Font("Segoe UI Semibold", 9F);
            guna2HtmlLabel1.ForeColor = Color.Gainsboro;
            guna2HtmlLabel1.Location = new Point(12, 144);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(480, 20);
            guna2HtmlLabel1.TabIndex = 8;
            guna2HtmlLabel1.Text = "© 2026 Schumacher Josias Mariano. Todos los derechos reservados.";
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.FromArgb(40, 40, 40);
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.ForeColor = Color.Gainsboro;
            richTextBox1.Location = new Point(30, 170);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(440, 100);
            richTextBox1.TabIndex = 9;
            richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // guna2HtmlLabel2
            // 
            guna2HtmlLabel2.BackColor = Color.Transparent;
            guna2HtmlLabel2.Font = new Font("Segoe UI Semibold", 9F);
            guna2HtmlLabel2.ForeColor = Color.Gainsboro;
            guna2HtmlLabel2.Location = new Point(30, 301);
            guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            guna2HtmlLabel2.Size = new Size(87, 17);
            guna2HtmlLabel2.TabIndex = 33;
            guna2HtmlLabel2.Text = "ID de Producto:";
            // 
            // lbNumeroSerie
            // 
            lbNumeroSerie.BackColor = Color.Transparent;
            lbNumeroSerie.Font = new Font("Segoe UI Semibold", 9F);
            lbNumeroSerie.ForeColor = Color.Gainsboro;
            lbNumeroSerie.Location = new Point(123, 301);
            lbNumeroSerie.Name = "lbNumeroSerie";
            lbNumeroSerie.Size = new Size(116, 17);
            lbNumeroSerie.TabIndex = 34;
            lbNumeroSerie.Text = "[NUMERO_DE_SERIE]";
            // 
            // frmAcercaDe
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(500, 350);
            Controls.Add(lbNumeroSerie);
            Controls.Add(guna2HtmlLabel2);
            Controls.Add(richTextBox1);
            Controls.Add(guna2HtmlLabel1);
            Controls.Add(lbVersion);
            Controls.Add(guna2PictureBox1);
            Controls.Add(guna2Panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmAcercaDe";
            Text = "frmAcercaDe";
            Load += frmAcercaDe_Load;
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbVersion;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private RichTextBox richTextBox1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbNumeroSerie;
    }
}