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
    public partial class frmReporteStock : Form
    {
        private List<ReporteStockDto> _listaStock;
        private ToolTip tt = new ToolTip();
        public frmReporteStock()
        {
            InitializeComponent();
        }

        private void frmReporteStock_Load(object sender, EventArgs e)
        {
            dgvReporteStock.AutoGenerateColumns = false;
            CargarDatos();
        }

        private void CargarDatos()
        {
            using (var context = new AppDbContext())
            {
                var service = new ReporteService(context);
                _listaStock = service.ObtenerStock();
                dgvReporteStock.DataSource = _listaStock;
                ActualizarDashboardYResumen(_listaStock);
            }
        }
        private void ActualizarDashboardYResumen(List<ReporteStockDto> listaActual)
        {
            if (listaActual == null) return;

            // --- 1. ACTUALIZAR LABELS PRINCIPALES ---
            lbTotalArticulos.Text = listaActual.Count.ToString();
            lbTotalStock.Text = listaActual.Sum(x => x.MontoTotalStock).ToString("C2");
            lbAgotados.Text = listaActual.Count(x => x.Stock <= 0).ToString();
            lbGananciaPotencial.Text = listaActual.Sum(x => x.GananciaPotencialItem).ToString("C2");

            var topCat = listaActual.GroupBy(x => x.Categoria)
                                    .OrderByDescending(g => g.Count())
                                    .FirstOrDefault();
            lbCategoriaTop.Text = topCat?.Key ?? "-";

            // --- 2. ACTUALIZAR FLOWLAYOUTPANEL (DASHBOARD DINÁMICO) ---
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.AutoScroll = true;

            // Cálculo de Margen Promedio sobre la lista actual
            decimal valorVentaTotal = listaActual.Sum(x => x.PrecioVenta * x.Stock);
            decimal gananciaTotal = listaActual.Sum(x => x.GananciaPotencialItem);
            decimal margenPorcentaje = valorVentaTotal > 0 ? (gananciaTotal / valorVentaTotal) * 100 : 0;

            AgregarIndicadorCustom("Margen Promedio", margenPorcentaje.ToString("N1") + "%", Color.Gold);

            // Alertas de Reposición
            int paraReponer = listaActual.Count(x => x.EsStockBajo && x.Stock > 0);
            AgregarIndicadorCustom("Para Reponer", paraReponer.ToString() + " productos", Color.Orange);

            // Ítem más Caro
            var masCaro = listaActual.OrderByDescending(x => x.PrecioCompra).FirstOrDefault();
            if (masCaro != null)
            {
                string nombreProducto = masCaro.Nombre.Length > 20 ? masCaro.Nombre.Substring(0, 17) + "..." : masCaro.Nombre;
                AgregarIndicadorCustom("Producto más Costoso", nombreProducto, Color.MediumPurple);
            }

            // Espaciador final
            flowLayoutPanel1.Controls.Add(new Label { Text = " ", Height = 20 });
        }
        private void tbBuscarStock_TextChanged(object sender, EventArgs e)
        {
            if (_listaStock == null) return;

            string filtro = tbBuscarStock.Text.Trim().ToUpper();
            var listaFiltrada = _listaStock.Where(p =>
                p.Codigo.ToUpper().Contains(filtro) ||
                p.Nombre.ToUpper().Contains(filtro) ||
                p.Categoria.ToUpper().Contains(filtro)
            ).ToList();
            dgvReporteStock.DataSource = listaFiltrada;
            ActualizarDashboardYResumen(listaFiltrada);
        }

        private void dgvReporteStock_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Supongamos que la columna Stock es la 3
            if (dgvReporteStock.Columns[e.ColumnIndex].Name == "Stock")
            {
                // Obtenemos el objeto completo de la fila actual
                var item = (ReporteStockDto)dgvReporteStock.Rows[e.RowIndex].DataBoundItem;
                if (item.EsStockBajo)
                {
                    e.CellStyle.ForeColor = Color.Tomato; // Texto Rojo/Naranja
                    e.CellStyle.SelectionForeColor = Color.Tomato;
                }
            }
        }

        private void dgvReporteStock_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvReporteStock.Rows[e.RowIndex];

            if (row.Selected)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(35, 30, 15);
                row.DefaultCellStyle.ForeColor = Color.FromArgb(212, 175, 55);
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 40, 20); // Un poco más claro al seleccionar
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
        private void AgregarIndicadorCustom(string titulo, string valor, Color colorValor)
        {
            Label lblTit = new Label
            {
                Text = titulo.ToUpper(),
                AutoSize = true,
                ForeColor = Color.DarkGray,
                Font = new Font("Segoe UI", 7, FontStyle.Bold) // Un punto más chica para que no sature
            };

            Label lblVal = new Label
            {
                Text = valor,
                AutoSize = true,
                ForeColor = colorValor,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 12),
                Cursor = Cursors.Hand
            };

            tt.SetToolTip(lblVal, $"{titulo}: {valor}");

            flowLayoutPanel1.Controls.Add(lblTit);
            flowLayoutPanel1.Controls.Add(lblVal);
        }
        private void LimpiarDashboard()
        {
            lbAgotados.Text = "$ 0.00";
            lbCategoriaTop.Text = "0";
            lbGananciaPotencial.Text = "-";
            lbTotalArticulos.Text = "-";
            lbTotalStock.Text = "$ 0.00";
            flowLayoutPanel1.Controls.Clear();
        }
        private void btnLimpiarData_Click(object sender, EventArgs e)
        {
            LimpiarDashboard();
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            Utilidades.ExportarExcel.Descargar(dgvReporteStock, $"ReporteStock");
        }
    }
}
