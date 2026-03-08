using SistemaVentas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CapaNegocio.Services;
using CapaNegocio.DTOs;
using SistemaVentas.Domain.Entities;
using CapaDatos;
using SistemaVentas.Data;
using CapaPresentacion.Forms;

namespace CapaPresentacion
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private void btnLoginSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Login_Load(object sender, EventArgs e)
        {
            ValidarProteccionHardware();
        }
        private void ValidarProteccionHardware()
        {
            string idPC = Utilidades.SeguridadHardware.ObtenerHardwareID();

            using (var context = new AppDbContext())
            {
                var negocio = context.Negocios.FirstOrDefault();

                if (negocio == null) return;

                string licenciaEnBD = negocio.Licencia ?? "";
                DateTime? fechaVence = negocio.FechaVencimiento;
                DateTime? ultimaVezAbierto = negocio.UltimaConexion;

                if (ultimaVezAbierto.HasValue && DateTime.Now < ultimaVezAbierto.Value)
                {
                    MessageBox.Show("Se ha detectado una inconsistencia en la fecha del sistema.\n" +
                                    "Por favor, sincronice la hora de Windows con Internet para continuar.",
                                    "Error de Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Application.Exit();
                    return;
                }

                negocio.UltimaConexion = DateTime.Now;
                context.SaveChanges();

                if (string.IsNullOrEmpty(licenciaEnBD) || !licenciaEnBD.Contains("-"))
                {
                    AbrirFormActivacion();
                    return;
                }

                string[] partes = licenciaEnBD.Split('-');
                if (partes.Length < 2) { AbrirFormActivacion(); return; }
                string hashEnBD = partes[0];
                string fechaStr = partes[1];
                string hashEsperado = Utilidades.SeguridadHardware.GenerarHashConFecha(idPC, fechaStr);

                if (hashEnBD != hashEsperado)
                {
                    AbrirFormActivacion();
                    return;
                }

                if (fechaVence.HasValue && DateTime.Now.Date > fechaVence.Value.Date)
                {
                    MessageBox.Show("Su suscripción ha expirado. Por favor, renueve su licencia.",
                                    "NovaSales", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    AbrirFormActivacion();

                    Application.Exit();
                    return;
                }

                if (fechaVence.HasValue)
                {
                    lbVencimiento.Text = $"Licencia válida hasta: {fechaVence.Value.ToString("dd/MM/yyyy")}";

                    if ((fechaVence.Value.Date - DateTime.Now.Date).TotalDays <= 5)
                    {
                        lbVencimiento.ForeColor = Color.DarkRed;
                        lbVencimiento.Text += " - [Renovar aquí]";
                    }
                }
            }
        }

        private void AbrirFormActivacion()
        {
            this.Hide();
            using (frmActivacion frm = new frmActivacion())
            {
                if (frm.ShowDialog() != DialogResult.OK) Application.Exit();
                else this.Show();
            }
        }
        private void btnLoginIngresar_Click(object sender, EventArgs e)
        {
            using (var context = new AppDbContext())
            {
                var service = new UsuarioService(context);
                var request = new LoginRequestDto
                {
                    Documento = tbUsuario.Text.Trim(),
                    Clave = tbContra.Text.Trim()
                };
                var usuario = service.Login(request);
                if (usuario != null)
                {
                    CapaPresentacion.Utilidades.Sesion.UsuarioActual = usuario;
                    Inicio inicio = new Inicio(usuario);
                    inicio.Show();
                    this.Hide();
                    inicio.FormClosing += frm_Closing;
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos", "Error de autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void frm_Closing(object sender, FormClosingEventArgs e)
        {
            tbUsuario.Text = "";
            tbContra.Text = "";
            this.Show();
        }

        private void lbVencimiento_Click(object sender, EventArgs e)
        {
            using (frmActivacion frm = new frmActivacion())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    ValidarProteccionHardware();
                    MessageBox.Show("Licencia actualizada correctamente.", "NovaSales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
