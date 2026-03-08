using CapaNegocio.DTOs;
using CapaNegocio.Services;
using ClosedXML.Excel;
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
    public partial class frmClientes : Form
    {
        private bool _formCargado = false;
        public frmClientes()
        {
            InitializeComponent();
        }

        private void frmClientes_Load(object sender, EventArgs e)
        {
            dgvClientes.AutoGenerateColumns = false;
            CargarOpcionesBusqueda();
            CargarFiltrosEstados();
            _formCargado = true;

            ListarClientesDto();
            LimpiarFormulario();
        }

        private void btnGuardarCliente_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbClienteDocumento.Text) ||
                string.IsNullOrWhiteSpace(tbClienteCorreo.Text) ||
                string.IsNullOrWhiteSpace(tbClienteNombre.Text) ||
                string.IsNullOrWhiteSpace(tbClienteTelefono.Text))
            {
                MessageBox.Show("Todos los datos son obligatorios.");
                return;
            }
            try
            {
                using (var context = new AppDbContext())
                {
                    var service = new ClienteService(context);

                    int id = Convert.ToInt32(tbIdClientes.Text);

                    if (id == 0)
                    {
                        if (string.IsNullOrWhiteSpace(tbClienteDocumento.Text))
                        {
                            MessageBox.Show("Para un nuevo Cliente, el documento es obligatorio.");
                            return;
                        }
                    }

                    if (id == 0)
                    {
                        var nuevoCliente = new ClienteCreateDto
                        {
                            NombreCompleto = tbClienteNombre.Text,
                            Correo = tbClienteCorreo.Text,
                            Telefono = tbClienteTelefono.Text,
                            Documento = tbClienteDocumento.Text,
                        };

                        service.Registrar(nuevoCliente);
                        MessageBox.Show("Cliente registrado exitosamente.");
                    }
                    else
                    {
                        var dto = new ClienteUpdateDto
                        {
                            Id = id,
                            NombreCompleto = tbClienteNombre.Text,
                            Correo = tbClienteCorreo.Text,
                            Telefono = tbClienteTelefono.Text,
                            Documento = tbClienteDocumento.Text,
                        };

                        bool editado = service.Editar(dto);

                        if (!editado)
                        {
                            MessageBox.Show("No se pudo editar el Cliente.");
                            return;
                        }

                        MessageBox.Show("Cliente editado correctamente.");
                    }

                    ListarClientesDto();
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnEliminarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                int idCliente = Convert.ToInt32(tbIdClientes.Text);

                if (idCliente == 0)
                {
                    MessageBox.Show("Por favor, seleccione un cliente de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string accion = btnEliminarCliente.Text;

                var confirmResult = MessageBox.Show($"¿Está seguro de que desea {accion.ToLower()} este cliente?",
                                                  "Confirmar Acción",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        var service = new ClienteService(context);

                        bool exito = service.CambiarEstado(idCliente);

                        if (exito)
                        {
                            MessageBox.Show($"Cliente {accion.ToLower()}ado con éxito.", "NovaSales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ListarClientesDto();
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

        private void btnLimpiarCliente_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
        private void btnExportar_Click(object sender, EventArgs e)
        {
            Utilidades.ExportarExcel.Descargar(dgvClientes, "Reporte_Clientes");
        }
        private void ListarClientesDto(int filtro = 1)
        {
            using (var context = new AppDbContext())
            {
                var service = new ClienteService(context);
                dgvClientes.DataSource = service.ListarClientes(filtro);
            }

            ActualizarEstadisticas();

            if (dgvClientes.Columns["Id"] != null) dgvClientes.Columns["Id"].Visible = false;
            dgvClientes.ClearSelection();
            dgvClientes.CurrentCell = null;
        }

        private void Limpiar()
        {
            tbIdClientes.Text = "0";
            tbClienteCorreo.Clear();
            tbClienteDocumento.Clear();
            tbClienteNombre.Clear();
            tbClienteTelefono.Clear();
            btnGuardarCliente.Text = "GUARDAR";
            btnEliminarCliente.Text = "ELIMINAR";
            btnEliminarCliente.BorderColor = Color.FromArgb(45, 45, 45); // O el color dorado suave que uses
            btnEliminarCliente.ForeColor = Color.Gainsboro;
            btnEliminarCliente.FillColor = Color.FromArgb(30, 30, 30);
            tbClienteDocumento.Focus();
            dgvClientes.ClearSelection();
        }

        private void ActualizarEstadisticas()
        {
            using (var context = new AppDbContext())
            {
                var service = new ClienteService(context);
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
            cbxBuscarPor.DataSource = new List<string> { "Documento", "Nombre", "Correo", "Telefono" };
            cbxBuscarPor.SelectedIndex = 0;
        }
        private void LimpiarFormulario()
        {
            tbIdClientes.Text = "0";
            tbClienteDocumento.Clear();
            tbClienteCorreo.Clear();
            tbClienteNombre.Clear();
            tbClienteTelefono.Clear();
            btnGuardarCliente.Text = "GUARDAR";

            if (dgvClientes.DataSource != null)
            {
                dgvClientes.ClearSelection();
                dgvClientes.CurrentCell = null;
            }
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_formCargado || e.RowIndex < 0) return;

            if (dgvClientes.CurrentRow != null)
            {
                tbIdClientes.Text = dgvClientes.CurrentRow.Cells["Id"].Value?.ToString() ?? "0";
                tbClienteDocumento.Text = dgvClientes.CurrentRow.Cells["Documento"].Value?.ToString() ?? "";
                tbClienteNombre.Text = dgvClientes.CurrentRow.Cells["NombreCompleto"].Value?.ToString() ?? "";
                tbClienteCorreo.Text = dgvClientes.CurrentRow.Cells["Correo"].Value?.ToString() ?? "";
                tbClienteTelefono.Text = dgvClientes.CurrentRow.Cells["Telefono"].Value?.ToString() ?? "";

                string estadoStr = dgvClientes.CurrentRow.Cells["Estado"].Value?.ToString() ?? "Activo";

                if (estadoStr == "Activo")
                {
                    btnEliminarCliente.Text = "BAJA";
                    btnEliminarCliente.BorderColor = Color.FromArgb(192, 57, 43); // Borde Rojo
                    btnEliminarCliente.ForeColor = Color.FromArgb(192, 57, 43); // Texto Rojo
                }
                else
                {
                    btnEliminarCliente.Text = "ALTA";
                    btnEliminarCliente.BorderColor = Color.FromArgb(46, 204, 113); // Borde Verde
                    btnEliminarCliente.ForeColor = Color.FromArgb(46, 204, 113); // Texto Verde
                }

                btnEliminarCliente.FillColor = Color.Transparent;
                btnGuardarCliente.Text = "ACTUALIZAR";
            }
        }

        private void dgvClientes_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvClientes.Rows[e.RowIndex];

            if (row.Cells["Estado"].Value != null)
            {
                string estado = row.Cells["Estado"].Value.ToString();

                if (estado == "Activo")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(35, 30, 15);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(212, 175, 55);
                    row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 40, 20); // Un poco más claro al seleccionar
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

        private void cbxFiltrar_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!_formCargado) return;

            if (cbxFiltrar.SelectedValue is int filtro)
            {
                ListarClientesDto(filtro);
                tbBuscarClientes.Clear();
            }
        }

        private void tbBuscarClientes_TextChanged(object sender, EventArgs e)
        {
            if (!_formCargado) return;
            using (var context = new AppDbContext())
            {
                var service = new ClienteService(context);

                string campo = cbxBuscarPor.SelectedItem.ToString();
                string texto = tbBuscarClientes.Text.Trim();
                int filtroEstado = Convert.ToInt32(cbxFiltrar.SelectedValue);
                dgvClientes.DataSource = service.Buscar(campo, texto, filtroEstado);
            }
        }

        private void btnLimpiarData_Click(object sender, EventArgs e)
        {
            tbBuscarClientes.Clear();
            if (cbxBuscarPor.Items.Count > 0)
            {
                cbxBuscarPor.SelectedIndex = 0;
            }
            if (cbxFiltrar.Items.Count > 0)
            {
                cbxFiltrar.SelectedIndex = 0;
            }
            ListarClientesDto();
        }
    }
}
