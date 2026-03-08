using CapaNegocio.Services;
using SistemaVentas.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CapaPresentacion.Forms
{
    public partial class mdVerProducto : Form
    {
        private string _codigo;
        public mdVerProducto(string codigoProducto)
        {
            InitializeComponent();
            _codigo = codigoProducto;
        }

        private void mdVerProducto_Load(object sender, EventArgs e)
        {
            CargarDatosProducto();
        }

        private void CargarDatosProducto()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var service = new ProductoService(context);
                    var p = service.GetByCodigo(_codigo); // Necesitas este método que traiga todo el objeto

                    if (p != null)
                    {
                        tbCategoriaProducto.Text = p.Categoria;
                        tbCodigoProducto.Text = p.Codigo;
                        tbStockProducto.Text = p.Stock.ToString();
                        tbPrecioProducto.Text = $"$ {p.PrecioVenta:N2}";

                        if (p.Imagen != null && p.Imagen.Length > 0)
                        {
                            using (var ms = new MemoryStream(p.Imagen))
                            {
                                picImagenProducto.Image = Image.FromStream(ms);
                            }
                        }
                        else
                        {
                            picImagenProducto.Image = null; // O null
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
