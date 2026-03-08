using CapaDatos;
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
    public partial class mdProveedor : Form
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ProveedorListadoDto Proveedor { get; set; }
        private bool _formCargado = false;
        public mdProveedor()
        {
            InitializeComponent();
        }

        private void mdProveedor_Load(object sender, EventArgs e)
        {
            dgvMdProveedores.AutoGenerateColumns = false;
            CargarOpcionesBusqueda();
            _formCargado = true;
            ListarProveedoresDto();
        }
        private void ListarProveedoresDto(int filtro = 1)
        {
            using (var context = new AppDbContext())
            {
                var service = new ProveedorService(context);
                dgvMdProveedores.DataSource = service.ListarProveedores(filtro, true);
            }

            string[] columnasAOcultar = { "Id", "Telefono", "Correo", "Estado" };

            foreach (var col in columnasAOcultar)
            {
                if (dgvMdProveedores.Columns[col] != null)
                    dgvMdProveedores.Columns[col].Visible = false;
            }

            dgvMdProveedores.ClearSelection();
            dgvMdProveedores.CurrentCell = null;
        }
        private void CargarOpcionesBusqueda()
        {
            cbxBuscarPor.DataSource = new List<string> { "Documento", "Razon Social" };
            cbxBuscarPor.SelectedIndex = 0;
        }

        private void tbBuscarProveedores_TextChanged(object sender, EventArgs e)
        {
            if (!_formCargado) return;
            using (var context = new AppDbContext())
            {
                var service = new ProveedorService(context);

                string campo = cbxBuscarPor.SelectedItem.ToString();
                string texto = tbBuscarProveedores.Text.Trim();
                dgvMdProveedores.DataSource = service.Buscar(texto, campo);
            }
        }

        private void btnLimpiarData_Click(object sender, EventArgs e)
        {
            tbBuscarProveedores.Clear();
            if (cbxBuscarPor.Items.Count > 0)
            {
                cbxBuscarPor.SelectedIndex = 0;
            }
            ListarProveedoresDto();
        }

        private void dgvMdProveedores_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvMdProveedores.Rows[e.RowIndex];

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
                row.DefaultCellStyle.ForeColor = Color.FromArgb(120, 120, 120);
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(40, 40, 40);
                row.DefaultCellStyle.SelectionForeColor = Color.FromArgb(160, 160, 160);
            }
        }

        private void dgvMdProveedores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMdProveedores.Columns[e.ColumnIndex].Name == "btnSeleccionar" && e.RowIndex >= 0)
            {
                Proveedor = (ProveedorListadoDto)dgvMdProveedores.Rows[e.RowIndex].DataBoundItem;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
