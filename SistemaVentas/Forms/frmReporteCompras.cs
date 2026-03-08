using CapaNegocio.DTOs;
using CapaNegocio.Services;
using Microsoft.EntityFrameworkCore;
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
    public partial class frmReporteCompras : Form
    {
        private ToolTip _tt = new ToolTip();
        public frmReporteCompras()
        {
            InitializeComponent();
        }
        private void frmReporteCompras_Load(object sender, EventArgs e)
        {
            dgvReporteCompras.AutoGenerateColumns = false;
            cbxProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
            CargarProveedores();
        }
        private void btnVerReporte_Click(object sender, EventArgs e)
        {
            DateTime inicio = dtpFechaInicio.Value.Date;
            DateTime fin = dtpFechaFin.Value.Date;
            int idProveedor = Convert.ToInt32(cbxProveedor.SelectedValue);

            try
            {
                using (var context = new AppDbContext())
                {
                    var service = new ReporteService(context);

                    var lista = service.ObtenerCompras(inicio, fin, idProveedor);

                    dgvReporteCompras.DataSource = lista;

                    if (lista != null && lista.Count > 0)
                    {
                        CalcularResumenCompras(lista);
                        CargarDashboardCompletoCompras(lista, service);
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron compras en el periodo seleccionado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarDashboard();
                        flowLayoutPanel1.Controls.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el reporte de compras: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CalcularResumenCompras(List<ReporteCompraDto> lista)
        {
            if (lista == null || !lista.Any())
            {
                LimpiarDashboard();
                return;
            }

            decimal inversionPeriodo = lista.Sum(c => c.SubTotal);
            lbMontoTotal.Text = inversionPeriodo.ToString("C2");

            lbOrdenesCompra.Text = lista.Sum(c => c.Cantidad).ToString();

            var mayorProv = lista.GroupBy(c => c.RazonSocial)
                                 .OrderByDescending(g => g.Sum(x => x.SubTotal))
                                 .First().Key;
            lbProveedorPrincipal.Text = mayorProv;

            var prodMasCaro = lista.GroupBy(c => c.NombreProducto)
                                   .OrderByDescending(g => g.Sum(x => x.SubTotal))
                                   .First().Key;
            lbMayorCompra.Text = prodMasCaro;

            using (var context = new AppDbContext())
            {
                decimal valorStockActual = context.Productos
                                           .AsNoTracking()
                                           .Sum(p => p.Stock * p.PrecioCompra);

                lbGananciaNeta.Text = valorStockActual.ToString("C2");
            }
        }
        private void CargarDashboardCompletoCompras(List<ReporteCompraDto> lista, ReporteService service)
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.AutoScroll = true; 
            flowLayoutPanel1.Padding = new Padding(10);

            int totalFacturas = lista.Select(c => c.NumeroDocumento).Distinct().Count();
            decimal ticketPromedio = lista.Sum(c => c.SubTotal) / totalFacturas;
            AgregarIndicadorCustom("Gasto Promedio p/Factura", ticketPromedio.ToString("C2"), Color.FromArgb(0, 191, 255));

            decimal inversionActual = lista.Sum(c => c.SubTotal);
            decimal inversionAnterior = service.ObtenerTotalInversionMesAnterior();
            AgregarComparativaCustom(inversionActual, inversionAnterior);

            int productosDistintos = lista.Select(c => c.CodigoProducto).Distinct().Count();
            AgregarIndicadorCustom("Variedad de Artículos", productosDistintos.ToString() + " items", Color.LightSteelBlue);

            Label lblEspacio = new Label { Text = " ", Height = 20 };
            flowLayoutPanel1.Controls.Add(lblEspacio);
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

            _tt.SetToolTip(lblVal, $"{titulo}: {valor}");
            flowLayoutPanel1.Controls.Add(lblTit);
            flowLayoutPanel1.Controls.Add(lblVal);
        }

        private void AgregarComparativaCustom(decimal actual, decimal anterior)
        {
            Label lblTit = new Label { Text = "GASTO VS MES ANT.", AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 8, FontStyle.Bold) };
            flowLayoutPanel1.Controls.Add(lblTit);

            if (anterior > 0)
            {
                decimal diferencia = actual - anterior;
                decimal porcentaje = (diferencia / anterior) * 100;
                bool ahorro = porcentaje <= 0;

                Label lblPorc = new Label
                {
                    Text = $"{(porcentaje >= 0 ? "+" : "")}{porcentaje:N1}% {(porcentaje >= 0 ? "▲" : "▼")}",
                    AutoSize = true,
                    ForeColor = ahorro ? Color.LimeGreen : Color.Tomato,
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
        private void CargarProveedores()
        {
            using (var context = new AppDbContext())
            {
                var proveedoresService = new ProveedorService(context);
                var listaBD = proveedoresService.ListarProveedores();

                var listaConTodos = listaBD.Select(p => new
                {
                    Id = p.Id,
                    RazonSocial = p.RazonSocial
                }).ToList();

                listaConTodos.Insert(0, new { Id = 0, RazonSocial = "Todos" });

                cbxProveedor.DataSource = listaConTodos;
                cbxProveedor.DisplayMember = "RazonSocial";
                cbxProveedor.ValueMember = "Id";

                cbxProveedor.SelectedIndex = 0;
                cbxProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }
        private void LimpiarDashboard()
        {
            lbMontoTotal.Text = "$ 0.00";
            lbGananciaNeta.Text = "0";
            lbMayorCompra.Text = "-";
            lbProveedorPrincipal.Text = "-";
            lbOrdenesCompra.Text = "$ 0.00";
            flowLayoutPanel1.Controls.Clear();
        }

        private void dgvReporteCompras_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvReporteCompras.Rows[e.RowIndex];

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

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            Utilidades.ExportarExcel.Descargar(dgvReporteCompras, $"ReporteCompras");
        }

        private void btnLimpiarData_Click(object sender, EventArgs e)
        {
            LimpiarDashboard();
        }
    }
}
