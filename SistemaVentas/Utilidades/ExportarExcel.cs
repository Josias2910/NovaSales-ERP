using CapaNegocio.DTOs.CapaNegocio.DTOs.Reportes;
using ClosedXML.Excel;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacion.Utilidades
{
    public static class ExportarExcel
    {
        public static void Descargar(DataGridView dgv, string nombreArchivo)
        {
            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFile = new SaveFileDialog
            {
                FileName = $"{nombreArchivo}_{DateTime.Now:yyyyMMdd}.xlsx",
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Reporte");

                        int colVisibleIndex = 1;
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            if (dgv.Columns[i].Visible && !string.IsNullOrEmpty(dgv.Columns[i].HeaderText))
                            {
                                var cell = worksheet.Cell(1, colVisibleIndex);
                                cell.Value = dgv.Columns[i].HeaderText;
                                cell.Style.Font.Bold = true;
                                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#D4AF37");
                                colVisibleIndex++;
                            }
                        }

                        for (int r = 0; r < dgv.Rows.Count; r++)
                        {
                            colVisibleIndex = 1;
                            for (int c = 0; c < dgv.Columns.Count; c++)
                            {
                                if (dgv.Columns[c].Visible && !string.IsNullOrEmpty(dgv.Columns[c].HeaderText))
                                {
                                    worksheet.Cell(r + 2, colVisibleIndex).Value = dgv.Rows[r].Cells[c].Value?.ToString();
                                    colVisibleIndex++;
                                }
                            }
                        }

                        worksheet.Columns().AdjustToContents();
                        workbook.SaveAs(saveFile.FileName);
                    }
                    MessageBox.Show("Reporte generado con éxito.", "NovaSales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public static void DescargarResumen(ReporteResumenDto datos, string rangoFechas)
        {
            SaveFileDialog saveFile = new SaveFileDialog
            {
                FileName = $"Resumen_NovaSales_{DateTime.Now:yyyyMMdd}.xlsx",
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("Resumen General");

                        ws.Cell("A1").Value = "SISTEMA DE VENTAS - NOVASALES";
                        ws.Range("A1:F1").Merge().Style.Font.Bold = true;
                        ws.Range("A1:F1").Style.Font.FontSize = 16;
                        ws.Range("A1:F1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Cell("A2").Value = $"REPORTE DE RESULTADOS: {rangoFechas}";
                        ws.Range("A2:F2").Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Range("A2:F2").Style.Font.FontColor = XLColor.Gray;

                        var headerStyle = workbook.Style;
                        headerStyle.Font.Bold = true;
                        headerStyle.Fill.BackgroundColor = XLColor.FromHtml("#1E1E1E");
                        headerStyle.Font.FontColor = XLColor.White;

                        ws.Cell("A4").Value = "RESUMEN DE VENTAS";
                        ws.Range("A4:B4").Merge().Style = headerStyle;
                        ws.Cell(5, 1).Value = "Total Ventas:"; ws.Cell(5, 2).Value = datos.TotalVentas;
                        ws.Cell(6, 1).Value = "Cantidad:"; ws.Cell(6, 2).Value = datos.CantidadVentas;
                        ws.Cell(7, 1).Value = "Var. Ventas:"; ws.Cell(7, 2).Value = (datos.VariacionVentas / 100);
                        ws.Cell(8, 1).Value = "Día Pico:"; ws.Cell(8, 2).Value = datos.DiaPicoVenta;

                        ws.Cell("A10").Value = "RENTABILIDAD";
                        ws.Range("A10:B10").Merge().Style = headerStyle;
                        ws.Cell(11, 1).Value = "Ganancia Neta:"; ws.Cell(11, 2).Value = datos.GananciaNeta;
                        ws.Cell(12, 1).Value = "ROI:"; ws.Cell(12, 2).Value = (datos.PorcentajeROI / 100);
                        ws.Cell(13, 1).Value = "Top Producto:"; ws.Cell(13, 2).Value = datos.ProductoMasVendido;

                        ws.Cell("D4").Value = "RESUMEN DE COMPRAS";
                        ws.Range("D4:E4").Merge().Style = headerStyle;
                        ws.Cell(5, 4).Value = "Total Compras:"; ws.Cell(5, 5).Value = datos.TotalCompras;
                        ws.Cell(6, 4).Value = "Facturas:"; ws.Cell(6, 5).Value = datos.CantidadCompras;
                        ws.Cell(7, 4).Value = "Bajo Stock:"; ws.Cell(7, 5).Value = datos.ProductosBajoStock;
                        ws.Cell(8, 4).Value = "Prov. Principal:"; ws.Cell(8, 5).Value = datos.ProveedorPrincipal;

                        ws.Cell("D10").Value = "CLIENTES Y TICKET";
                        ws.Range("D10:E10").Merge().Style = headerStyle;
                        ws.Cell(11, 4).Value = "Ticket Promedio:"; ws.Cell(11, 5).Value = datos.PromedioTicketVenta;
                        ws.Cell(12, 4).Value = "Clientes Únicos:"; ws.Cell(12, 5).Value = datos.CantidadClientes;
                        ws.Cell(13, 4).Value = "Pago Preferido:"; ws.Cell(13, 5).Value = datos.MetodoPagoPreferido;

                        ws.Cell("A16").Value = "EVOLUCIÓN DIARIA (DATOS DEL GRÁFICO)";
                        ws.Range("A16:C16").Merge().Style = headerStyle;
                        ws.Range("A16:C16").Style.Fill.BackgroundColor = XLColor.FromHtml("#D4AF37");

                        ws.Cell(17, 1).Value = "FECHA";
                        ws.Cell(17, 2).Value = "VENTAS";
                        ws.Cell(17, 3).Value = "COMPRAS";
                        ws.Range("A17:C17").Style.Font.Bold = true;

                        for (int i = 0; i < datos.EtiquetasDias.Count; i++)
                        {
                            int fila = 18 + i;
                            ws.Cell(fila, 1).Value = datos.EtiquetasDias[i];
                            ws.Cell(fila, 2).Value = datos.VentasPorDia[i];
                            ws.Cell(fila, 3).Value = datos.ComprasPorDia[i];
                        }

                        ws.Range("B5:B5").Style.NumberFormat.Format = "$ #,##0.00";
                        ws.Range("B11:B11").Style.NumberFormat.Format = "$ #,##0.00";
                        ws.Range("E5:E5").Style.NumberFormat.Format = "$ #,##0.00";
                        ws.Range("E11:E11").Style.NumberFormat.Format = "$ #,##0.00";
                        ws.Range("B18:C" + (18 + datos.EtiquetasDias.Count)).Style.NumberFormat.Format = "$ #,##0.00";

                        ws.Cell(7, 2).Style.NumberFormat.Format = "0.0%";
                        ws.Cell(12, 2).Style.NumberFormat.Format = "0.0%";

                        ws.Columns().AdjustToContents();
                        workbook.SaveAs(saveFile.FileName);
                    }
                    MessageBox.Show("Excel generado con éxito con todos los datos del Dashboard.", "NovaSales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}