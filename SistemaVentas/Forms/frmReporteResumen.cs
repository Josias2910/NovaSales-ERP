using CapaNegocio.DTOs.CapaNegocio.DTOs.Reportes;
using CapaNegocio.Services;
using SistemaVentas.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;

namespace CapaPresentacion.Forms
{
    public partial class frmReporteResumen : Form
    {
        private ReporteResumenDto _ultimosDatosCargados;
        public frmReporteResumen()
        {
            InitializeComponent();
            ConfigurarGraficoInicial();
        }
        private void frmReporteResumen_Load(object sender, EventArgs e)
        {
            DateTime hoy = DateTime.Now;
            dtpFechaInicio.Value = new DateTime(hoy.Year, hoy.Month, 1);
            dtpFechaFin.Value = hoy;
            CargarDashboard();
        }
        private void btnLimpiarData_Click(object sender, EventArgs e)
        {
            LimpiarDashboard();
        }
        private void LimpiarDashboard()
        {
            flowCompras.Controls.Clear();
            flowGanancia.Controls.Clear();
            flowVentas.Controls.Clear();
            flowTicket.Controls.Clear();
            chartReporte.Series.Clear();
        }
        private void CargarDatosAlGrafico(ReporteResumenDto datos)
        {
            Func<ChartPoint, string> formateador = point => $"{point.Y:C2}";
            chartReporte.Series.Clear();

            chartReporte.Series.Add(new LineSeries
            {
                Title = "Ventas",
                Values = new ChartValues<decimal>(datos.VentasPorDia),
                LabelPoint = formateador,
                Stroke = System.Windows.Media.Brushes.LimeGreen,
                PointGeometrySize = 10,
                Fill = new System.Windows.Media.LinearGradientBrush
                {
                    StartPoint = new System.Windows.Point(0, 0),
                    EndPoint = new System.Windows.Point(0, 1),
                    GradientStops = new System.Windows.Media.GradientStopCollection {
                    new System.Windows.Media.GradientStop(System.Windows.Media.Color.FromArgb(50, 50, 205, 50), 0),
                    new System.Windows.Media.GradientStop(System.Windows.Media.Colors.Transparent, 1)
                    }
                }
            });

            chartReporte.Series.Add(new LineSeries
            {
                Title = "Compras",
                Values = new ChartValues<decimal>(datos.ComprasPorDia),
                LabelPoint = formateador,
                Stroke = System.Windows.Media.Brushes.Tomato,
                PointGeometrySize = 8,
                Fill = System.Windows.Media.Brushes.Transparent 
            });

            chartReporte.AxisX[0].Labels = datos.EtiquetasDias;
        }
        private void ConfigurarGraficoInicial()
        {
            chartReporte.BackColor = Color.FromArgb(30, 30, 30);
            chartReporte.ForeColor = Color.White;

            chartReporte.LegendLocation = LegendLocation.None;

            chartReporte.AxisY.Add(new Axis
            {
                Foreground = System.Windows.Media.Brushes.Gray,
                Separator = new Separator { Stroke = System.Windows.Media.Brushes.DimGray }
            });

            chartReporte.AxisX.Add(new Axis
            {
                Foreground = System.Windows.Media.Brushes.Gray,
                Separator = new Separator { IsEnabled = false }
            });
        }
        private void LlenarTarjetaDinamica(FlowLayoutPanel fp, string titulo, string valor, Color colorValor, List<string> detalles)
        {
            fp.Controls.Clear();
            fp.FlowDirection = FlowDirection.TopDown;
            fp.WrapContents = false;
            fp.Padding = new Padding(10, 5, 10, 5);

            fp.Controls.Add(new Label
            {
                Text = titulo.ToUpper(),
                ForeColor = Color.DarkGray,
                Font = new Font("Segoe UI", 7, FontStyle.Bold),
                AutoSize = true
            });

            fp.Controls.Add(new Label
            {
                Text = valor,
                ForeColor = colorValor,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 5, 0, 10)
            });

            Panel line = new Panel
            {
                Size = new Size(fp.Width - 25, 1),
                BackColor = Color.FromArgb(60, 60, 60),
                Margin = new Padding(0, 0, 0, 8)
            };
            fp.Controls.Add(line);

