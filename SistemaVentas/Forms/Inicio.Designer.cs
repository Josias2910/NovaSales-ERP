namespace SistemaVentas
{
    partial class Inicio
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Inicio));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            menuButtons = new MenuStrip();
            menuUsuarios = new FontAwesome.Sharp.IconMenuItem();
            menuMantenedor = new FontAwesome.Sharp.IconMenuItem();
            subMenuCategoria = new FontAwesome.Sharp.IconMenuItem();
            subMenuProducto = new FontAwesome.Sharp.IconMenuItem();
            subMenuNegocio = new FontAwesome.Sharp.IconMenuItem();
            menuVentas = new FontAwesome.Sharp.IconMenuItem();
            subMenuRegistrarVenta = new FontAwesome.Sharp.IconMenuItem();
            subMenuVerDetalleVenta = new FontAwesome.Sharp.IconMenuItem();
            menuCompras = new FontAwesome.Sharp.IconMenuItem();
            subMenuRegistrarCompra = new FontAwesome.Sharp.IconMenuItem();
            subMenuVerDetalleCompra = new FontAwesome.Sharp.IconMenuItem();
            menuClientes = new FontAwesome.Sharp.IconMenuItem();
            menuProveedores = new FontAwesome.Sharp.IconMenuItem();
            menuReportes = new FontAwesome.Sharp.IconMenuItem();
            subMenuReporteVentas = new ToolStripMenuItem();
            subMenuReporteCompras = new ToolStripMenuItem();
            subMenuReporteStock = new ToolStripMenuItem();
            subMenuReporteResumen = new ToolStripMenuItem();
            menuAcercaDe = new FontAwesome.Sharp.IconMenuItem();
            panel1 = new Panel();
            lbUsuario = new Label();
            label2 = new Label();
            guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            label1 = new Label();
            contenedor = new Panel();
            btnSalir = new Guna.UI2.WinForms.Guna2CircleButton();
            menuButtons.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).BeginInit();
            SuspendLayout();
            // 
            // menuButtons
            // 
            menuButtons.AutoSize = false;
            menuButtons.BackColor = Color.FromArgb(37, 37, 38);
            menuButtons.Dock = DockStyle.Left;
            menuButtons.Items.AddRange(new ToolStripItem[] { menuUsuarios, menuMantenedor, menuVentas, menuCompras, menuClientes, menuProveedores, menuReportes, menuAcercaDe });
            menuButtons.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            menuButtons.Location = new Point(0, 109);
            menuButtons.Name = "menuButtons";
            menuButtons.RenderMode = ToolStripRenderMode.Professional;
            menuButtons.RightToLeft = RightToLeft.Yes;
            menuButtons.Size = new Size(208, 647);
            menuButtons.TabIndex = 0;
            menuButtons.Text = "menuButtons";
            // 
            // menuUsuarios
            // 
            menuUsuarios.AutoSize = false;
            menuUsuarios.BackColor = Color.Transparent;
            menuUsuarios.Font = new Font("Segoe UI", 12F);
            menuUsuarios.ForeColor = Color.WhiteSmoke;
            menuUsuarios.IconChar = FontAwesome.Sharp.IconChar.UserFriends;
            menuUsuarios.IconColor = Color.WhiteSmoke;
            menuUsuarios.IconFont = FontAwesome.Sharp.IconFont.Auto;
            menuUsuarios.IconSize = 40;
            menuUsuarios.ImageAlign = ContentAlignment.MiddleLeft;
            menuUsuarios.ImageScaling = ToolStripItemImageScaling.None;
            menuUsuarios.Margin = new Padding(5);
            menuUsuarios.Name = "menuUsuarios";
            menuUsuarios.Padding = new Padding(10, 0, 0, 0);
            menuUsuarios.Size = new Size(200, 70);
            menuUsuarios.Text = "Usuarios";
            menuUsuarios.TextImageRelation = TextImageRelation.TextBeforeImage;
            menuUsuarios.Click += menuUsuarios_Click;
            // 
            // menuMantenedor
            // 
            menuMantenedor.AutoSize = false;
            menuMantenedor.BackColor = Color.Transparent;
            menuMantenedor.DropDownItems.AddRange(new ToolStripItem[] { subMenuCategoria, subMenuProducto, subMenuNegocio });
            menuMantenedor.Font = new Font("Segoe UI", 12F);
            menuMantenedor.ForeColor = Color.WhiteSmoke;
            menuMantenedor.IconChar = FontAwesome.Sharp.IconChar.Tools;
            menuMantenedor.IconColor = Color.WhiteSmoke;
            menuMantenedor.IconFont = FontAwesome.Sharp.IconFont.Auto;
            menuMantenedor.IconSize = 40;
            menuMantenedor.ImageAlign = ContentAlignment.MiddleLeft;
            menuMantenedor.ImageScaling = ToolStripItemImageScaling.None;
            menuMantenedor.Margin = new Padding(5);
            menuMantenedor.Name = "menuMantenedor";
            menuMantenedor.Padding = new Padding(10, 0, 0, 0);
            menuMantenedor.Size = new Size(200, 70);
            menuMantenedor.Text = "Mantenedor";
            menuMantenedor.TextImageRelation = TextImageRelation.TextBeforeImage;
            // 
            // subMenuCategoria
            // 
            subMenuCategoria.IconChar = FontAwesome.Sharp.IconChar.None;
            subMenuCategoria.IconColor = Color.Black;
            subMenuCategoria.IconFont = FontAwesome.Sharp.IconFont.Auto;
            subMenuCategoria.Name = "subMenuCategoria";
            subMenuCategoria.Size = new Size(147, 26);
            subMenuCategoria.Text = "Categoria";
            subMenuCategoria.Click += subMenuCategoria_Click;
            // 
            // subMenuProducto
            // 
            subMenuProducto.IconChar = FontAwesome.Sharp.IconChar.None;
            subMenuProducto.IconColor = Color.Black;
            subMenuProducto.IconFont = FontAwesome.Sharp.IconFont.Auto;
            subMenuProducto.Name = "subMenuProducto";
            subMenuProducto.Size = new Size(147, 26);
            subMenuProducto.Text = "Producto";
            subMenuProducto.Click += subMenuProducto_Click;
            // 
            // subMenuNegocio
            // 
            subMenuNegocio.IconChar = FontAwesome.Sharp.IconChar.None;
            subMenuNegocio.IconColor = Color.Black;
            subMenuNegocio.IconFont = FontAwesome.Sharp.IconFont.Auto;
            subMenuNegocio.Name = "subMenuNegocio";
            subMenuNegocio.Size = new Size(147, 26);
            subMenuNegocio.Text = "Negocio";
            subMenuNegocio.Click += subMenuNegocio_Click;
            // 
            // menuVentas
            // 
            menuVentas.AutoSize = false;
            menuVentas.BackColor = Color.Transparent;
            menuVentas.DropDownItems.AddRange(new ToolStripItem[] { subMenuRegistrarVenta, subMenuVerDetalleVenta });
            menuVentas.Font = new Font("Segoe UI", 12F);
            menuVentas.ForeColor = Color.WhiteSmoke;
            menuVentas.IconChar = FontAwesome.Sharp.IconChar.CartShopping;
            menuVentas.IconColor = Color.WhiteSmoke;
            menuVentas.IconFont = FontAwesome.Sharp.IconFont.Auto;
            menuVentas.IconSize = 40;
            menuVentas.ImageAlign = ContentAlignment.MiddleLeft;
            menuVentas.ImageScaling = ToolStripItemImageScaling.None;
            menuVentas.Margin = new Padding(5);
            menuVentas.Name = "menuVentas";
            menuVentas.Padding = new Padding(10, 0, 0, 0);
            menuVentas.Size = new Size(200, 70);
            menuVentas.Text = "Ventas";
            menuVentas.TextImageRelation = TextImageRelation.TextBeforeImage;
            // 
            // subMenuRegistrarVenta
            // 
            subMenuRegistrarVenta.IconChar = FontAwesome.Sharp.IconChar.None;
            subMenuRegistrarVenta.IconColor = Color.Black;
            subMenuRegistrarVenta.IconFont = FontAwesome.Sharp.IconFont.Auto;
            subMenuRegistrarVenta.Name = "subMenuRegistrarVenta";
            subMenuRegistrarVenta.Size = new Size(155, 26);
            subMenuRegistrarVenta.Text = "Registrar";
            subMenuRegistrarVenta.Click += subMenuRegistrarVenta_Click;
            // 
            // subMenuVerDetalleVenta
            // 
            subMenuVerDetalleVenta.IconChar = FontAwesome.Sharp.IconChar.None;
            subMenuVerDetalleVenta.IconColor = Color.Black;
            subMenuVerDetalleVenta.IconFont = FontAwesome.Sharp.IconFont.Auto;
            subMenuVerDetalleVenta.Name = "subMenuVerDetalleVenta";
            subMenuVerDetalleVenta.Size = new Size(155, 26);
            subMenuVerDetalleVenta.Text = "Ver Detalle";
            subMenuVerDetalleVenta.Click += subMenuVerDetalleVenta_Click;
            // 
            // menuCompras
            // 
            menuCompras.AutoSize = false;
            menuCompras.BackColor = Color.Transparent;
            menuCompras.DropDownItems.AddRange(new ToolStripItem[] { subMenuRegistrarCompra, subMenuVerDetalleCompra });
            menuCompras.Font = new Font("Segoe UI", 12F);
            menuCompras.ForeColor = Color.WhiteSmoke;
            menuCompras.IconChar = FontAwesome.Sharp.IconChar.Truck;
            menuCompras.IconColor = Color.WhiteSmoke;
            menuCompras.IconFont = FontAwesome.Sharp.IconFont.Auto;
            menuCompras.IconSize = 40;
            menuCompras.ImageAlign = ContentAlignment.MiddleLeft;
            menuCompras.ImageScaling = ToolStripItemImageScaling.None;
            menuCompras.Margin = new Padding(5);
            menuCompras.Name = "menuCompras";
            menuCompras.Padding = new Padding(10, 0, 0, 0);
            menuCompras.Size = new Size(200, 70);
            menuCompras.Text = "Compras";
            menuCompras.TextImageRelation = TextImageRelation.TextBeforeImage;
            // 
            // subMenuRegistrarCompra
            // 
            subMenuRegistrarCompra.IconChar = FontAwesome.Sharp.IconChar.None;
            subMenuRegistrarCompra.IconColor = Color.Black;
            subMenuRegistrarCompra.IconFont = FontAwesome.Sharp.IconFont.Auto;
            subMenuRegistrarCompra.Name = "subMenuRegistrarCompra";
            subMenuRegistrarCompra.Size = new Size(155, 26);
            subMenuRegistrarCompra.Text = "Registrar";
            subMenuRegistrarCompra.Click += subMenuRegistrarCompra_Click;
            // 
            // subMenuVerDetalleCompra
            // 
            subMenuVerDetalleCompra.IconChar = FontAwesome.Sharp.IconChar.None;
            subMenuVerDetalleCompra.IconColor = Color.Black;
            subMenuVerDetalleCompra.IconFont = FontAwesome.Sharp.IconFont.Auto;
            subMenuVerDetalleCompra.Name = "subMenuVerDetalleCompra";
            subMenuVerDetalleCompra.Size = new Size(155, 26);
            subMenuVerDetalleCompra.Text = "Ver Detalle";
            subMenuVerDetalleCompra.Click += subMenuVerDetalleCompra_Click;
            // 
            // menuClientes
            // 
            menuClientes.AutoSize = false;
            menuClientes.BackColor = Color.Transparent;
            menuClientes.Font = new Font("Segoe UI", 12F);
            menuClientes.ForeColor = Color.WhiteSmoke;
            menuClientes.IconChar = FontAwesome.Sharp.IconChar.UserTag;
            menuClientes.IconColor = Color.WhiteSmoke;
            menuClientes.IconFont = FontAwesome.Sharp.IconFont.Auto;
            menuClientes.IconSize = 40;
            menuClientes.ImageAlign = ContentAlignment.MiddleLeft;
            menuClientes.ImageScaling = ToolStripItemImageScaling.None;
            menuClientes.Margin = new Padding(5);
            menuClientes.Name = "menuClientes";
            menuClientes.Padding = new Padding(10, 0, 0, 0);
            menuClientes.Size = new Size(200, 70);
            menuClientes.Text = "Clientes";
            menuClientes.TextImageRelation = TextImageRelation.TextBeforeImage;
            menuClientes.Click += menuClientes_Click;
            // 
            // menuProveedores
            // 
            menuProveedores.AutoSize = false;
            menuProveedores.BackColor = Color.Transparent;
            menuProveedores.Font = new Font("Segoe UI", 12F);
            menuProveedores.ForeColor = Color.WhiteSmoke;
            menuProveedores.IconChar = FontAwesome.Sharp.IconChar.Handshake;
            menuProveedores.IconColor = Color.WhiteSmoke;
            menuProveedores.IconFont = FontAwesome.Sharp.IconFont.Auto;
            menuProveedores.IconSize = 40;
            menuProveedores.ImageAlign = ContentAlignment.MiddleLeft;
            menuProveedores.ImageScaling = ToolStripItemImageScaling.None;
            menuProveedores.Margin = new Padding(5);
            menuProveedores.Name = "menuProveedores";
            menuProveedores.Padding = new Padding(10, 0, 0, 0);
            menuProveedores.Size = new Size(200, 70);
            menuProveedores.Text = "Proveedores";
            menuProveedores.TextImageRelation = TextImageRelation.TextBeforeImage;
            menuProveedores.Click += menuProveedores_Click;
            // 
            // menuReportes
            // 
            menuReportes.AutoSize = false;
            menuReportes.BackColor = Color.Transparent;
            menuReportes.DropDownItems.AddRange(new ToolStripItem[] { subMenuReporteVentas, subMenuReporteCompras, subMenuReporteStock, subMenuReporteResumen });
            menuReportes.Font = new Font("Segoe UI", 12F);
            menuReportes.ForeColor = Color.WhiteSmoke;
            menuReportes.IconChar = FontAwesome.Sharp.IconChar.BarChart;
            menuReportes.IconColor = Color.WhiteSmoke;
            menuReportes.IconFont = FontAwesome.Sharp.IconFont.Auto;
            menuReportes.IconSize = 40;
            menuReportes.ImageAlign = ContentAlignment.MiddleLeft;
            menuReportes.ImageScaling = ToolStripItemImageScaling.None;
            menuReportes.Margin = new Padding(5);
            menuReportes.Name = "menuReportes";
            menuReportes.Padding = new Padding(10, 0, 0, 0);
            menuReportes.Size = new Size(200, 70);
            menuReportes.Text = "Reportes";
            menuReportes.TextImageRelation = TextImageRelation.TextBeforeImage;
            // 
            // subMenuReporteVentas
            // 
            subMenuReporteVentas.Name = "subMenuReporteVentas";
            subMenuReporteVentas.Size = new Size(202, 26);
            subMenuReporteVentas.Text = "Reporte Ventas";
            subMenuReporteVentas.Click += subMenuReporteVentas_Click;
            // 
            // subMenuReporteCompras
            // 
            subMenuReporteCompras.Name = "subMenuReporteCompras";
            subMenuReporteCompras.Size = new Size(202, 26);
            subMenuReporteCompras.Text = "Reporte Compras";
            subMenuReporteCompras.Click += subMenuReporteCompras_Click;
            // 
            // subMenuReporteStock
            // 
            subMenuReporteStock.Name = "subMenuReporteStock";
            subMenuReporteStock.Size = new Size(202, 26);
            subMenuReporteStock.Text = "Reporte Stock";
            subMenuReporteStock.Click += subMenuReporteStock_Click;
            // 
            // subMenuReporteResumen
            // 
            subMenuReporteResumen.Name = "subMenuReporteResumen";
            subMenuReporteResumen.Size = new Size(202, 26);
            subMenuReporteResumen.Text = "Resumen";
            subMenuReporteResumen.Click += subMenuReporteResumen_Click;
            // 
            // menuAcercaDe
            // 
            menuAcercaDe.AutoSize = false;
            menuAcercaDe.BackColor = Color.Transparent;
            menuAcercaDe.Font = new Font("Segoe UI", 12F);
            menuAcercaDe.ForeColor = Color.WhiteSmoke;
            menuAcercaDe.IconChar = FontAwesome.Sharp.IconChar.CircleInfo;
            menuAcercaDe.IconColor = Color.WhiteSmoke;
            menuAcercaDe.IconFont = FontAwesome.Sharp.IconFont.Auto;
            menuAcercaDe.IconSize = 40;
            menuAcercaDe.ImageAlign = ContentAlignment.MiddleLeft;
            menuAcercaDe.ImageScaling = ToolStripItemImageScaling.None;
            menuAcercaDe.Margin = new Padding(5);
            menuAcercaDe.Name = "menuAcercaDe";
            menuAcercaDe.Padding = new Padding(10, 0, 0, 0);
            menuAcercaDe.Size = new Size(200, 70);
            menuAcercaDe.Text = "Acerca de";
            menuAcercaDe.TextImageRelation = TextImageRelation.TextBeforeImage;
            menuAcercaDe.Click += menuAcercaDe_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(37, 37, 38);
            panel1.Controls.Add(btnSalir);
            panel1.Controls.Add(lbUsuario);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(guna2PictureBox1);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1426, 109);
            panel1.TabIndex = 1;
            panel1.Paint += panel1_Paint;
            // 
            // lbUsuario
            // 
            lbUsuario.AutoSize = true;
            lbUsuario.BackColor = Color.Transparent;
            lbUsuario.ForeColor = Color.Gainsboro;
            lbUsuario.Location = new Point(1367, 88);
            lbUsuario.Name = "lbUsuario";
            lbUsuario.Size = new Size(47, 15);
            lbUsuario.TabIndex = 4;
            lbUsuario.Text = "Usuario";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.ForeColor = Color.Gainsboro;
            label2.Location = new Point(1311, 88);
            label2.Name = "label2";
            label2.Size = new Size(50, 15);
            label2.TabIndex = 3;
            label2.Text = "Usuario:";
            // 
            // guna2PictureBox1
            // 
            guna2PictureBox1.BackColor = Color.Transparent;
            guna2PictureBox1.CustomizableEdges = customizableEdges2;
            guna2PictureBox1.Image = (Image)resources.GetObject("guna2PictureBox1.Image");
            guna2PictureBox1.ImageRotate = 0F;
            guna2PictureBox1.Location = new Point(12, 3);
            guna2PictureBox1.Name = "guna2PictureBox1";
            guna2PictureBox1.ShadowDecoration.CustomizableEdges = customizableEdges3;
            guna2PictureBox1.Size = new Size(102, 100);
            guna2PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            guna2PictureBox1.TabIndex = 3;
            guna2PictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("PMingLiU-ExtB", 30F, FontStyle.Italic);
            label1.ForeColor = Color.Gainsboro;
            label1.Location = new Point(120, 34);
            label1.Name = "label1";
            label1.Size = new Size(300, 57);
            label1.TabIndex = 2;
            label1.Text = "NovaSales - ERP";
            // 
            // contenedor
            // 
            contenedor.BackColor = Color.FromArgb(30, 30, 30);
            contenedor.Dock = DockStyle.Fill;
            contenedor.Location = new Point(208, 109);
            contenedor.Name = "contenedor";
            contenedor.Size = new Size(1218, 647);
            contenedor.TabIndex = 2;
            // 
            // btnSalir
            // 
            btnSalir.Animated = true;
            btnSalir.BackColor = Color.Transparent;
            btnSalir.Cursor = Cursors.Hand;
            btnSalir.DisabledState.BorderColor = Color.DarkGray;
            btnSalir.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSalir.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSalir.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSalir.FillColor = Color.White;
            btnSalir.Font = new Font("Segoe UI", 9F);
            btnSalir.ForeColor = Color.White;
            btnSalir.HoverState.FillColor = Color.Red;
            btnSalir.Image = CapaPresentacion.Properties.Resources.x_button;
            btnSalir.ImageSize = new Size(38, 38);
            btnSalir.Location = new Point(1374, 12);
            btnSalir.Name = "btnSalir";
            btnSalir.ShadowDecoration.CustomizableEdges = customizableEdges1;
            btnSalir.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            btnSalir.Size = new Size(40, 40);
            btnSalir.TabIndex = 32;
            btnSalir.Click += btnSalir_Click;
            // 
            // Inicio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(18, 18, 18);
            ClientSize = new Size(1426, 756);
            Controls.Add(contenedor);
            Controls.Add(menuButtons);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            MainMenuStrip = menuButtons;
            Name = "Inicio";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Inicio_Load;
            menuButtons.ResumeLayout(false);
            menuButtons.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private MenuStrip menuButtons;
        private Panel panel1;
        private Label label1;
        private FontAwesome.Sharp.IconMenuItem menuAcercaDe;
        private FontAwesome.Sharp.IconMenuItem menuUsuarios;
        private FontAwesome.Sharp.IconMenuItem menuMantenedor;
        private FontAwesome.Sharp.IconMenuItem menuVentas;
        private FontAwesome.Sharp.IconMenuItem menuCompras;
        private FontAwesome.Sharp.IconMenuItem menuClientes;
        private FontAwesome.Sharp.IconMenuItem menuProveedores;
        private FontAwesome.Sharp.IconMenuItem menuReportes;
        private Panel contenedor;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Label label2;
        private Label lbUsuario;
        private FontAwesome.Sharp.IconMenuItem subMenuCategoria;
        private FontAwesome.Sharp.IconMenuItem subMenuProducto;
        private FontAwesome.Sharp.IconMenuItem subMenuRegistrarVenta;
        private FontAwesome.Sharp.IconMenuItem subMenuVerDetalleVenta;
        private FontAwesome.Sharp.IconMenuItem subMenuRegistrarCompra;
        private FontAwesome.Sharp.IconMenuItem subMenuVerDetalleCompra;
        private FontAwesome.Sharp.IconMenuItem subMenuNegocio;
        private ToolStripMenuItem subMenuReporteVentas;
        private ToolStripMenuItem subMenuReporteCompras;
        private ToolStripMenuItem subMenuReporteStock;
        private ToolStripMenuItem subMenuReporteResumen;
        private Guna.UI2.WinForms.Guna2CircleButton btnSalir;
    }
}
