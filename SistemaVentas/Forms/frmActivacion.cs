using CapaNegocio.DTOs;
using CapaNegocio.Services;
using SistemaVentas.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace CapaPresentacion.Forms
{
    public partial class frmActivacion : Form
    {
        public frmActivacion()
        {
            InitializeComponent();
            tbHardwareId.Text = Utilidades.SeguridadHardware.ObtenerHardwareID();
        }

        private void tbId_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbHardwareId.Text))
            {
                Clipboard.SetText(tbHardwareId.Text);
                // Opcional: un pequeño tooltip o mensaje rápido
                MessageBox.Show("ID de equipo copiado al portapapeles.", "Copiado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnActivarSoftware_Click(object sender, EventArgs e)
        {
            string input = tbLicencia.Text.Trim();
            string salt = "B23UFKS8453K@SLF3NOVA"; // Tu Salt ultra-seguro

            // 1. Validación básica de formato (Debe ser HASH-FECHA)
            if (string.IsNullOrEmpty(input) || !input.Contains("-"))
            {
                MessageBox.Show("Formato de llave incorrecto.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string[] partes = input.Split('-');
                string hashDelCliente = partes[0];
                string fechaDelCliente = partes[1]; // Formato esperado: AAAAMMDD
                string idPC = tbHardwareId.Text;

                // 2. Re-calculamos el hash localmente para validar integridad
                string hashLocal = "";
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(idPC + salt + fechaDelCliente));
                    hashLocal = BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 16);
                }

                // 3. Verificación de seguridad
                if (hashDelCliente == hashLocal)
                {
                    // Convertimos la fecha del string a DateTime
                    DateTime fechaVence = DateTime.ParseExact(fechaDelCliente, "yyyyMMdd", null);

                    // 4. Guardado en Base de Datos
                    bool exito = false;
                    using (var context = new AppDbContext())
                    {
                        var service = new NegocioService(context);
                        exito = service.ActivarSoftware(input, fechaVence);
                    }

                    if (exito)
                    {
                        MessageBox.Show($"¡NovaSales activado con éxito!\nVencimiento: {fechaVence.ToLongDateString()}",
                                        "Activación", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo guardar la activación en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("La llave no es válida para este equipo o el código fue alterado.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al procesar la llave: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
