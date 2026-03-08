using CapaNegocio.DTOs;
using CapaNegocio.Services;
using SistemaVentas.Data;
using SistemaVentas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CapaPresentacion.Forms
{
    public partial class mdProducto : Form
    {

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ProductoListadoDto Producto { get; set; }
        private bool _formCargado = false;
        public mdProducto()
        {
            InitializeComponent();
        }

        private void mdProducto_Load(object sender, EventArgs e)
        {
            dgvMdProductos.AutoGenerateColumns = false;
            CargarOpcionesBusqueda();
            _formCargado = true;
            ListarProductosDto();
        }
        private void CargarOpcionesBusqueda()
        {
            cbxBuscarPor.DataSource = new List<string> { "Codigo", "Nombre", "Categoria" };
            cbxBuscarPor.SelectedIndex = 0;
        }
        private void tbBuscarProveedores_TextChanged(object sender, EventArgs e)
        {
            if (!_formCargado) return;
            using (var context = new AppDbContext())
            {
                var service = new ProductoService(context);

                string campo = cbxBuscarPor.SelectedItem.ToString();
                string texto = tbBuscarProductos.Text.Trim();
                dgvMdProductos.DataSource = service.Buscar(texto, campo);
            }
        }

        private void btnLimpiarData_Click(object sender, EventArgs e)
        {
            tbBuscarProductos.Clear();
            if (cbxBuscarPor.Items.Count > 0)
            {
                cbxBuscarPor.SelectedIndex = 0;
            }
            ListarProductosDto();
        }
        private void ListarProductosDto(int filtro = 1)
        {
            using (var context = new AppDbContext())
            {
                var service = new ProductoService(context);
                dgvMdProductos.DataSource = service.ListarProductos(filtro, false);
            }

            string[] columnasAOcultar = { "Id", "Descripcion", "Stock", "PrecioVenta", "PrecioCompra", "Estado" };

            foreach (var col in columnasAOcultar)
            {
                if (dgvMdProductos.Columns[col] != null)
                    dgvMdProductos.Columns[col].Visible = false;
            }

            dgvMdProductos.ClearSelection();
            dgvMdProductos.CurrentCell = null;
        }

        private void dgvMdProductos_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvMdProductos.Rows[e.RowIndex];

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
                row.DefaultCellStyle.ForeColor = Color.FromArgb(120, 120, 120);
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(40, 40, 40);
                row.DefaultCellStyle.SelectionForeColor = Color.FromArgb(160, 160, 160);
            }
        }

        private void dgvMdProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificamos que el clic sea en una columna de tipo botón y no en el encabezado
            if (dgvMdProductos.Columns[e.ColumnIndex].Name == "btnSeleccionar" && e.RowIndex >= 0)
            {
                // Guardamos el objeto completo de la fila seleccionada
                Producto = (ProductoListadoDto)dgvMdProductos.Rows[e.RowIndex].DataBoundItem;

                // Le decimos al formulario que la operación fue exitosa y lo cerramos
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
