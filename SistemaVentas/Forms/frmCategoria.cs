using SistemaVentas.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CapaNegocio.Services;
using CapaNegocio.DTOs;
using ClosedXML.Excel;

namespace CapaPresentacion.Forms
{
    public partial class frmCategoria : Form
    {
        private bool _formCargado = false;
        public frmCategoria()
        {
            InitializeComponent();
        }

        private void frmCategoria_Load(object sender, EventArgs e)
        {
            dgvCategorias.AutoGenerateColumns = false;
            CargarFiltrosEstados();
            CargarOpcionesBusqueda();

            _formCargado = true;
            ListarCategoriasDto();
            LimpiarFormulario();
        }

        private void dgvCategorias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_formCargado || e.RowIndex < 0) return;
            if (dgvCategorias.CurrentRow != null)
            {
                tbIdCategorias.Text = dgvCategorias.CurrentRow.Cells["Id"].Value?.ToString() ?? "0";
                tbCategoriaDescripcion.Text = dgvCategorias.CurrentRow.Cells["Descripcion"].Value?.ToString() ?? "";

                string estadoStr = dgvCategorias.CurrentRow.Cells["Estado"].Value?.ToString() ?? "Activo";

                if (estadoStr == "Activo")
                {
                    btnEliminarCategoria.Text = "BAJA";
                    btnEliminarCategoria.BorderColor = Color.FromArgb(192, 57, 43); // Borde Rojo
                    btnEliminarCategoria.ForeColor = Color.FromArgb(192, 57, 43); // Texto Rojo
                }
                else
                {
                    btnEliminarCategoria.Text = "ALTA";
                    btnEliminarCategoria.BorderColor = Color.FromArgb(46, 204, 113); // Borde Verde
                    btnEliminarCategoria.ForeColor = Color.FromArgb(46, 204, 113); // Texto Verde
                }

                btnEliminarCategoria.FillColor = Color.Transparent;

                btnGuardarCategoria.Text = "ACTUALIZAR";
            }
        }

        private void btnGuardarCategoria_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbCategoriaDescripcion.Text))
            {
                MessageBox.Show("Descripcion es obligatorio");
                return;
            }
            try
            {
                using (var context = new AppDbContext())
                {
                    var service = new CategoriaService(context);

                    int id = Convert.ToInt32(tbIdCategorias.Text);

                    if (id == 0)
                    {
                        if (string.IsNullOrWhiteSpace(tbCategoriaDescripcion.Text))
                        {
                            MessageBox.Show("Para una nueva categoria, la descripcion es obligatoria.");
                            return;
                        }
                    }

                    if (id == 0)
                    {
                        var nuevaCategoria = new CategoriaCreateDto
                        {
                            Descripcion = tbCategoriaDescripcion.Text,
                        };

                        service.Registar(nuevaCategoria);
                        MessageBox.Show("Categoria registrada exitosamente.");
                    }
                    else
                    {
                        var dto = new CategoriaUpdateDto
                        {
                            Id = id,
                            Descripcion = tbCategoriaDescripcion.Text,
                        };

                        bool editado = service.Editar(dto);

                        if (!editado)
                        {
                            MessageBox.Show("No se pudo editar el Categoria.");
                            return;
                        }

                        MessageBox.Show("Categoria editado correctamente.");
                    }

                    ListarCategoriasDto();
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Limpiar()
        {
            tbIdCategorias.Text = "0";
            tbCategoriaDescripcion.Clear();
            btnGuardarCategoria.Text = "GUARDAR";
            btnEliminarCategoria.Text = "ELIMINAR";
            btnEliminarCategoria.BorderColor = Color.FromArgb(45, 45, 45);
            btnEliminarCategoria.ForeColor = Color.Gainsboro;
            btnEliminarCategoria.FillColor = Color.FromArgb(30, 30, 30);
            tbCategoriaDescripcion.Focus();
            dgvCategorias.ClearSelection();
            tbCategoriaDescripcion.Focus();
        }

        private void btnLimpiarCategoria_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
        private void LimpiarFormulario()
        {
            tbIdCategorias.Text = "0";
            tbCategoriaDescripcion.Clear();
            btnGuardarCategoria.Text = "GUARDAR";

            if (dgvCategorias.DataSource != null)
            {
                dgvCategorias.ClearSelection();
                dgvCategorias.CurrentCell = null;
            }
        }
        private void btnEliminarCategoria_Click(object sender, EventArgs e)
        {
            try
            {
                int idCategoria = Convert.ToInt32(tbIdCategorias.Text);

                if (idCategoria == 0)
                {
                    MessageBox.Show("Por favor, seleccione una Categoria de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string accion = btnEliminarCategoria.Text;

                var confirmResult = MessageBox.Show($"¿Está seguro de que desea {accion.ToLower()} esta Categoria?",
                                                  "Confirmar Acción",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        var service = new CategoriaService(context);

                        bool exito = service.CambiarEstado(idCategoria);

                        if (exito)
                        {
                            MessageBox.Show($"Categoria {accion.ToLower()} con éxito.", "NovaSales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ListarCategoriasDto();
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
        private void CargarOpcionesBusqueda()
        {
            cbxBuscarPor.DataSource = new List<string> { "Descripcion" };
            cbxBuscarPor.SelectedIndex = 0;
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
        private void ListarCategoriasDto(int filtro = 1)
        {
            using (var context = new AppDbContext())
            {
                var service = new CategoriaService(context);
                dgvCategorias.DataSource = service.ListarCategorias(filtro);
            }

            ActualizarEstadisticas();

            if (dgvCategorias.Columns["Id"] != null) dgvCategorias.Columns["Id"].Visible = false;
            dgvCategorias.ClearSelection();
            dgvCategorias.CurrentCell = null;
        }

        private void dgvCategorias_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvCategorias.Rows[e.RowIndex];

            if (row.Cells["Estado"].Value != null)
            {
                string estado = row.Cells["Estado"].Value.ToString();

                if (estado == "Activo")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(35, 30, 15);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(212, 175, 55);
                }
                else if (estado == "Inactivo")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(150, 150, 150);
                }
            }
        }

        private void btnLimpiarData_Click(object sender, EventArgs e)
        {
            tbBuscarCategorias.Clear();
            if (cbxBuscarPor.Items.Count > 0)
            {
                cbxBuscarPor.SelectedIndex = 0;
            }
            if (cbxFiltrar.Items.Count > 0)
            {
                cbxFiltrar.SelectedIndex = 0;
            }
            ListarCategoriasDto();
        }

        private void cbxFiltrar_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!_formCargado) return;

            if (cbxFiltrar.SelectedValue is int filtro)
            {
                ListarCategoriasDto(filtro);
            }
        }

        private void tbBuscarCategorias_TextChanged(object sender, EventArgs e)
        {
            if (!_formCargado) return;
            using (var context = new AppDbContext())
            {
                var service = new CategoriaService(context);

                string campo = cbxBuscarPor.SelectedItem.ToString();
                string texto = tbBuscarCategorias.Text.Trim();
                int filtroEstado = Convert.ToInt32(cbxFiltrar.SelectedValue);
                dgvCategorias.DataSource = service.Buscar(campo, texto, filtroEstado);
            }
        }

        private void ActualizarEstadisticas()
        {
            using (var context = new AppDbContext())
            {
                var service = new CategoriaService(context);
                var resumen = service.ObtenerResumenEstadistico();

                lbTotal.Text = resumen.total.ToString();

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

        private void btnExportar_Click(object sender, EventArgs e)
        {
            Utilidades.ExportarExcel.Descargar(dgvCategorias, "Reporte_Categorias");
        }
    }
}
