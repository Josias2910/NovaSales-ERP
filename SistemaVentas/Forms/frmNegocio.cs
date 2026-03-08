using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SistemaVentas.Data;
using CapaNegocio.Services;
using CapaNegocio.DTOs;
using CapaPresentacion.Utilidades;

namespace CapaPresentacion.Forms
{
    public partial class frmNegocio : Form
    {
        private bool datosCargados = false;
        private NegocioCreateDto datosOriginales;
        public frmNegocio()
        {
            InitializeComponent();
            VincularEventosCambio();
        }

        private void frmNegocio_Load(object sender, EventArgs e)
        {
            CargarDatosNegocio();
            btnGuardarNegocio.Enabled = false;
            datosCargados = true;
            GenerarLivePreview();
        }

        private void VincularEventosCambio()
        {
            tbNegocioNombre.TextChanged += ControlModificado;
            tbNegocioRUC.TextChanged += ControlModificado;
            tbNegocioDireccion.TextChanged += ControlModificado;
            tbNegocioTelefono.TextChanged += ControlModificado;
            tbNegocioCorreo.TextChanged += ControlModificado;
            tbNegocioSitio.TextChanged += ControlModificado;
            tbNegocioLema.TextChanged += ControlModificado;
            updNegocioPuntoVenta.ValueChanged += ControlModificado;
        }

        private void ControlModificado(object sender, EventArgs e)
        {
            ValidarCambiosReales();

            ReiniciarTimerPreview();
        }
        private void CargarDatosNegocio()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var negocioService = new NegocioService(context);
                    var datos = negocioService.ListarDatos();

                    if (datos != null)
                    {
                        datosCargados = false;
                        tbNegocioNombre.Text = datos.Nombre;
                        tbNegocioRUC.Text = datos.CUIT;
                        tbNegocioDireccion.Text = datos.Direccion;
                        tbNegocioTelefono.Text = datos.Telefono;
                        tbNegocioCorreo.Text = datos.Correo;
                        tbNegocioSitio.Text = datos.SitioWeb;
                        tbNegocioLema.Text = datos.Lema;
                        updNegocioPuntoVenta.Value = Convert.ToDecimal(datos.PuntoVenta);
                        picLogo.Image = datos.Logo != null ? ImagenHelper.ByteArrayToImage(datos.Logo) : null;
                        datosOriginales = datos;
                        datosCargados = true;
                        ValidarCambiosReales();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error cargando datos del negocio: " + ex.Message);
            }
        }
        private void ValidarCambiosReales()
        {
            if (!datosCargados || datosOriginales == null) return;

            bool huboCambios =
                tbNegocioNombre.Text != datosOriginales.Nombre ||
                tbNegocioRUC.Text != datosOriginales.CUIT ||
                tbNegocioDireccion.Text != datosOriginales.Direccion ||
                tbNegocioTelefono.Text != datosOriginales.Telefono ||
                tbNegocioCorreo.Text != datosOriginales.Correo ||
                tbNegocioSitio.Text != datosOriginales.SitioWeb ||
                tbNegocioLema.Text != datosOriginales.Lema ||
                updNegocioPuntoVenta.Value != Convert.ToDecimal(datosOriginales.PuntoVenta);

            btnGuardarNegocio.Enabled = huboCambios;
        }

        private IDocument ObtenerDocumento()
        {
            var negocioPreview = new NegocioCreateDto
            {
                Nombre = tbNegocioNombre.Text,
                CUIT = tbNegocioRUC.Text,
                Direccion = tbNegocioDireccion.Text,
                Telefono = tbNegocioTelefono.Text,
                Correo = tbNegocioCorreo.Text,
                SitioWeb = tbNegocioSitio.Text,
                Lema = tbNegocioLema.Text,
                Logo = picLogo.Image != null ? ImagenHelper.ImageToByteArray(picLogo.Image) : null
            };

            var compraPreview = new CompraCreateDto
            {
                TipoDocumento = "BOLETA DE PRUEBA",
                NumeroDocumento = $"{Convert.ToInt32(updNegocioPuntoVenta.Value):D4}-00000001",
                ProveedorId = 1,
                MontoTotal = 1500,
                Detalles = new List<DetalleCompraCreateDto>
        {
            new DetalleCompraCreateDto { Cantidad = 1, ProductoNombre = "PRODUCTO DE MUESTRA", PrecioCompra = 1500, MontoTotal = 1500 }
        }
            };

            return GeneradorReporte.CrearTicketCompra(negocioPreview, compraPreview);
        }

