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
    public partial class mdCliente : Form
    {

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ClienteListadoDto Cliente { get; set; }
        private bool _formCargado = false;
        public mdCliente()
        {
            InitializeComponent();
        }

        private void mdCliente_Load(object sender, EventArgs e)
        {
            dgvMdClientes.AutoGenerateColumns = false;
            CargarOpcionesBusqueda();
            _formCargado = true;
            ListarClientesDto();
        }
        private void CargarOpcionesBusqueda()
        {
            cbxBuscarPor.DataSource = new List<string> { "NombreCompleto", "Documento" };
            cbxBuscarPor.SelectedIndex = 0;
        }

        private void tbBuscarClientes_TextChanged(object sender, EventArgs e)
        {
            if (!_formCargado) return;
            using (var context = new AppDbContext())
            {
                var service = new ClienteService(context);

                string campo = cbxBuscarPor.SelectedItem.ToString();
                string texto = tbBuscarClientes.Text.Trim();
                dgvMdClientes.DataSource = service.Buscar(texto, campo);
            }
        }

        private void btnLimpiarData_Click(object sender, EventArgs e)
        {
            tbBuscarClientes.Clear();
            if (cbxBuscarPor.Items.Count > 0)
            {
                cbxBuscarPor.SelectedIndex = 0;
            }
            ListarClientesDto();
        }
        private void ListarClientesDto(int filtro = 1)
        {
            using (var context = new AppDbContext())
            {
                var service = new ClienteService(context);
                dgvMdClientes.DataSource = service.ListarClientes(filtro, true);
            }

            string[] columnasAOcultar = { "Id", "Correo", "Telefono", "Estado" };

            foreach (var col in columnasAOcultar)
            {
                if (dgvMdClientes.Columns[col] != null)
                    dgvMdClientes.Columns[col].Visible = false;
            }

            dgvMdClientes.ClearSelection();
            dgvMdClientes.CurrentCell = null;
        }

        private void dgvMdClientes_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvMdClientes.Rows[e.RowIndex];

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

        private void dgvMdClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMdClientes.Columns[e.ColumnIndex].Name == "btnSeleccionar" && e.RowIndex >= 0)
            {
                Cliente = (ClienteListadoDto)dgvMdClientes.Rows[e.RowIndex].DataBoundItem;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
