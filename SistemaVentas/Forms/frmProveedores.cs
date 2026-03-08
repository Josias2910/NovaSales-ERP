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
    public partial class frmProveedores : Form
    {
        private bool _formCargado = false;
        public frmProveedores()
        {
            InitializeComponent();
        }

        private void frmProveedores_Load(object sender, EventArgs e)
        {
            dgvProveedores.AutoGenerateColumns = false;
            CargarOpcionesBusqueda();
            CargarFiltrosEstados();
            _formCargado = true;

            ListarProveedoresDto();
            LimpiarFormulario();
        }

        private void btnGuardarProveedor_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbProveedorCorreo.Text) ||
               string.IsNullOrWhiteSpace(tbProveedorDocumento.Text) ||
               string.IsNullOrWhiteSpace(tbProveedorTelefono.Text) ||
               string.IsNullOrWhiteSpace(tbProveedorRazonSocial.Text))
            {
                MessageBox.Show("Todos los datos son obligatorios.");
                return;
            }
            try
            {
                using (var context = new AppDbContext())
                {
                    var service = new ProveedorService(context);

                    int id = Convert.ToInt32(tbIdProveedores.Text);

                    if (id == 0)
                    {
                        if (string.IsNullOrWhiteSpace(tbProveedorDocumento.Text))
                        {
                            MessageBox.Show("Para un nuevo Proveedor, el documento es obligatorio.");
                            return;
                        }
                    }

                    if (id == 0)
                    {
                        var nuevoProveedor = new ProveedorCreateDto
                        {
                            Documento = tbProveedorDocumento.Text,
                            RazonSocial = tbProveedorRazonSocial.Text,
                            Telefono = tbProveedorTelefono.Text,
                            Correo = tbProveedorCorreo.Text
                        };

                        service.Registrar(nuevoProveedor);
                        MessageBox.Show("Proveedor registrado exitosamente.");
                    }
                    else
                    {
                        var dto = new ProveedorUpdateDto
                        {
                            Id = id,
                            Documento = tbProveedorDocumento.Text,
                            RazonSocial = tbProveedorRazonSocial.Text,
                            Telefono = tbProveedorTelefono.Text,
                            Correo = tbProveedorCorreo.Text
                        };

                        bool editado = service.Editar(dto);

                        if (!editado)
                        {
                            MessageBox.Show("No se pudo editar el Proveedor.");
                            return;
                        }

                        MessageBox.Show("Proveedor editado correctamente.");
                    }

                    ListarProveedoresDto();
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLimpiarProveedor_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnEliminarProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                int idProveedor = Convert.ToInt32(tbIdProveedores.Text);

                if (idProveedor == 0)
                {
                    MessageBox.Show("Por favor, seleccione un proveedor de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string accion = btnEliminarProveedor.Text;

                var confirmResult = MessageBox.Show($"¿Está seguro de que desea {accion.ToLower()} este proveedor?",
                                                  "Confirmar Acción",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        var service = new ProveedorService(context);

                        bool exito = service.CambiarEstado(idProveedor);

                        if (exito)
                        {
                            MessageBox.Show($"Proveedor {accion.ToLower()} con éxito.", "NovaSales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ListarProveedoresDto();
                            Limpiar();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo completar la operación.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiarData_Click(object sender, EventArgs e)
        {
            tbBuscarProveedor.Clear();
            if (cbxBuscarPor.Items.Count > 0)
            {
                cbxBuscarPor.SelectedIndex = 0;
            }
            if (cbxFiltrar.Items.Count > 0)
            {
                cbxFiltrar.SelectedIndex = 0;
            }
            ListarProveedoresDto();
        }

        private void ListarProveedoresDto(int filtro = 1)
        {
            using (var context = new AppDbContext())
            {
                var service = new ProveedorService(context);
                dgvProveedores.DataSource = service.ListarProveedores(filtro);
            }

            ActualizarEstadisticas();

            if (dgvProveedores.Columns["Id"] != null) dgvProveedores.Columns["Id"].Visible = false;
            dgvProveedores.ClearSelection();
            dgvProveedores.CurrentCell = null;
        }

        private void Limpiar()
        {
            tbIdProveedores.Text = "0";
            tbProveedorCorreo.Clear();
            tbProveedorDocumento.Clear();
            tbProveedorRazonSocial.Clear();
            tbProveedorTelefono.Clear();
            btnGuardarProveedor.Text = "GUARDAR";
            btnEliminarProveedor.Text = "ELIMINAR";
            btnEliminarProveedor.BorderColor = Color.FromArgb(45, 45, 45);
            btnEliminarProveedor.ForeColor = Color.Gainsboro;
            btnEliminarProveedor.FillColor = Color.FromArgb(30, 30, 30);
            cbxFiltrar.SelectedIndex = 0;
            tbProveedorDocumento.Focus();
            dgvProveedores.ClearSelection();
        }

        private void ActualizarEstadisticas()
        {
            using (var context = new AppDbContext())
            {
                var service = new ProveedorService(context);
                var resumen = service.ObtenerResumenEstadistico();

                lbTotal.Text = resumen.total.ToString();
                lbActivosNum.Text = resumen.activos.ToString();
                lbInactivosNum.Text = resumen.inactivos.ToString();

                if (resumen.total == 0)
                {
                    guna2CircleProgressBar1.Value = 0;
                    return;
                }

                if (resumen.activos == resumen.total)
                {
                    guna2CircleProgressBar1.ProgressColor = Color.FromArgb(46, 204, 113);
                    guna2CircleProgressBar1.ProgressColor2 = Color.FromArgb(46, 204, 113);
                }
                else if (resumen.activos == 0)
                {
                    guna2CircleProgressBar1.ProgressColor = Color.FromArgb(230, 126, 34);
                    guna2CircleProgressBar1.ProgressColor2 = Color.FromArgb(230, 126, 34);
                }
                else
                {
                    guna2CircleProgressBar1.ProgressColor = Color.FromArgb(46, 204, 113);
                    guna2CircleProgressBar1.ProgressColor2 = Color.FromArgb(39, 174, 96);
                }

                double porcentajeActivos = (double)resumen.activos / resumen.total * 100;
                guna2CircleProgressBar1.Value = Convert.ToInt32(porcentajeActivos);
            }
        }

        private void CargarFiltrosEstados()
        {
            var estados = new List<EstadoFiltroDto>()
            {
                new EstadoFiltroDto{ Texto = "Activo", Valor = 1 },
                new EstadoFiltroDto{ Texto = "Inactivo", Valor = 2 },
                new EstadoFiltroDto{ Texto = "Todos", Valor = 3 }
            };

            cbxFiltrar.DisplayMember = "Texto";
            cbxFiltrar.ValueMember = "Valor";
            cbxFiltrar.DataSource = estados;
        }

        private void CargarOpcionesBusqueda()
        {
            cbxBuscarPor.DataSource = new List<string> { "Documento", "Correo", "Telefono", "Razon Social" };
            cbxBuscarPor.SelectedIndex = 0;
        }

        private void LimpiarFormulario()
        {
            tbIdProveedores.Text = "0";
            tbProveedorDocumento.Clear();
            tbProveedorCorreo.Clear();
            tbProveedorRazonSocial.Clear();
            tbProveedorTelefono.Clear();
            btnGuardarProveedor.Text = "GUARDAR";

            if (dgvProveedores.DataSource != null)
            {
                dgvProveedores.ClearSelection();
                dgvProveedores.CurrentCell = null;
            }
        }

        private void tbBuscarProveedor_TextChanged(object sender, EventArgs e)
        {
            if (!_formCargado) return;
            using (var context = new AppDbContext())
            {
                var service = new ProveedorService(context);

                string campo = cbxBuscarPor.SelectedItem.ToString();
                string texto = tbBuscarProveedor.Text.Trim();
                int filtroEstado = Convert.ToInt32(cbxFiltrar.SelectedValue);
                dgvProveedores.DataSource = service.Buscar(texto, campo, filtroEstado);
            }
        }

        private void cbxFiltrar_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!_formCargado) return;

            if (cbxFiltrar.SelectedValue is int filtro)
            {
                ListarProveedoresDto(filtro);
                tbBuscarProveedor.Clear();
            }
        }

        private void dgvProveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_formCargado || e.RowIndex < 0) return;

            if (dgvProveedores.CurrentRow != null)
            {
                tbIdProveedores.Text = dgvProveedores.CurrentRow.Cells["Id"].Value?.ToString() ?? "0";
                tbProveedorDocumento.Text = dgvProveedores.CurrentRow.Cells["Documento"].Value?.ToString() ?? "";
                tbProveedorRazonSocial.Text = dgvProveedores.CurrentRow.Cells["RazonSocial"].Value?.ToString() ?? "";
                tbProveedorTelefono.Text = dgvProveedores.CurrentRow.Cells["Telefono"].Value?.ToString() ?? "";
                tbProveedorCorreo.Text = dgvProveedores.CurrentRow.Cells["Correo"].Value?.ToString() ?? "";

                string estadoStr = dgvProveedores.CurrentRow.Cells["Estado"].Value?.ToString() ?? "Activo";

                if (estadoStr == "Activo")
                {
                    btnEliminarProveedor.Text = "BAJA";
                    btnEliminarProveedor.BorderColor = Color.FromArgb(192, 57, 43); // Borde Rojo
                    btnEliminarProveedor.ForeColor = Color.FromArgb(192, 57, 43); // Texto Rojo
                }
                else
                {
                    btnEliminarProveedor.Text = "ALTA";
                    btnEliminarProveedor.BorderColor = Color.FromArgb(46, 204, 113); // Borde Verde
                    btnEliminarProveedor.ForeColor = Color.FromArgb(46, 204, 113); // Texto Verde
                }

                btnEliminarProveedor.FillColor = Color.Transparent;
                btnGuardarProveedor.Text = "ACTUALIZAR";
            }
        }

        private void dgvProveedores_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvProveedores.Rows[e.RowIndex];

            if (row.Cells["Estado"].Value != null)
            {
                string estado = row.Cells["Estado"].Value.ToString();

                if (estado == "Activo")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(35, 30, 15);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(212, 175, 55);
                    row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 40, 20);
                    row.DefaultCellStyle.SelectionForeColor = Color.FromArgb(255, 215, 0);
                }
                else if (estado == "Inactivo")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(120, 120, 120);
                    row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(40, 40, 40);
                    row.DefaultCellStyle.SelectionForeColor = Color.FromArgb(160, 160, 160);
                }
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            Utilidades.ExportarExcel.Descargar(dgvProveedores, "Reporte_Proveedores");
        }
    }
}