        private void GenerarLivePreview()
        {
            try
            {
                var documento = ObtenerDocumento();
                var imageStream = documento.GenerateImages().First();

                using (var ms = new MemoryStream(imageStream))
                {
                    picReportePreviw.Image = System.Drawing.Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en Preview: " + ex.Message);
            }
        }

        private void ReiniciarTimerPreview()
        {
            timerDebounce.Stop();
            timerDebounce.Start();
        }

        private void timerDebounce_Tick(object sender, EventArgs e)
        {
            timerDebounce.Stop();
            GenerarLivePreview();
        }

        private void tbNegocioNombre_TextChanged(object sender, EventArgs e) => ReiniciarTimerPreview();
        private void tbNegocioRUC_TextChanged(object sender, EventArgs e) => ReiniciarTimerPreview();
        private void tbNegocioDireccion_TextChanged(object sender, EventArgs e) => ReiniciarTimerPreview();
        private void tbNegocioCorreo_TextChanged(object sender, EventArgs e) => ReiniciarTimerPreview();
        private void tbNegocioTelefono_TextChanged(object sender, EventArgs e) => ReiniciarTimerPreview();
        private void tbNegocioSitio_TextChanged(object sender, EventArgs e) => ReiniciarTimerPreview();
        private void tbNegocioLema_TextChanged(object sender, EventArgs e) => ReiniciarTimerPreview();
        private void updNegocioPuntoVenta_ValueChanged(object sender, EventArgs e) => ReiniciarTimerPreview();


        private void btnGuardarNegocio_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbNegocioNombre.Text) || string.IsNullOrWhiteSpace(tbNegocioRUC.Text))
                {
                    MessageBox.Show("Nombre y RUC son campos obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var context = new AppDbContext())
                {
                    var negocioService = new NegocioService(context);

                    var dto = new NegocioCreateDto
                    {
                        Nombre = tbNegocioNombre.Text,
                        CUIT = tbNegocioRUC.Text,
                        PuntoVenta = Convert.ToInt32(updNegocioPuntoVenta.Value),
                        Direccion = tbNegocioDireccion.Text,
                        Telefono = tbNegocioTelefono.Text,
                        Correo = tbNegocioCorreo.Text,
                        SitioWeb = tbNegocioSitio.Text,
                        Lema = tbNegocioLema.Text,
                        Logo = picLogo.Image != null ? ImagenHelper.ImageToByteArray(picLogo.Image) : null
                    };

                    if (negocioService.GuardarCambios(dto))
                    {
                        MessageBox.Show("Configuración actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        btnGuardarNegocio.Enabled = false;
                        CargarDatosNegocio();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNegocioSubirLogo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFile = new OpenFileDialog())
            {
                openFile.Filter = "Imagenes (*.jpg; *.png; *.jpeg)|*.jpg;*.png;*.jpeg";
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(openFile.FileName);
                    picLogo.Image = ImagenHelper.ByteArrayToImage(fileBytes);
                    ControlModificado(sender, e);
                    GenerarLivePreview();
                }
            }
        }

        private void btnEliminarLogo_Click(object sender, EventArgs e)
        {
            if (picLogo.Image == null) return;

            try
            {
                using (var context = new AppDbContext())
                {
                    var negocioService = new NegocioService(context);
                    if (negocioService.EliminarLogo())
                    {
                        picLogo.Image = null;
                        GenerarLivePreview();
                        MessageBox.Show("Logo eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Archivo PDF (*.pdf)|*.pdf";
                sfd.FileName = $"Plantilla_{tbNegocioNombre.Text}.pdf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var documento = ObtenerDocumento();
                        documento.GeneratePdf(sfd.FileName);
                        MessageBox.Show("¡PDF exportado con éxito!", "NovaSales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al guardar: " + ex.Message);
                    }
                }
            }
        }
    }
}