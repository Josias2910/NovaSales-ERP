using CapaNegocio.Services;
using SistemaVentas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SistemaVentas.Data;

namespace CapaPresentacion.Forms
{
    public partial class mdVerReporteVentas : Form
    {
        private int _idVentaRecibido;
        public mdVerReporteVentas(int idVenta)
        {
            InitializeComponent();
            _idVentaRecibido = idVenta;
        }

        private void mdVerReporteVentas_Load(object sender, EventArgs e)
        {
            dgvDetalleVenta.AutoGenerateColumns = false;
            ObtenerDetallesVenta();
        }
        private void ObtenerDetallesVenta()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var service = new ReporteService(context);
                    var detalle = service.ObtenerDetalleVentaCompleto(_idVentaRecibido);
                    if (detalle != null)
                    {
                        tbVentaDetalleFecha.Text = detalle.FechaRegistro;
                        tbVentaDetalleTipoDoc.Text = detalle.TipoDocumento;
                        tbVentaDetalleNroDoc.Text = detalle.NumeroDocumento;
                        tbVentaDetalleCliente.Text = $"{detalle.DocumentoCliente} - {detalle.NombreCliente}";
                        tbVentaDetalleVendedor.Text = detalle.UsuarioNombre;

                        dgvDetalleVenta.DataSource = detalle.Detalles;

                        lbMontoTotal.Text = detalle.MontoTotal.ToString("C2");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos de venta");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
