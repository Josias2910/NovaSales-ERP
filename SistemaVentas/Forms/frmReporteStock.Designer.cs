namespace CapaPresentacion.Forms
{
    partial class frmReporteStock
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReporteStock));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            dgvReporteStock = new Guna.UI2.WinForms.Guna2DataGridView();
            Codigo = new DataGridViewTextBoxColumn();
            Nombre = new DataGridViewTextBoxColumn();
            Categoria = new DataGridViewTextBoxColumn();
            Stock = new DataGridViewTextBoxColumn();
            PrecioCompra = new DataGridViewTextBoxColumn();
            PrecioVenta = new DataGridViewTextBoxColumn();
            MontoTotalStock = new DataGridViewTextBoxColumn();
            guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            lbGananciaPotencial = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2vSeparator3 = new Guna.UI2.WinForms.Guna2VSeparator();
            flowLayoutPanel1 = new FlowLayoutPanel();
            guna2vSeparator2 = new Guna.UI2.WinForms.Guna2VSeparator();
            guna2vSeparator1 = new Guna.UI2.WinForms.Guna2VSeparator();
            lbCategoriaTop = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2HtmlLabel7 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lbAgotados = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2HtmlLabel6 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lbTotalStock = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2HtmlLabel5 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lbTotalArticulos = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            tbBuscarStock = new Guna.UI2.WinForms.Guna2TextBox();
            guna2HtmlLabel4 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            btnLimpiarData = new Guna.UI2.WinForms.Guna2Button();
            btnExportarExcel = new Guna.UI2.WinForms.Guna2CircleButton();
            ((System.ComponentModel.ISupportInitialize)dgvReporteStock).BeginInit();
            guna2Panel2.SuspendLayout();
            guna2Panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvReporteStock
            // 
            dgvReporteStock.AllowUserToAddRows = false;
            dgvReporteStock.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(37, 37, 38);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(70, 70, 70);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dgvReporteStock.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvReporteStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvReporteStock.BackgroundColor = Color.FromArgb(35, 35, 36);
            dgvReporteStock.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(45, 45, 48);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(212, 175, 55);
            dataGridViewCellStyle2.Padding = new Padding(2);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(67, 67, 67);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvReporteStock.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvReporteStock.ColumnHeadersHeight = 40;
            dgvReporteStock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvReporteStock.Columns.AddRange(new DataGridViewColumn[] { Codigo, Nombre, Categoria, Stock, PrecioCompra, PrecioVenta, MontoTotalStock });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(33, 33, 33);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(70, 70, 70);
            dataGridViewCellStyle3.SelectionForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvReporteStock.DefaultCellStyle = dataGridViewCellStyle3;
            dgvReporteStock.Dock = DockStyle.Fill;
            dgvReporteStock.GridColor = Color.FromArgb(60, 60, 60);
            dgvReporteStock.Location = new Point(0, 100);
            dgvReporteStock.MultiSelect = false;
            dgvReporteStock.Name = "dgvReporteStock";
            dgvReporteStock.ReadOnly = true;
            dgvReporteStock.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(33, 33, 33);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(70, 70, 70);
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dgvReporteStock.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvReporteStock.RowTemplate.Height = 30;
            dgvReporteStock.Size = new Size(1257, 428);
            dgvReporteStock.TabIndex = 8;
            dgvReporteStock.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(37, 37, 38);
            dgvReporteStock.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvReporteStock.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvReporteStock.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(70, 70, 70);
            dgvReporteStock.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.White;
            dgvReporteStock.ThemeStyle.BackColor = Color.FromArgb(35, 35, 36);
            dgvReporteStock.ThemeStyle.GridColor = Color.FromArgb(60, 60, 60);
            dgvReporteStock.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvReporteStock.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvReporteStock.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dgvReporteStock.ThemeStyle.HeaderStyle.ForeColor = Color.FromArgb(212, 175, 55);
            dgvReporteStock.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvReporteStock.ThemeStyle.HeaderStyle.Height = 40;
            dgvReporteStock.ThemeStyle.ReadOnly = true;
            dgvReporteStock.ThemeStyle.RowsStyle.BackColor = Color.FromArgb(33, 33, 33);
            dgvReporteStock.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvReporteStock.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgvReporteStock.ThemeStyle.RowsStyle.ForeColor = Color.WhiteSmoke;
            dgvReporteStock.ThemeStyle.RowsStyle.Height = 30;
            dgvReporteStock.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(70, 70, 70);
            dgvReporteStock.ThemeStyle.RowsStyle.SelectionForeColor = Color.White;
            dgvReporteStock.CellFormatting += dgvReporteStock_CellFormatting;
            dgvReporteStock.RowPrePaint += dgvReporteStock_RowPrePaint;
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
            Nombre.DataPropertyName = "Nombre";
            Nombre.HeaderText = "Nombre";
            Nombre.Name = "Nombre";
            Nombre.ReadOnly = true;
            Nombre.Width = 407;
            // 
            // Categoria
            // 
            Categoria.DataPropertyName = "Categoria";
            Categoria.HeaderText = "Categoria";
            Categoria.Name = "Categoria";
            Categoria.ReadOnly = true;
            Categoria.Width = 150;
            // 
            // Stock
            // 
            Stock.DataPropertyName = "Stock";
            Stock.HeaderText = "Stock";
            Stock.Name = "Stock";
            Stock.ReadOnly = true;
            // 
            // PrecioCompra
            // 
            PrecioCompra.DataPropertyName = "PrecioCompra";
            PrecioCompra.HeaderText = "Precio Compra";
            PrecioCompra.Name = "PrecioCompra";
            PrecioCompra.ReadOnly = true;
            PrecioCompra.Width = 150;
            // 
            // PrecioVenta
            // 
            PrecioVenta.DataPropertyName = "PrecioVenta";
            PrecioVenta.HeaderText = "Precio Venta";
            PrecioVenta.Name = "PrecioVenta";
            PrecioVenta.ReadOnly = true;
            PrecioVenta.Width = 150;
            // 
            // MontoTotalStock
            // 
            MontoTotalStock.DataPropertyName = "MontoTotalStock";
            MontoTotalStock.HeaderText = "Inversion";
            MontoTotalStock.Name = "MontoTotalStock";
            MontoTotalStock.ReadOnly = true;
            MontoTotalStock.Width = 150;
            // 
            // guna2Panel2
            // 
            guna2Panel2.BackColor = Color.FromArgb(37, 37, 38);
            guna2Panel2.Controls.Add(lbGananciaPotencial);
            guna2Panel2.Controls.Add(guna2HtmlLabel1);
            guna2Panel2.Controls.Add(guna2vSeparator3);
            guna2Panel2.Controls.Add(flowLayoutPanel1);
            guna2Panel2.Controls.Add(guna2vSeparator2);
            guna2Panel2.Controls.Add(guna2vSeparator1);
            guna2Panel2.Controls.Add(lbCategoriaTop);
            guna2Panel2.Controls.Add(guna2HtmlLabel7);
            guna2Panel2.Controls.Add(lbAgotados);
            guna2Panel2.Controls.Add(guna2HtmlLabel6);
            guna2Panel2.Controls.Add(lbTotalStock);
            guna2Panel2.Controls.Add(guna2HtmlLabel5);
            guna2Panel2.Controls.Add(lbTotalArticulos);
            guna2Panel2.Controls.Add(guna2HtmlLabel2);
            guna2Panel2.CustomizableEdges = customizableEdges1;
            guna2Panel2.Dock = DockStyle.Bottom;
            guna2Panel2.FillColor = Color.FromArgb(30, 30, 30);
            guna2Panel2.Location = new Point(0, 528);
            guna2Panel2.Name = "guna2Panel2";
            guna2Panel2.Padding = new Padding(10);
            guna2Panel2.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Panel2.Size = new Size(1257, 80);
            guna2Panel2.TabIndex = 7;
            // 
            // lbGananciaPotencial
            // 
            lbGananciaPotencial.BackColor = Color.Transparent;
            lbGananciaPotencial.Font = new Font("Segoe UI Semibold", 10F);
            lbGananciaPotencial.ForeColor = Color.Gainsboro;
            lbGananciaPotencial.Location = new Point(755, 57);
            lbGananciaPotencial.Name = "lbGananciaPotencial";
            lbGananciaPotencial.Size = new Size(8, 19);
            lbGananciaPotencial.TabIndex = 55;
            lbGananciaPotencial.Text = "-";
            // 
            // guna2HtmlLabel1
            // 
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Font = new Font("Segoe UI Semibold", 10F);
            guna2HtmlLabel1.ForeColor = Color.Gainsboro;
            guna2HtmlLabel1.Location = new Point(730, 38);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(120, 19);
            guna2HtmlLabel1.TabIndex = 54;
            guna2HtmlLabel1.Text = "Ganancia Potencial:";
            // 
            // guna2vSeparator3
            // 
            guna2vSeparator3.FillColor = Color.FromArgb(215, 175, 77);
            guna2vSeparator3.Location = new Point(961, 0);
            guna2vSeparator3.Name = "guna2vSeparator3";
            guna2vSeparator3.Size = new Size(10, 80);
            guna2vSeparator3.TabIndex = 53;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(1009, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(248, 80);
            flowLayoutPanel1.TabIndex = 6;
            flowLayoutPanel1.WrapContents = false;
            // 
            // guna2vSeparator2
            // 
            guna2vSeparator2.FillColor = Color.FromArgb(215, 175, 77);
            guna2vSeparator2.Location = new Point(683, 0);
            guna2vSeparator2.Name = "guna2vSeparator2";
            guna2vSeparator2.Size = new Size(10, 80);
            guna2vSeparator2.TabIndex = 50;
            // 
            // guna2vSeparator1
            // 
            guna2vSeparator1.FillColor = Color.FromArgb(215, 175, 77);
            guna2vSeparator1.Location = new Point(383, 0);
            guna2vSeparator1.Name = "guna2vSeparator1";
            guna2vSeparator1.Size = new Size(10, 80);
            guna2vSeparator1.TabIndex = 49;
            // 
            // lbCategoriaTop
            // 
            lbCategoriaTop.BackColor = Color.Transparent;
            lbCategoriaTop.Font = new Font("Segoe UI Semibold", 10F);
            lbCategoriaTop.ForeColor = Color.Gainsboro;
            lbCategoriaTop.Location = new Point(755, 22);
            lbCategoriaTop.Name = "lbCategoriaTop";
            lbCategoriaTop.Size = new Size(8, 19);
            lbCategoriaTop.TabIndex = 48;
            lbCategoriaTop.Text = "-";
            // 
            // guna2HtmlLabel7
            // 
            guna2HtmlLabel7.BackColor = Color.Transparent;
            guna2HtmlLabel7.Font = new Font("Segoe UI Semibold", 10F);
            guna2HtmlLabel7.ForeColor = Color.Gainsboro;
            guna2HtmlLabel7.Location = new Point(730, 3);
            guna2HtmlLabel7.Name = "guna2HtmlLabel7";
            guna2HtmlLabel7.Size = new Size(146, 19);
            guna2HtmlLabel7.TabIndex = 47;
            guna2HtmlLabel7.Text = "Categoria mas Poblada:";
            // 
            // lbAgotados
            // 
            lbAgotados.BackColor = Color.Transparent;
            lbAgotados.Font = new Font("Segoe UI Semibold", 10F);
            lbAgotados.ForeColor = Color.Gainsboro;
            lbAgotados.Location = new Point(557, 45);
            lbAgotados.Name = "lbAgotados";
            lbAgotados.Size = new Size(8, 19);
            lbAgotados.TabIndex = 46;
            lbAgotados.Text = "-";
            // 
            // guna2HtmlLabel6
            // 
            guna2HtmlLabel6.BackColor = Color.Transparent;
            guna2HtmlLabel6.Font = new Font("Segoe UI Semibold", 10F);
            guna2HtmlLabel6.ForeColor = Color.Gainsboro;
            guna2HtmlLabel6.Location = new Point(420, 45);
            guna2HtmlLabel6.Name = "guna2HtmlLabel6";
            guna2HtmlLabel6.Size = new Size(131, 19);
            guna2HtmlLabel6.TabIndex = 45;
            guna2HtmlLabel6.Text = "Productos Agotados:";
            // 
            // lbTotalStock
            // 
            lbTotalStock.BackColor = Color.Transparent;
            lbTotalStock.Font = new Font("Segoe UI Semibold", 10F);
            lbTotalStock.ForeColor = Color.Gainsboro;
            lbTotalStock.Location = new Point(534, 10);
            lbTotalStock.Name = "lbTotalStock";
            lbTotalStock.Size = new Size(8, 19);
            lbTotalStock.TabIndex = 44;
            lbTotalStock.Text = "-";
            // 
            // guna2HtmlLabel5
            // 
            guna2HtmlLabel5.BackColor = Color.Transparent;
            guna2HtmlLabel5.Font = new Font("Segoe UI Semibold", 10F);
            guna2HtmlLabel5.ForeColor = Color.Gainsboro;
            guna2HtmlLabel5.Location = new Point(420, 10);
            guna2HtmlLabel5.Name = "guna2HtmlLabel5";
            guna2HtmlLabel5.Size = new Size(108, 19);
            guna2HtmlLabel5.TabIndex = 43;
            guna2HtmlLabel5.Text = "Valor Total Stock:";
            // 
            // lbTotalArticulos
            // 
            lbTotalArticulos.BackColor = Color.Transparent;
            lbTotalArticulos.Font = new Font("Segoe UI Black", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbTotalArticulos.ForeColor = Color.FromArgb(255, 175, 55);
            lbTotalArticulos.Location = new Point(45, 36);
            lbTotalArticulos.Name = "lbTotalArticulos";
            lbTotalArticulos.Size = new Size(13, 34);
            lbTotalArticulos.TabIndex = 29;
            lbTotalArticulos.Text = "-";
            // 
            // guna2HtmlLabel2
            // 
            guna2HtmlLabel2.BackColor = Color.Transparent;
            guna2HtmlLabel2.Font = new Font("Segoe UI Semibold", 14F);
            guna2HtmlLabel2.ForeColor = Color.Gainsboro;
            guna2HtmlLabel2.Location = new Point(23, 8);
            guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            guna2HtmlLabel2.Size = new Size(197, 27);
            guna2HtmlLabel2.TabIndex = 28;
            guna2HtmlLabel2.Text = "TOTAL DE ARTICULOS:";
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackColor = Color.FromArgb(37, 37, 38);
            guna2Panel1.Controls.Add(tbBuscarStock);
            guna2Panel1.Controls.Add(guna2HtmlLabel4);
            guna2Panel1.Controls.Add(btnLimpiarData);
            guna2Panel1.Controls.Add(btnExportarExcel);
            guna2Panel1.CustomizableEdges = customizableEdges8;
            guna2Panel1.Dock = DockStyle.Top;
            guna2Panel1.FillColor = Color.FromArgb(45, 45, 45);
            guna2Panel1.Location = new Point(0, 0);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges9;
            guna2Panel1.Size = new Size(1257, 100);
            guna2Panel1.TabIndex = 6;
            // 
            // tbBuscarStock
            // 
            tbBuscarStock.BackColor = Color.Transparent;
            tbBuscarStock.BorderColor = Color.FromArgb(64, 64, 64);
            tbBuscarStock.BorderRadius = 12;
            tbBuscarStock.BorderThickness = 2;
            tbBuscarStock.CustomizableEdges = customizableEdges3;
            tbBuscarStock.DefaultText = "";
            tbBuscarStock.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            tbBuscarStock.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            tbBuscarStock.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            tbBuscarStock.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            tbBuscarStock.FillColor = Color.FromArgb(30, 30, 30);
            tbBuscarStock.FocusedState.BorderColor = Color.FromArgb(212, 175, 55);
            tbBuscarStock.FocusedState.PlaceholderForeColor = Color.Transparent;
            tbBuscarStock.Font = new Font("Segoe UI", 9F);
            tbBuscarStock.HoverState.BorderColor = Color.Gray;
            tbBuscarStock.IconLeft = (Image)resources.GetObject("tbBuscarStock.IconLeft");
            tbBuscarStock.IconLeftOffset = new Point(10, 0);
            tbBuscarStock.Location = new Point(819, 31);
            tbBuscarStock.Margin = new Padding(5);
            tbBuscarStock.Name = "tbBuscarStock";
            tbBuscarStock.PlaceholderForeColor = Color.FromArgb(120, 120, 120);
            tbBuscarStock.PlaceholderText = "Buscar";
            tbBuscarStock.SelectedText = "";
            tbBuscarStock.ShadowDecoration.CustomizableEdges = customizableEdges4;
            tbBuscarStock.Size = new Size(195, 40);
            tbBuscarStock.TabIndex = 43;
            tbBuscarStock.TextChanged += tbBuscarStock_TextChanged;
            // 
            // guna2HtmlLabel4
            // 
            guna2HtmlLabel4.BackColor = Color.Transparent;
            guna2HtmlLabel4.Font = new Font("Segoe UI Semibold", 18F);
            guna2HtmlLabel4.ForeColor = Color.Gainsboro;
            guna2HtmlLabel4.Location = new Point(12, 12);
            guna2HtmlLabel4.Name = "guna2HtmlLabel4";
            guna2HtmlLabel4.Size = new Size(183, 34);
            guna2HtmlLabel4.TabIndex = 42;
            guna2HtmlLabel4.Text = "REPORTE STOCK";
            // 
            // btnLimpiarData
            // 
            btnLimpiarData.Animated = true;
            btnLimpiarData.BackColor = Color.Transparent;
            btnLimpiarData.BorderColor = Color.FromArgb(45, 45, 45);
            btnLimpiarData.BorderRadius = 20;
            btnLimpiarData.BorderThickness = 1;
            btnLimpiarData.Cursor = Cursors.Hand;
            btnLimpiarData.CustomizableEdges = customizableEdges5;
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
            btnLimpiarData.Location = new Point(1024, 28);
            btnLimpiarData.Margin = new Padding(5);
            btnLimpiarData.Name = "btnLimpiarData";
            btnLimpiarData.PressedColor = Color.FromArgb(20, 20, 20);
            btnLimpiarData.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnLimpiarData.Size = new Size(42, 44);
            btnLimpiarData.TabIndex = 25;
            btnLimpiarData.Click += btnLimpiarData_Click;
            // 
            // btnExportarExcel
            // 
            btnExportarExcel.Animated = true;
            btnExportarExcel.BackColor = Color.Transparent;
            btnExportarExcel.BackgroundImage = Properties.Resources.excel;
            btnExportarExcel.Cursor = Cursors.Hand;
            btnExportarExcel.DisabledState.BorderColor = Color.DarkGray;
            btnExportarExcel.DisabledState.CustomBorderColor = Color.DarkGray;
            btnExportarExcel.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnExportarExcel.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnExportarExcel.FillColor = Color.FromArgb(70, 70, 70);
            btnExportarExcel.Font = new Font("Segoe UI", 24.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExportarExcel.ForeColor = Color.White;
            btnExportarExcel.HoverState.FillColor = Color.Lime;
            btnExportarExcel.Image = Properties.Resources.excel;
            btnExportarExcel.ImageSize = new Size(38, 38);
            btnExportarExcel.Location = new Point(1074, 31);
            btnExportarExcel.Name = "btnExportarExcel";
            btnExportarExcel.ShadowDecoration.CustomizableEdges = customizableEdges7;
            btnExportarExcel.ShadowDecoration.Enabled = true;
            btnExportarExcel.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            btnExportarExcel.Size = new Size(40, 40);
            btnExportarExcel.TabIndex = 41;
            btnExportarExcel.TextOffset = new Point(1, 0);
            btnExportarExcel.UseTransparentBackground = true;
            btnExportarExcel.Click += btnExportarExcel_Click;
            // 
            // frmReporteStock
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(1257, 608);
            Controls.Add(dgvReporteStock);
            Controls.Add(guna2Panel2);
            Controls.Add(guna2Panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmReporteStock";
            Text = "frmReporteStock";
            Load += frmReporteStock_Load;
            ((System.ComponentModel.ISupportInitialize)dgvReporteStock).EndInit();
            guna2Panel2.ResumeLayout(false);
            guna2Panel2.PerformLayout();
            guna2Panel1.ResumeLayout(false);
            guna2Panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dgvReporteStock;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2VSeparator guna2vSeparator3;
        private FlowLayoutPanel flowLayoutPanel1;
        private Guna.UI2.WinForms.Guna2VSeparator guna2vSeparator2;
        private Guna.UI2.WinForms.Guna2VSeparator guna2vSeparator1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbCategoriaTop;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel7;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbAgotados;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel6;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbTotalStock;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel5;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbTotalArticulos;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel4;
        public Guna.UI2.WinForms.Guna2Button btnLimpiarData;
        private Guna.UI2.WinForms.Guna2CircleButton btnExportarExcel;
        private Guna.UI2.WinForms.Guna2TextBox tbBuscarStock;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbGananciaPotencial;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private DataGridViewTextBoxColumn Codigo;
        private DataGridViewTextBoxColumn Nombre;
        private DataGridViewTextBoxColumn Categoria;
        private DataGridViewTextBoxColumn Stock;
        private DataGridViewTextBoxColumn PrecioCompra;
        private DataGridViewTextBoxColumn PrecioVenta;
        private DataGridViewTextBoxColumn MontoTotalStock;
    }
}