            foreach (var d in detalles)
            {
                fp.Controls.Add(new Label
                {
                    Text = "• " + d,
                    ForeColor = Color.Silver,
                    Font = new Font("Segoe UI", 8, FontStyle.Regular),
                    AutoSize = true,
                    Margin = new Padding(0, 0, 0, 3)
                });
            }
        }
        private void CargarDashboard()
        {
            if (dtpFechaInicio.Value.Date > dtpFechaFin.Value.Date)
            {
                MessageBox.Show("La fecha de inicio no puede ser mayor a la fecha fin.",
                                "Rango de Fechas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;

                using (var context = new AppDbContext())
                {
                    var service = new ReporteService(context);
                    var datos = service.ObtenerResumen(dtpFechaInicio.Value.Date, dtpFechaFin.Value.Date);
                    _ultimosDatosCargados = datos;
                    ActualizarInterfaz(datos);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnVerResumen_Click(object sender, EventArgs e)
        {
            CargarDashboard();
        }
        private void ActualizarInterfaz(ReporteResumenDto datos)
        {
            chartReporte.Series.Clear();

            if (chartReporte.AxisX.Count > 0)
            {
                chartReporte.AxisX[0].Labels = datos.EtiquetasDias;
            }

            string FormatearTendencia(decimal variacion)
            {
                string simbolo = variacion >= 0 ? "↑" : "↓";
                return $"{simbolo} {Math.Abs(variacion):N1}% vs período ant.";
            }

            // VENTAS
            LlenarTarjetaDinamica(flowVentas, "Ventas Totales", datos.TotalVentas.ToString("C2"), Color.LimeGreen, new List<string> {
            $"{datos.CantidadVentas} ventas realizadas",
            FormatearTendencia(datos.VariacionVentas),
            $"Pago pref: {datos.MetodoPagoPreferido}",
            $"Pico: {datos.DiaPicoVenta}"
            });

            // COMPRAS
            LlenarTarjetaDinamica(flowCompras, "Compras Totales", datos.TotalCompras.ToString("C2"), Color.Tomato, new List<string> {
            $"{datos.CantidadCompras} facturas de compra",
            FormatearTendencia(datos.VariacionCompras),
            $"Prov. Principal: {datos.ProveedorPrincipal}",
            $"{datos.ProductosBajoStock} productos bajo stock"
            });

            // GANANCIA
            decimal margen = datos.TotalVentas > 0 ? (datos.GananciaNeta / datos.TotalVentas) * 100 : 0;

            string estatusFinanciero = "Estatus: Crítico";
            if (margen > 30) estatusFinanciero = "Estatus: Muy Rentable";
            else if (margen > 10) estatusFinanciero = "Estatus: Rentable";
            else if (margen > 0) estatusFinanciero = "Estatus: Ajustado";
            else if (datos.TotalVentas > 0 && datos.GananciaNeta <= 0) estatusFinanciero = "Estatus: Pérdida";

            LlenarTarjetaDinamica(flowGanancia, "Ganancia Neta", datos.GananciaNeta.ToString("C2"), Color.DeepSkyBlue, new List<string> {
            $"Margen de utilidad: {margen:N1}%",
            $"Top: {datos.ProductoMasVendido}",
            $"ROI: {datos.PorcentajeROI:N1}%",
            estatusFinanciero
            });

            // TICKET
            LlenarTarjetaDinamica(flowTicket, "Ticket Promedio", datos.PromedioTicketVenta.ToString("C2"), Color.Gold, new List<string> {
            $"{datos.CantidadClientes} clientes únicos",
            FormatearTendencia(datos.VariacionTicket),
            $"{datos.ArticulosPorTicket:N1} prod. p/ticket",
            datos.VariacionTicket >= 0 ? "Estado: Creciendo" : "Estado: Revisar precios"
            });

            CargarDatosAlGrafico(datos);
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (_ultimosDatosCargados == null)
            {
                MessageBox.Show("Primero debe ver un resumen para exportar los datos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rango = $"{dtpFechaInicio.Value:dd/MM/yyyy} - {dtpFechaFin.Value:dd/MM/yyyy}";
            Utilidades.ExportarExcel.DescargarResumen(_ultimosDatosCargados, rango);
        }
    }
}
