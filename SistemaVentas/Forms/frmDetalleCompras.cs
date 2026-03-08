using CapaNegocio.DTOs;
using CapaNegocio.Services;
using DocumentFormat.OpenXml.InkML;
using QuestPDF.Fluent;
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
    public partial class frmDetalleCompras : Form
    {
        private CompraDetalleDto compraActual;
        public frmDetalleCompras()
        {
            InitializeComponent();
        }
        private void frmDetalleCompras_Load(object sender, EventArgs e)
        {
            dgvDetalleCompra.AutoGenerateColumns = false;
            CargarPuntosVenta();
        }
        private void dgvDetalleCompra_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string codigo = dgvDetalleCompra.Rows[e.RowIndex].Cells["Codigo"].Value.ToString();

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
                var service = new CompraService(context);
                var listaPuntosVenta = service.ObtenerPuntosDeVenta()
                                              .Distinct()
                                              .OrderBy(x => x)
                                              .ToList();
                cbxPuntoVenta.DataSource = listaPuntosVenta;
            }

            btnExportarPDF.Enabled = false;
            btnExportarExcel.Enabled = false;
        }
        private void btnBuscarCompra_Click(object sender, EventArgs e)
        {
            if (cbxPuntoVenta.SelectedItem == null || string.IsNullOrWhiteSpace(tbNumeroDocumento.Text))
            {
                MessageBox.Show("Complete el Punto de Venta y el Número de Documento.");
                return;
            }

            string nroBuscar = $"{cbxPuntoVenta.Text}-{tbNumeroDocumento.Text.PadLeft(8, '0')}";

            using (var context = new AppDbContext())
            {
                var service = new CompraService(context);
                var resultado = service.ObtenerDetalle(nroBuscar);

                if (resultado != null)
                {
                    tbDetalleFecha.Text = resultado.FechaRegistro;
                    tbDetalleTipoDoc.Text = resultado.TipoDocumento;
                    tbDetalleUsuario.Text = resultado.UsuarioNombre;
                    tbDetalleMetodoPago.Text = resultado.MetodoPago;
                    tbDetalleNroDocProv.Text = resultado.DocumentoProveedor;
                    tbDetalleRazonSocialProv.Text = resultado.RazonSocial;
                    tbMontoTotal.Text = resultado.MontoTotal.ToString("N2");

                    compraActual = resultado;

                    dgvDetalleCompra.Rows.Clear();
                    foreach (var item in resultado.Detalles)
                    {
                        dgvDetalleCompra.Rows.Add(new object[] {
                        item.ProductoNombre,
                        item.PrecioCompra.ToString("0.00"),
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
            tbDetalleNroDocProv.Text = "";
            tbDetalleRazonSocialProv.Text = "";
            tbMontoTotal.Text = "0.00";
            dgvDetalleCompra.Rows.Clear();
        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            Utilidades.ExportarPDF.DescargarCompra(compraActual);
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            Utilidades.ExportarExcel.Descargar(dgvDetalleCompra, $"Compra_{compraActual.NumeroDocumento}.xlsx");
        }
    }
}
