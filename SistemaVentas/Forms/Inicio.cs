using CapaNegocio.DTOs;
using SistemaVentas.Domain.Entities;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using FontAwesome.Sharp;
using CapaPresentacion.Forms;
using CapaDatos;

namespace SistemaVentas
{
    public partial class Inicio : Form
    {
        private UsuarioResponseDto usuarioActual;
        private static IconMenuItem MenuActivo = null;
        private static Form formularioActivo = null;
        public Inicio(UsuarioResponseDto usuario)
        {
            usuarioActual = usuario;
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics mgraphics = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(31, 31, 31), 1);
            Rectangle area = new Rectangle(0, 0, this.Width, this.Height);
            LinearGradientBrush lgb = new LinearGradientBrush(area, Color.FromArgb(45, 45, 48), Color.FromArgb(31, 31, 31), LinearGradientMode.Vertical);
            mgraphics.FillRectangle(lgb, area);
            mgraphics.DrawRectangle(pen, area);
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            lbUsuario.Text = usuarioActual.Nombre;
            AplicarPermisos();
        }
        private void AplicarPermisos()
        {
            menuUsuarios.Visible = false;
            menuMantenedor.Visible = false;
            menuVentas.Visible = false;
            menuCompras.Visible = false;
            menuClientes.Visible = false;
            menuProveedores.Visible = false;
            menuReportes.Visible = false;
            menuAcercaDe.Visible = false;

            foreach (var permiso in usuarioActual.Permisos)
            {
                switch (permiso)
                {
                    case "menuUsuarios":
                        menuUsuarios.Visible = true;
                        break;
                    case "menuMantenedor":
                        menuMantenedor.Visible = true;
                        break;
                    case "menuVentas":
                        menuVentas.Visible = true;
                        break;
                    case "menuCompras":
                        menuCompras.Visible = true;
                        break;
                    case "menuClientes":
                        menuClientes.Visible = true;
                        break;
                    case "menuProveedores":
                        menuProveedores.Visible = true;
                        break;
                    case "menuReportes":
                        menuReportes.Visible = true;
                        break;
                    case "menuAcercaDe":
                        menuAcercaDe.Visible = true;
                        break;
                }
            }
        }
        private void AbrirFormulario(IconMenuItem menu, Form formularioUsuarios)
        {
            if (MenuActivo != null)
            {
                MenuActivo.BackColor = Color.Transparent;
                MenuActivo.IconColor = Color.WhiteSmoke;
                MenuActivo.ForeColor = Color.WhiteSmoke;
            }
            menu.BackColor = Color.FromArgb(45, 45, 48);
            menu.IconColor = Color.FromArgb(212, 175, 55);
            menu.ForeColor = Color.FromArgb(212, 175, 55);
            MenuActivo = menu;

            if (formularioActivo != null)
            {
                formularioActivo.Close();
            }

            formularioActivo = formularioUsuarios;
            formularioUsuarios.TopLevel = false;
            formularioUsuarios.FormBorderStyle = FormBorderStyle.None;
            formularioUsuarios.Dock = DockStyle.Fill;
            formularioUsuarios.BackColor = Color.FromArgb(31, 31, 31);
            contenedor.Controls.Add(formularioUsuarios);
            contenedor.Tag = formularioUsuarios;
            formularioUsuarios.BringToFront();
            formularioUsuarios.Show();
        }
        private void menuUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuUsuarios, new frmUsuarios());
        }

        private void subMenuCategoria_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuMantenedor, new frmCategoria());
        }

        private void subMenuProducto_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuMantenedor, new frmProducto());
        }

        private void subMenuRegistrarVenta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuVentas, new frmVentas());
        }

        private void subMenuVerDetalleVenta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuVentas, new frmDetalleVenta());
        }

        private void subMenuRegistrarCompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuCompras, new frmCompras());
        }

        private void subMenuVerDetalleCompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuCompras, new frmDetalleCompras());
        }

        private void menuClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuClientes, new frmClientes());
        }

        private void menuProveedores_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuProveedores, new frmProveedores());
        }

        private void subMenuNegocio_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuMantenedor, new frmNegocio());
        }

        private void subMenuReporteVentas_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuReportes, new frmReporteVentas());
        }

        private void subMenuReporteCompras_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuMantenedor, new frmReporteCompras());
        }

        private void subMenuReporteStock_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuMantenedor, new frmReporteStock());
        }

        private void subMenuReporteResumen_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuMantenedor, new frmReporteResumen());
        }

        private void menuAcercaDe_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuAcercaDe, new frmAcercaDe());
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
