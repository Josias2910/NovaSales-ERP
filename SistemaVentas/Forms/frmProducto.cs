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
    public partial class frmProducto : Form
    {
        private bool _formCargado = false;
        public frmProducto()
        {
            InitializeComponent();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {
            dgvProductos.AutoGenerateColumns = false;
            CargarComboCategorias();
            CargarOpcionesBusqueda();
            CargarFiltrosEstados();
            _formCargado = true;

            ListarProductosDto();
            LimpiarFormulario();
        }

        private void btnProductoGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbProductoDescripcion.Text) ||
                string.IsNullOrWhiteSpace(tbProductoCodigo.Text) ||
                string.IsNullOrWhiteSpace(tbProductoNombre.Text))
            {
                MessageBox.Show("Todos los datos son obligatorios.");
                return;
            }
            try
            {
                using (var context = new AppDbContext())
                {
                    var service = new ProductoService(context);

                    int id = Convert.ToInt32(tbIdProductos.Text);

                    if (id == 0)
                    {
                        if (string.IsNullOrWhiteSpace(tbProductoDescripcion.Text))
                        {
                            MessageBox.Show("Para un nuevo Producto, la descripcion es obligatoria.");
                            return;
                        }
                    }

                    if (id == 0)
                    {
                        var nuevoProducto = new ProductoCreateDto
                        {
                            Codigo = tbProductoCodigo.Text,
                            Nombre = tbProductoNombre.Text,
                            Descripcion = tbProductoDescripcion.Text,
                            CategoriaId = Convert.ToInt32(cbxProductoCategoria.SelectedValue),
                        };

                        service.Registar(nuevoProducto);
                        MessageBox.Show("Producto registrado exitosamente.");
                    }
                    else
                    {
                        var dto = new ProductoUpdateDto
                        {
                            Id = id,
                            Codigo = tbProductoCodigo.Text,
                            Nombre = tbProductoNombre.Text,
                            Descripcion = tbProductoDescripcion.Text,
                            CategoriaId = Convert.ToInt32(cbxProductoCategoria.SelectedValue),
                        };

                        bool editado = service.Editar(dto);

                        if (!editado)
                        {
                            MessageBox.Show("No se pudo editar el Categoria.");
                            return;
                        }

                        MessageBox.Show("Categoria editado correctamente.");
                    }

                    ListarProductosDto();
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnProductoLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnProductoEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int idProducto = Convert.ToInt32(tbIdProductos.Text);

                if (idProducto == 0)
                {
                    MessageBox.Show("Por favor, seleccione un Producto de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string accion = btnProductoEliminar.Text;

                var confirmResult = MessageBox.Show($"¿Está seguro de que desea {accion.ToLower()} este Producto?",
                                                  "Confirmar Acción",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        var service = new ProductoService(context);

                        bool exito = service.CambiarEstado(idProducto);

                        if (exito)
                        {
                            MessageBox.Show($"Producto {accion.ToLower()} con éxito.", "NovaSales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ListarProductosDto();
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

        private void ListarProductosDto(int filtro = 1)
        {
            using (var context = new AppDbContext())
            {
                var service = new ProductoService(context);
                dgvProductos.DataSource = service.ListarProductos(filtro);
            }

            ActualizarEstadisticas();

            if (dgvProductos.Columns["Id"] != null) dgvProductos.Columns["Id"].Visible = false;
            dgvProductos.ClearSelection();
            dgvProductos.CurrentCell = null;
        }

        private void Limpiar()
        {
            tbIdProductos.Text = "0";
            tbProductoDescripcion.Clear();
            tbProductoCodigo.Clear();
            tbProductoNombre.Clear();
            btnProductoGuardar.Text = "GUARDAR";
            btnProductoEliminar.Text = "ELIMINAR";
            btnProductoEliminar.BorderColor = Color.FromArgb(45, 45, 45); // O el color dorado suave que uses
            btnProductoEliminar.ForeColor = Color.Gainsboro;
            btnProductoEliminar.FillColor = Color.FromArgb(30, 30, 30);
            dgvProductos.ClearSelection();
            tbProductoDescripcion.Focus();
        }

        private void ActualizarEstadisticas()
        {
            using (var context = new AppDbContext())
            {
                var service = new ProductoService(context);
                var resumen = service.ObtenerResumenEstadistico();

                lbTotal.Text = resumen.total.ToString();
                lbActivosNum.Text = resumen.activos.ToString();
                lbInactivosNum.Text = resumen.inactivos.ToString();

                lbStock.Text = resumen.criticos.ToString();
                lbStock.ForeColor = resumen.criticos > 0 ? Color.FromArgb(230, 126, 34) : Color.FromArgb(46, 204, 113);

                lbCapTotal.Text = resumen.capital.ToString("C2");

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

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_formCargado || e.RowIndex < 0) return;

            if (dgvProductos.CurrentRow != null)
            {
                tbIdProductos.Text = dgvProductos.CurrentRow.Cells["Id"].Value?.ToString() ?? "0";
                tbProductoCodigo.Text = dgvProductos.CurrentRow.Cells["Codigo"].Value?.ToString() ?? "";
                tbProductoNombre.Text = dgvProductos.CurrentRow.Cells["Nombre"].Value?.ToString() ?? "";
                tbProductoDescripcion.Text = dgvProductos.CurrentRow.Cells["Descripcion"].Value?.ToString() ?? "";

                string estadoStr = dgvProductos.CurrentRow.Cells["Estado"].Value?.ToString() ?? "Activo";

                if (estadoStr == "Activo")
                {
                    btnProductoEliminar.Text = "BAJA";
                    btnProductoEliminar.BorderColor = Color.FromArgb(192, 57, 43); // Borde Rojo
                    btnProductoEliminar.ForeColor = Color.FromArgb(192, 57, 43); // Texto Rojo
                }
                else
                {
                    btnProductoEliminar.Text = "ALTA";
                    btnProductoEliminar.BorderColor = Color.FromArgb(46, 204, 113); // Borde Verde
                    btnProductoEliminar.ForeColor = Color.FromArgb(46, 204, 113); // Texto Verde
                }

                btnProductoEliminar.FillColor = Color.Transparent;

                string nombreCategoria = dgvProductos.CurrentRow.Cells["Categoria"].Value?.ToString();
                cbxProductoCategoria.SelectedIndex = cbxProductoCategoria.FindStringExact(nombreCategoria);

                btnProductoGuardar.Text = "ACTUALIZAR";
            }
        }

        private void CargarComboCategorias()
        {
            using (var context = new AppDbContext())
            {
                var service = new CategoriaService(context);
                var categorias = service.ListarCategorias(1);

                cbxProductoCategoria.DataSource = categorias;
                cbxProductoCategoria.DisplayMember = "Descripcion";
                cbxProductoCategoria.ValueMember = "Id";
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
            cbxBuscarPor.DataSource = new List<string> { "Nombre", "Codigo", "Categoria", "Descripcion" };
            cbxBuscarPor.SelectedIndex = 0;
        }

        private void LimpiarFormulario()
        {
            tbIdProductos.Text = "0";
            tbProductoDescripcion.Clear();
            btnProductoGuardar.Text = "GUARDAR";

            if (dgvProductos.DataSource != null)
            {
                dgvProductos.ClearSelection();
                dgvProductos.CurrentCell = null;
            }
        }

        private void dgvProductos_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvProductos.Rows[e.RowIndex];

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
                if (row.Cells["Stock"].Value != null && estado == "Activo")
                {
                    int stock = Convert.ToInt32(row.Cells["Stock"].Value);
                    var celdaStock = row.Cells["Stock"];

                    if (stock == 0)
                    {
                        // Rojo intenso para advertencia crítica
                        celdaStock.Style.BackColor = Color.FromArgb(192, 57, 43);
                        celdaStock.Style.ForeColor = Color.White;
                        celdaStock.Style.SelectionBackColor = Color.FromArgb(231, 76, 60); // Color cuando está seleccionado
                    }
                    else if (stock < 10)
                    {
                        // Naranja para advertencia preventiva
                        celdaStock.Style.BackColor = Color.FromArgb(211, 84, 0);
                        celdaStock.Style.ForeColor = Color.White;
                        celdaStock.Style.SelectionBackColor = Color.FromArgb(230, 126, 34);
                    }
                    else
                    {
                        // Si tiene stock normal, nos aseguramos de que herede el estilo de la fila
                        // Esto es importante para que al editar un producto el color se limpie si sube el stock
                        celdaStock.Style.BackColor = Color.Empty;
                        celdaStock.Style.ForeColor = Color.Empty;
                    }
                }
            }
        }

        private void tbBuscarProductos_TextChanged(object sender, EventArgs e)
        {
            if (!_formCargado) return;
            using (var context = new AppDbContext())
            {
                var service = new ProductoService(context);

                string campo = cbxBuscarPor.SelectedItem.ToString();
                string texto = tbBuscarProductos.Text.Trim();
                int filtroEstado = Convert.ToInt32(cbxFiltrar.SelectedValue);
                dgvProductos.DataSource = service.Buscar(campo, texto, filtroEstado);
            }
        }

        private void cbxFiltrar_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!_formCargado) return;

            if (cbxFiltrar.SelectedValue is int filtro)
            {
                ListarProductosDto(filtro);
                tbBuscarProductos.Clear();
            }
        }

        private void btnLimpiarData_Click(object sender, EventArgs e)
        {
            tbBuscarProductos.Clear();
            if (cbxBuscarPor.Items.Count > 0)
            {
                cbxBuscarPor.SelectedIndex = 0;
            }
            if (cbxFiltrar.Items.Count > 0)
            {
                cbxFiltrar.SelectedIndex = 0;
            }
            ListarProductosDto();
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            Utilidades.ExportarExcel.Descargar(dgvProductos, "Reporte_Productos");
        }

        private void dgvProductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    int idProducto = Convert.ToInt32(dgvProductos.Rows[e.RowIndex].Cells["Id"].Value);
                    string nombreProducto = dgvProductos.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();

                    using (var modal = new mdImagenProducto(idProducto, nombreProducto))
                    {
                        var resultado = modal.ShowDialog();

                        if (resultado == DialogResult.OK)
                        {
                            ListarProductosDto();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al seleccionar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
