using CapaNegocio.DTOs;
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
    public partial class frmDetalleVenta : Form
    {
        private VentaDetalleDto ventaActual;
        public frmDetalleVenta()
        {
            InitializeComponent();
        }

        private void frmDetalleVenta_Load(object sender, EventArgs e)
        {
            dgvDetalleVenta.AutoGenerateColumns = false;
            CargarPuntosVenta();
        }

        private void dgvDetalleVenta_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string codigo = dgvDetalleVenta.Rows[e.RowIndex].Cells["Codigos"].Value.ToString();

                using (var modal = new mdVerProducto(codigo))
                {
                    modal.ShowDialog();
                }
            }
        }

        private void CargarPuntosVenta()
        {
            using (var context = new AppDbContext())
            {
                var service = new VentaService(context);
                var listaPuntosVenta = service.ObtenerPuntosDeVenta()
                                              .Distinct()
                                              .OrderBy(x => x)
                                              .ToList();
                cbxPuntoVenta.DataSource = listaPuntosVenta;
            }

            btnExportarPDF.Enabled = false;
            btnExportarExcel.Enabled = false;
        }

        private void btnBuscarVenta_Click(object sender, EventArgs e)
        {
            if (cbxPuntoVenta.SelectedItem == null || string.IsNullOrWhiteSpace(tbNumeroDocumento.Text))
            {
                MessageBox.Show("Complete el Punto de Venta y el Número de Documento.");
                return;
            }

            string nroBuscar = $"{cbxPuntoVenta.Text}-{tbNumeroDocumento.Text.PadLeft(8, '0')}";

            using (var context = new AppDbContext())
            {
                var service = new VentaService(context);
                var resultado = service.ObtenerDetalle(nroBuscar);

                if (resultado != null)
                {
                    tbDetalleFecha.Text = resultado.FechaRegistro;
                    tbDetalleTipoDoc.Text = resultado.TipoDocumento;
                    tbDetalleUsuario.Text = resultado.UsuarioNombre;
                    tbDetalleMetodoPago.Text = resultado.MetodoPago;
                    tbPagaCon.Text = resultado.MontoPago.ToString("0.00");
                    tbCambio.Text = resultado.MontoCambio.ToString("0.00");
                    tbDocumentoCliente.Text = resultado.DocumentoCliente;
                    tbNombreCliente.Text = resultado.NombreCliente;
                    tbMontoTotal.Text = resultado.MontoTotal.ToString("N2");

                    ventaActual = resultado;

                    dgvDetalleVenta.Rows.Clear();
                    foreach (var item in resultado.Detalles)
                    {
                        dgvDetalleVenta.Rows.Add(new object[] {
                        item.ProductoNombre,
                        item.PrecioVenta.ToString("0.00"),
                        item.Cantidad.ToString(),
                        item.MontoTotal.ToString("0.00"),
                        item.Codigo.ToString()
                        });
                    }

                    btnExportarPDF.Enabled = true;
                    btnExportarExcel.Enabled = true;
                }
                else
                {
                    MessageBox.Show($"No se encontró la compra {nroBuscar}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void btnLimpiarGrilla_Click(object sender, EventArgs e)
        {
            tbNumeroDocumento.Clear();
            LimpiarCampos();
        }
        private void LimpiarCampos()
        {
            tbDetalleFecha.Text = "";
            tbDetalleTipoDoc.Text = "";
            tbDetalleUsuario.Text = "";
            tbDetalleMetodoPago.Text = "";
            tbDocumentoCliente.Text = "";
            tbNombreCliente.Text = "";
            tbMontoTotal.Text = "0.00";
            dgvDetalleVenta.Rows.Clear();
        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            Utilidades.ExportarPDF.DescargarVenta(ventaActual);
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            Utilidades.ExportarExcel.Descargar(dgvDetalleVenta, $"Compra_{ventaActual.NumeroDocumento}.xlsx");
        }
    }
}
