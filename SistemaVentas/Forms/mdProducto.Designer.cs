namespace CapaPresentacion.Forms
{
    partial class mdProducto
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mdProducto));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            btnLimpiarData = new Guna.UI2.WinForms.Guna2Button();
            tbBuscarProductos = new Guna.UI2.WinForms.Guna2TextBox();
            cbxBuscarPor = new Guna.UI2.WinForms.Guna2ComboBox();
            guna2HtmlLabel5 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2HtmlLabel4 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            dgvMdProductos = new Guna.UI2.WinForms.Guna2DataGridView();
            btnSeleccionar = new DataGridViewButtonColumn();
            Id = new DataGridViewTextBoxColumn();
            Codigo = new DataGridViewTextBoxColumn();
            Nombre = new DataGridViewTextBoxColumn();
            Categoria = new DataGridViewTextBoxColumn();
            guna2Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMdProductos).BeginInit();
            SuspendLayout();
            // 
            // guna2Panel2
            // 
            guna2Panel2.BorderRadius = 10;
            guna2Panel2.BorderStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            guna2Panel2.Controls.Add(btnLimpiarData);
            guna2Panel2.Controls.Add(tbBuscarProductos);
            guna2Panel2.Controls.Add(cbxBuscarPor);
            guna2Panel2.Controls.Add(guna2HtmlLabel5);
            guna2Panel2.Controls.Add(guna2HtmlLabel4);
            customizableEdges7.TopLeft = false;
            customizableEdges7.TopRight = false;
            guna2Panel2.CustomizableEdges = customizableEdges7;
            guna2Panel2.Dock = DockStyle.Top;
            guna2Panel2.FillColor = Color.FromArgb(45, 45, 48);
            guna2Panel2.Location = new Point(0, 0);
            guna2Panel2.Margin = new Padding(0);
            guna2Panel2.Name = "guna2Panel2";
            guna2Panel2.ShadowDecoration.CustomizableEdges = customizableEdges8;
            guna2Panel2.Size = new Size(584, 100);
            guna2Panel2.TabIndex = 6;
            // 
            // btnLimpiarData
            // 
            btnLimpiarData.Animated = true;
            btnLimpiarData.BackColor = Color.Transparent;
            btnLimpiarData.BorderColor = Color.FromArgb(45, 45, 45);
            btnLimpiarData.BorderRadius = 20;
            btnLimpiarData.BorderThickness = 1;
            btnLimpiarData.Cursor = Cursors.Hand;
            btnLimpiarData.CustomizableEdges = customizableEdges1;
            btnLimpiarData.DialogResult = DialogResult.OK;
            btnLimpiarData.DisabledState.BorderColor = Color.DarkGray;
            btnLimpiarData.DisabledState.CustomBorderColor = Color.DarkGray;
            btnLimpiarData.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnLimpiarData.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnLimpiarData.FillColor = Color.FromArgb(30, 30, 30);
            btnLimpiarData.Font = new Font("Segoe UI", 9F);
            btnLimpiarData.ForeColor = Color.Gainsboro;
            btnLimpiarData.HoverState.BorderColor = SystemColors.Highlight;
            btnLimpiarData.HoverState.FillColor = Color.FromArgb(70, 70, 70);
            btnLimpiarData.HoverState.ForeColor = Color.White;
            btnLimpiarData.Image = (Image)resources.GetObject("btnLimpiarData.Image");
            btnLimpiarData.ImageSize = new Size(40, 40);
            btnLimpiarData.Location = new Point(519, 44);
            btnLimpiarData.Margin = new Padding(5);
            btnLimpiarData.Name = "btnLimpiarData";
            btnLimpiarData.PressedColor = Color.FromArgb(20, 20, 20);
            btnLimpiarData.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnLimpiarData.Size = new Size(42, 44);
            btnLimpiarData.TabIndex = 24;
            btnLimpiarData.Click += btnLimpiarData_Click;
            // 
            // tbBuscarProductos
            // 
            tbBuscarProductos.BackColor = Color.Transparent;
            tbBuscarProductos.BorderColor = Color.FromArgb(64, 64, 64);
            tbBuscarProductos.BorderRadius = 15;
            tbBuscarProductos.BorderThickness = 2;
            tbBuscarProductos.CustomizableEdges = customizableEdges3;
            tbBuscarProductos.DefaultText = "";
            tbBuscarProductos.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            tbBuscarProductos.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            tbBuscarProductos.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            tbBuscarProductos.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            tbBuscarProductos.FillColor = Color.FromArgb(30, 30, 30);
            tbBuscarProductos.FocusedState.BorderColor = Color.FromArgb(212, 175, 55);
            tbBuscarProductos.FocusedState.PlaceholderForeColor = Color.Transparent;
            tbBuscarProductos.Font = new Font("Segoe UI", 9F);
            tbBuscarProductos.HoverState.BorderColor = Color.Gray;
            tbBuscarProductos.IconLeftOffset = new Point(10, 0);
            tbBuscarProductos.Location = new Point(385, 48);
            tbBuscarProductos.Margin = new Padding(5);
            tbBuscarProductos.Name = "tbBuscarProductos";
            tbBuscarProductos.PlaceholderForeColor = Color.FromArgb(120, 120, 120);
            tbBuscarProductos.PlaceholderText = "Escriba aqui...";
            tbBuscarProductos.SelectedText = "";
            tbBuscarProductos.ShadowDecoration.CustomizableEdges = customizableEdges4;
            tbBuscarProductos.Size = new Size(124, 36);
            tbBuscarProductos.TabIndex = 24;
            tbBuscarProductos.TextChanged += tbBuscarProveedores_TextChanged;
            // 
            // cbxBuscarPor
            // 
            cbxBuscarPor.BackColor = Color.Transparent;
            cbxBuscarPor.BorderColor = Color.FromArgb(64, 64, 64);
            cbxBuscarPor.BorderRadius = 15;
            cbxBuscarPor.CustomizableEdges = customizableEdges5;
            cbxBuscarPor.DrawMode = DrawMode.OwnerDrawFixed;
            cbxBuscarPor.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxBuscarPor.FillColor = Color.FromArgb(33, 33, 33);
            cbxBuscarPor.FocusedColor = Color.FromArgb(212, 175, 55);
            cbxBuscarPor.FocusedState.BorderColor = Color.FromArgb(212, 175, 55);
            cbxBuscarPor.Font = new Font("Segoe UI", 9F);
            cbxBuscarPor.ForeColor = Color.FromArgb(125, 137, 149);
            cbxBuscarPor.HoverState.BorderColor = Color.FromArgb(212, 175, 55);
            cbxBuscarPor.ItemHeight = 30;
            cbxBuscarPor.ItemsAppearance.BackColor = Color.FromArgb(37, 37, 38);
            cbxBuscarPor.ItemsAppearance.ForeColor = Color.White;
            cbxBuscarPor.Location = new Point(266, 48);
            cbxBuscarPor.Margin = new Padding(5);
            cbxBuscarPor.Name = "cbxBuscarPor";
            cbxBuscarPor.ShadowDecoration.CustomizableEdges = customizableEdges6;
            cbxBuscarPor.Size = new Size(111, 36);
            cbxBuscarPor.TabIndex = 24;
            // 
            // guna2HtmlLabel5
            // 
            guna2HtmlLabel5.BackColor = Color.Transparent;
            guna2HtmlLabel5.Font = new Font("Segoe UI Symbol", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            guna2HtmlLabel5.ForeColor = Color.WhiteSmoke;
            guna2HtmlLabel5.Location = new Point(196, 57);
            guna2HtmlLabel5.Name = "guna2HtmlLabel5";
            guna2HtmlLabel5.Size = new Size(62, 17);
            guna2HtmlLabel5.TabIndex = 4;
            guna2HtmlLabel5.Text = "Buscar por:";
            // 
            // guna2HtmlLabel4
            // 
            guna2HtmlLabel4.BackColor = Color.Transparent;
            guna2HtmlLabel4.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2HtmlLabel4.ForeColor = Color.Gainsboro;
            guna2HtmlLabel4.Location = new Point(9, 9);
            guna2HtmlLabel4.Name = "guna2HtmlLabel4";
            guna2HtmlLabel4.Size = new Size(217, 32);
            guna2HtmlLabel4.TabIndex = 3;
            guna2HtmlLabel4.Text = "LISTA DE PRODUCTOS";
            // 
            // dgvMdProductos
            // 
            dgvMdProductos.AllowUserToAddRows = false;
            dgvMdProductos.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(37, 37, 38);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(70, 70, 70);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dgvMdProductos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvMdProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvMdProductos.BackgroundColor = Color.FromArgb(37, 37, 38);
            dgvMdProductos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(45, 45, 48);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(212, 175, 55);
            dataGridViewCellStyle2.Padding = new Padding(2);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(67, 67, 67);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvMdProductos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvMdProductos.ColumnHeadersHeight = 40;
            dgvMdProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvMdProductos.Columns.AddRange(new DataGridViewColumn[] { btnSeleccionar, Id, Codigo, Nombre, Categoria });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(33, 33, 33);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(70, 70, 70);
            dataGridViewCellStyle3.SelectionForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvMdProductos.DefaultCellStyle = dataGridViewCellStyle3;
            dgvMdProductos.Dock = DockStyle.Bottom;
            dgvMdProductos.GridColor = Color.FromArgb(60, 60, 60);
            dgvMdProductos.Location = new Point(0, 115);
            dgvMdProductos.MultiSelect = false;
            dgvMdProductos.Name = "dgvMdProductos";
            dgvMdProductos.ReadOnly = true;
            dgvMdProductos.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(33, 33, 33);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(70, 70, 70);
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dgvMdProductos.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvMdProductos.RowTemplate.Height = 28;
            dgvMdProductos.Size = new Size(584, 346);
            dgvMdProductos.TabIndex = 5;
            dgvMdProductos.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(37, 37, 38);
            dgvMdProductos.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvMdProductos.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvMdProductos.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(70, 70, 70);
            dgvMdProductos.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.White;
            dgvMdProductos.ThemeStyle.BackColor = Color.FromArgb(37, 37, 38);
            dgvMdProductos.ThemeStyle.GridColor = Color.FromArgb(60, 60, 60);
            dgvMdProductos.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvMdProductos.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvMdProductos.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dgvMdProductos.ThemeStyle.HeaderStyle.ForeColor = Color.FromArgb(212, 175, 55);
            dgvMdProductos.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvMdProductos.ThemeStyle.HeaderStyle.Height = 40;
            dgvMdProductos.ThemeStyle.ReadOnly = true;
            dgvMdProductos.ThemeStyle.RowsStyle.BackColor = Color.FromArgb(33, 33, 33);
            dgvMdProductos.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMdProductos.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgvMdProductos.ThemeStyle.RowsStyle.ForeColor = Color.WhiteSmoke;
            dgvMdProductos.ThemeStyle.RowsStyle.Height = 28;
            dgvMdProductos.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(70, 70, 70);
            dgvMdProductos.ThemeStyle.RowsStyle.SelectionForeColor = Color.White;
            dgvMdProductos.CellContentClick += dgvMdProductos_CellContentClick;
            dgvMdProductos.RowPrePaint += dgvMdProductos_RowPrePaint;
            // 
            // btnSeleccionar
            // 
            btnSeleccionar.DataPropertyName = "btnSeleccionar";
            btnSeleccionar.FillWeight = 61.17382F;
            btnSeleccionar.FlatStyle = FlatStyle.Flat;
            btnSeleccionar.HeaderText = "";
            btnSeleccionar.MinimumWidth = 20;
            btnSeleccionar.Name = "btnSeleccionar";
            btnSeleccionar.ReadOnly = true;
            btnSeleccionar.Text = "👁";
            btnSeleccionar.UseColumnTextForButtonValue = true;
            btnSeleccionar.Width = 30;
            // 
            // Id
            // 
            Id.DataPropertyName = "Id";
            Id.HeaderText = "IdProducto";
            Id.Name = "Id";
            Id.ReadOnly = true;
            Id.Visible = false;
            // 
            // Codigo
            // 
            Codigo.DataPropertyName = "Codigo";
            Codigo.HeaderText = "Codigo";
            Codigo.Name = "Codigo";
            Codigo.ReadOnly = true;
            Codigo.Width = 150;
            // 
            // Nombre
            // 
            Nombre.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Nombre.DataPropertyName = "Nombre";
            Nombre.HeaderText = "Nombre";
            Nombre.Name = "Nombre";
            Nombre.ReadOnly = true;
            // 
            // Categoria
            // 
            Categoria.DataPropertyName = "Categoria";
            Categoria.HeaderText = "Categoria";
            Categoria.Name = "Categoria";
            Categoria.ReadOnly = true;
            Categoria.Width = 200;
            // 
            // mdProducto
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(584, 461);
            Controls.Add(guna2Panel2);
            Controls.Add(dgvMdProductos);
            Name = "mdProducto";
            Text = "mdProducto";
            Load += mdProducto_Load;
            guna2Panel2.ResumeLayout(false);
            guna2Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMdProductos).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        public Guna.UI2.WinForms.Guna2Button btnLimpiarData;
        private Guna.UI2.WinForms.Guna2TextBox tbBuscarProductos;
        private Guna.UI2.WinForms.Guna2ComboBox cbxBuscarPor;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel5;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel4;
        private Guna.UI2.WinForms.Guna2DataGridView dgvMdProductos;
        private DataGridViewButtonColumn btnSeleccionar;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn Codigo;
        private DataGridViewTextBoxColumn Nombre;
        private DataGridViewTextBoxColumn Categoria;
    }
}