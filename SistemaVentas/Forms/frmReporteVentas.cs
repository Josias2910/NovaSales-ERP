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
    public partial class frmReporteVentas : Form
    {
        public frmReporteVentas()
        {
            InitializeComponent();
        }

        private void frmReporteVentas_Load(object sender, EventArgs e)
        {
            dgvReporteVentas.AutoGenerateColumns = false;
            CargarMetodosPago();
        }

        private void dgvCategorias_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int idVenta = Convert.ToInt32(dgvReporteVentas.Rows[e.RowIndex].Cells["IdVenta"].Value);

                using (var modal = new mdVerReporteVentas(idVenta))
                {
                    modal.ShowDialog();
                }
            }
        }
        private void CargarMetodosPago()
        {
            cbxMetodoPago.Items.Clear();
            cbxMetodoPago.Items.Add("Todos");
            cbxMetodoPago.Items.Add("Efectivo");
            cbxMetodoPago.Items.Add("Credito");
            cbxMetodoPago.Items.Add("Debito");
            cbxMetodoPago.Items.Add("Transferencia");

            cbxMetodoPago.SelectedIndex = 0;

            cbxMetodoPago.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnVerReporte_Click(object sender, EventArgs e)
        {
            DateTime inicio = dtpFechaInicio.Value.Date;
            DateTime fin = dtpFechaFin.Value.Date;
            string metodo = cbxMetodoPago.SelectedItem.ToString();

            try
            {
                using (var context = new AppDbContext())
                {
                    var service = new ReporteService(context);

                    var lista = service.ObtenerVentas(inicio, fin, metodo);

                    dgvReporteVentas.DataSource = lista;

                    if (lista != null && lista.Count > 0)
                    {
                        decimal totalActual = lista.Sum(v => v.MontoTotal);
                        CalcularResumen(lista);
                        CargarDashboardCompleto(lista, service);
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron ventas en el periodo seleccionado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarDashboard();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el reporte: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CalcularResumen(List<ReporteVentaDto> lista)
        {
            lbMontoTotal.Text = lista.Sum(v => v.MontoTotal).ToString("C2");

            lbVentasRegistradas.Text = lista.Count.ToString();

            var mayorCliente = lista.GroupBy(v => v.NombreCliente)
                                    .OrderByDescending(g => g.Sum(x => x.MontoTotal))
                                    .First().Key;
            lbMayorCompra.Text = mayorCliente;

            var metodoFav = lista.GroupBy(v => v.MetodoPago)
                                 .OrderByDescending(g => g.Count())
                                 .First().Key;
            lbMetodoPago.Text = metodoFav;

            lbGananciaNeta.Text = lista.Sum(v => v.GananciaVenta).ToString("C2");
        }

        private void LimpiarDashboard()
        {
            lbMontoTotal.Text = "$ 0.00";
            lbVentasRegistradas.Text = "0";
            lbMayorCompra.Text = "-";
            lbMetodoPago.Text = "-";
            lbGananciaNeta.Text = "$ 0.00";
        }

        private void btnLimpiarData_Click(object sender, EventArgs e)
        {
            LimpiarDashboard();
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            Utilidades.ExportarExcel.Descargar(dgvReporteVentas, $"ReporteVentas_{DateTime.Now}");
        }

        private void dgvReporteVentas_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvReporteVentas.Rows[e.RowIndex];

            if (row.Selected)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(35, 30, 15);
                row.DefaultCellStyle.ForeColor = Color.FromArgb(212, 175, 55);
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 40, 20);
                row.DefaultCellStyle.SelectionForeColor = Color.FromArgb(255, 215, 0);
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
                row.DefaultCellStyle.ForeColor = Color.White;
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(40, 40, 40);
                row.DefaultCellStyle.SelectionForeColor = Color.FromArgb(160, 160, 160);
            }
        }
        private void CargarDashboardCompleto(List<ReporteVentaDto> lista, ReporteService service)
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown; 
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.Padding = new Padding(10);

            decimal totalMonto = lista.Sum(v => v.MontoTotal);
            decimal ticketPromedio = totalMonto / lista.Count;
            AgregarIndicadorCustom("Ticket Promedio", ticketPromedio.ToString("C2"), Color.FromArgb(212, 175, 55));

            decimal totalMesAnterior = service.ObtenerTotalMesAnterior();
            AgregarComparativaCustom(totalMonto, totalMesAnterior);

            Label lblSep = new Label { Text = "____________________", ForeColor = Color.DimGray, AutoSize = true };
            flowLayoutPanel1.Controls.Add(lblSep);

            decimal promedioCambio = lista.Average(v => v.MontoCambio);
            AgregarIndicadorCustom("Promedio Vuelto", promedioCambio.ToString("C2"), Color.LightGray);
        }

        private void AgregarIndicadorCustom(string titulo, string valor, Color colorValor)
        {
            Label lblTit = new Label { Text = titulo.ToUpper(), AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 8, FontStyle.Bold) };
            Label lblVal = new Label
            {
                Text = valor,
                AutoSize = true,
                ForeColor = colorValor,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 10),
                Cursor = Cursors.Hand 
            };

            ToolTip tt = new ToolTip();
            tt.SetToolTip(lblVal, $"{titulo}: {valor}"); 

            flowLayoutPanel1.Controls.Add(lblTit);
            flowLayoutPanel1.Controls.Add(lblVal);
        }

        private void AgregarComparativaCustom(decimal actual, decimal anterior)
        {
            Label lblTit = new Label { Text = "RENDIMIENTO VS MES ANT.", AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 8, FontStyle.Bold) };
            flowLayoutPanel1.Controls.Add(lblTit);

            if (anterior > 0)
            {
                decimal diferencia = actual - anterior;
                decimal porcentaje = (diferencia / anterior) * 100;
                bool esPositivo = porcentaje >= 0;

                Label lblPorc = new Label
                {
                    Text = $"{(esPositivo ? "+" : "")}{porcentaje:N1}% {(esPositivo ? "▲" : "▼")}",
                    AutoSize = true,
                    ForeColor = esPositivo ? Color.LimeGreen : Color.Tomato,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold)
                };
                flowLayoutPanel1.Controls.Add(lblPorc);
            }
            else
            {
                Label lblInfo = new Label { Text = "Sin datos previos", AutoSize = true, ForeColor = Color.DimGray, Font = new Font("Segoe UI", 9, FontStyle.Italic) };
                flowLayoutPanel1.Controls.Add(lblInfo);
            }
        }
    }
}
