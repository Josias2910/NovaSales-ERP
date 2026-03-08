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
    public partial class frmUsuarios : Form
    {
        private bool _formCargado = false;
        public frmUsuarios()
        {
            InitializeComponent();
        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {
            dgvUsuarios.AutoGenerateColumns = false;
            CargarRoles();
            CargarFiltrosEstados();
            CargarOpcionesBusqueda();

            _formCargado = true;
            ListarUsuariosDto();
            LimpiarFormulario();
        }
        private void CargarRoles()
        {
            using (var context = new AppDbContext())
            {
                var service = new RolService(context);
                var roles = service.ObtenerRolesCombo();

                cbxRolUsuario.DataSource = roles;
                cbxRolUsuario.DisplayMember = "Descripcion";
                cbxRolUsuario.ValueMember = "Id";
            }
        }
        private void CargarOpcionesBusqueda()
        {
            cbxBuscarPor.DataSource = new List<string> { "Nombre", "Correo", "Documento" };
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
        private void ListarUsuariosDto()
        {
            using (var context = new AppDbContext())
            {
                var service = new UsuarioService(context);
                dgvUsuarios.DataSource = service.ListarUsuarios();
            }

            ActualizarEstadisticas();

            dgvUsuarios.ClearSelection();
            dgvUsuarios.CurrentCell = null;
            this.ActiveControl = null;
        }
        private void LimpiarFormulario()
        {
            tbId.Text = "0";
            tbNombreUsuario.Clear();
            tbCorreoUsuario.Clear();
            tbUsuarioDocumento.Clear();
            tbContraUsuario.Clear();
            tbConfirmarContraUsuario.Clear();
            cbxRolUsuario.SelectedIndex = 0;
            btnGuardarUsuario.Text = "GUARDAR";

            if (dgvUsuarios.DataSource != null)
            {
                dgvUsuarios.ClearSelection();
                dgvUsuarios.CurrentCell = null;
            }
        }
        private void btnGuardarUsuario_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbNombreUsuario.Text) ||
                string.IsNullOrWhiteSpace(tbCorreoUsuario.Text) ||
                string.IsNullOrWhiteSpace(tbUsuarioDocumento.Text))
            {
                MessageBox.Show("Los campos Nombre, Correo y Documento son obligatorios.");
                return;
            }
            try
            {
                using (var context = new AppDbContext())
                {
                    var service = new UsuarioService(context);

                    int id = Convert.ToInt32(tbId.Text);

                    if (id == 0)
                    {
                        if (string.IsNullOrWhiteSpace(tbContraUsuario.Text) ||
                            string.IsNullOrWhiteSpace(tbConfirmarContraUsuario.Text))
                        {
                            MessageBox.Show("Para un nuevo usuario, la contraseña es obligatoria.");
                            return;
                        }

                        if (tbContraUsuario.Text != tbConfirmarContraUsuario.Text)
                        {
                            MessageBox.Show("Las contraseñas no coinciden.");
                            return;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(tbContraUsuario.Text) &&
                            tbContraUsuario.Text != tbConfirmarContraUsuario.Text)
                        {
                            MessageBox.Show("Las contraseñas no coinciden.");
                            return;
                        }
                    }

                    if (id == 0)
                    {
                        var nuevoUsuario = new UsuarioCreateDto
                        {
                            NombreCompleto = tbNombreUsuario.Text,
                            Correo = tbCorreoUsuario.Text,
                            Documento = tbUsuarioDocumento.Text,
                            Clave = tbContraUsuario.Text,
                            ConfirmarClave = tbConfirmarContraUsuario.Text,
                            RolId = (int)cbxRolUsuario.SelectedValue
                        };

                        service.Guardar(nuevoUsuario);
                        MessageBox.Show("Usuario registrado exitosamente.");
                    }
                    else
                    {
                        var dto = new UsuarioUpdateDto
                        {
                            Id = id,
                            NombreCompleto = tbNombreUsuario.Text,
                            Correo = tbCorreoUsuario.Text,
                            Documento = tbUsuarioDocumento.Text,
                            Clave = tbContraUsuario.Text,
                            RolId = (int)cbxRolUsuario.SelectedValue
                        };

                        bool editado = service.Editar(dto);

                        if (!editado)
                        {
                            MessageBox.Show("No se pudo editar el usuario.");
                            return;
                        }

                        MessageBox.Show("Usuario editado correctamente.");
                    }

                    ListarUsuariosDto();
                    LimpiarFormulario();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tbBuscarUsuarios_TextChanged(object sender, EventArgs e)
        {
            using (var context = new AppDbContext())
            {
                var service = new UsuarioService(context);
                string campo = cbxBuscarPor.SelectedItem.ToString();
                string texto = tbBuscarUsuarios.Text.Trim();
                dgvUsuarios.DataSource = service.Buscar(campo, texto);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            tbBuscarUsuarios.Clear();
            cbxBuscarPor.SelectedIndex = 0;
            if (cbxFiltrar.Items.Count > 0)
            {
                cbxFiltrar.SelectedIndex = 0;
            }
            ListarUsuariosDto();
        }

        private void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                int idUsuario = Convert.ToInt32(tbId.Text);

                if (idUsuario == 0)
                {
                    MessageBox.Show("Por favor, seleccione un usuario de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string accion = btnEliminarUsuario.Text;

                var confirmResult = MessageBox.Show($"¿Está seguro de que desea {accion.ToLower()} este usuario?",
                                                  "Confirmar Acción",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    using (var context = new AppDbContext())
                    {
                        var service = new UsuarioService(context);

                        bool exito = service.CambiarEstado(idUsuario);

                        if (exito)
                        {
                            MessageBox.Show($"usuario {accion.ToLower()} con éxito.", "NovaSales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ListarUsuariosDto();
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

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_formCargado) return;

            if (dgvUsuarios.CurrentRow != null)
            {
                tbId.Text = dgvUsuarios.CurrentRow.Cells["Id"].Value?.ToString();
                tbNombreUsuario.Text = dgvUsuarios.CurrentRow.Cells["NombreCompleto"].Value?.ToString();
                tbCorreoUsuario.Text = dgvUsuarios.CurrentRow.Cells["Correo"].Value?.ToString();
                tbUsuarioDocumento.Text = dgvUsuarios.CurrentRow.Cells["Documento"].Value?.ToString();

                string rolNombre = dgvUsuarios.CurrentRow.Cells["Rol"].Value?.ToString() ?? "";
                cbxRolUsuario.SelectedIndex = cbxRolUsuario.FindStringExact(rolNombre);

                tbContraUsuario.Clear();
                tbConfirmarContraUsuario.Clear();

                string estadoStr = dgvUsuarios.CurrentRow.Cells["Estado"].Value?.ToString() ?? "Activo";

                if (estadoStr == "Activo")
                {
                    btnEliminarUsuario.Text = "BAJA";
                    btnEliminarUsuario.BorderColor = Color.FromArgb(192, 57, 43);
                    btnEliminarUsuario.ForeColor = Color.FromArgb(192, 57, 43);
                }
                else
                {
                    btnEliminarUsuario.Text = "ALTA";
                    btnEliminarUsuario.BorderColor = Color.FromArgb(46, 204, 113);
                    btnEliminarUsuario.ForeColor = Color.FromArgb(46, 204, 113);
                }

                btnEliminarUsuario.FillColor = Color.Transparent;
                btnGuardarUsuario.Text = "ACTUALIZAR";
            }
        }

        private void cbxFiltrar_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbxFiltrar.SelectedValue is int filtro)
            {
                using (var context = new AppDbContext())
                {
                    var service = new UsuarioService(context);
                    dgvUsuarios.DataSource = service.ListarUsuarios(filtro);
                }
            }
        }

        private void dgvUsuarios_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvUsuarios.Rows[e.RowIndex];

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

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (!_formCargado) return;

            if (dgvUsuarios.CurrentRow == null || dgvUsuarios.CurrentCell == null)
            {
                tbId.Text = "0";
                return;
            }

            if (dgvUsuarios.Focused)
            {
                tbId.Text = dgvUsuarios.CurrentRow.Cells["Id"].Value?.ToString();
            }
        }

        private void btnLimpiarUsuario_Click_1(object sender, EventArgs e)
        {
            Limpiar();
        }
        private void Limpiar()
        {
            tbId.Text = "0";
            tbCorreoUsuario.Clear();
            tbUsuarioDocumento.Clear();
            tbNombreUsuario.Clear();
            tbContraUsuario.Clear();
            tbConfirmarContraUsuario.Clear();
            btnGuardarUsuario.Text = "GUARDAR";
            btnEliminarUsuario.Text = "ELIMINAR";
            btnEliminarUsuario.BorderColor = Color.FromArgb(45, 45, 45); // O el color dorado suave que uses
            btnEliminarUsuario.ForeColor = Color.Gainsboro;
            btnEliminarUsuario.FillColor = Color.FromArgb(30, 30, 30);
            tbUsuarioDocumento.Focus();
            dgvUsuarios.ClearSelection();
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            Utilidades.ExportarExcel.Descargar(dgvUsuarios, "Reporte_Usuarios");
        }
        private void ActualizarEstadisticas()
        {
            using (var context = new AppDbContext())
            {
                var service = new UsuarioService(context);

                var (total, activos, inactivos) = service.ObtenerResumenEstadisticas();

                lbTotal.Text = total.ToString();
                lbActivosNum.Text = activos.ToString();
                lbInactivosNum.Text = inactivos.ToString();

                if (total == 0)
                {
                    guna2CircleProgressBar1.Value = 0;
                    return;
                }

                if (activos == total)
                {
                    guna2CircleProgressBar1.ProgressColor = Color.FromArgb(46, 204, 113);
                    guna2CircleProgressBar1.ProgressColor2 = Color.FromArgb(46, 204, 113);
                }
                else if (activos == 0)
                {
                    guna2CircleProgressBar1.ProgressColor = Color.FromArgb(230, 126, 34);
                    guna2CircleProgressBar1.ProgressColor2 = Color.FromArgb(230, 126, 34);
                }
                else
                {
                    guna2CircleProgressBar1.ProgressColor = Color.FromArgb(46, 204, 113);
                    guna2CircleProgressBar1.ProgressColor2 = Color.FromArgb(39, 174, 96);
                }

                double porcentajeActivos = (double)activos / total * 100;
                guna2CircleProgressBar1.Value = Convert.ToInt32(porcentajeActivos);
            }
        }
    }
}
