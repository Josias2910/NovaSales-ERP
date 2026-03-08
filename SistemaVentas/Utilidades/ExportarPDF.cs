using CapaNegocio.DTOs;
using CapaNegocio.Services; // Ajusta según tu proyecto
using CapaPresentacion.Utilidades;
using QuestPDF.Fluent;
using System.Windows.Forms;
using SistemaVentas.Data;

namespace CapaPresentacion.Utilidades
{
    public static class ExportarPDF
    {
        public static void DescargarCompra(CompraDetalleDto compra)
        {
            if (compra == null) return;

            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "PDF Files|*.pdf", FileName = $"Compra_{compra.NumeroDocumento}.pdf" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var datosNegocio = new NegocioService(new AppDbContext()).ListarDatos();

                        var documento = GeneradorReporte.CrearTicketCompra(datosNegocio, compra);
                        documento.GeneratePdf(sfd.FileName);

                        MessageBox.Show("PDF generado correctamente", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al generar PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public static void DescargarCompra(CompraCreateDto compra)
        {
            if (compra == null) return;

            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "PDF Files|*.pdf", FileName = $"Compra_{compra.NumeroDocumento}.pdf" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var datosNegocio = new NegocioService(new AppDbContext()).ListarDatos();

                        var documento = GeneradorReporte.CrearTicketCompra(datosNegocio, compra);
                        documento.GeneratePdf(sfd.FileName);

                        MessageBox.Show("PDF generado correctamente", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al generar PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public static void DescargarVenta(VentaCreateDto venta)
        {
            if (venta == null) return;

            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "PDF Files|*.pdf", FileName = $"Venta_{venta.NumeroDocumento}.pdf" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var datosNegocio = new NegocioService(new AppDbContext()).ListarDatos();

                        var documento = GeneradorReporte.CrearTicketVenta(datosNegocio, venta);
                        documento.GeneratePdf(sfd.FileName);

                        MessageBox.Show("PDF generado correctamente", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al generar PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public static void DescargarVenta(VentaDetalleDto venta)
        {
            if (venta == null) return;

            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "PDF Files|*.pdf", FileName = $"Compra_{venta.NumeroDocumento}.pdf" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var datosNegocio = new NegocioService(new AppDbContext()).ListarDatos();

                        var documento = GeneradorReporte.CrearTicketVenta(datosNegocio, venta);
                        documento.GeneratePdf(sfd.FileName);

                        MessageBox.Show("PDF generado correctamente", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al generar PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}