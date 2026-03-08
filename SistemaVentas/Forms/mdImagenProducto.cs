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
    public partial class mdImagenProducto : Form
    {
        private int _productoId;

        public mdImagenProducto(int idProducto, string nombreProducto)
        {
            InitializeComponent();
            _productoId = idProducto;
            this.Text = "Imagen de: " + nombreProducto;
        }

        private void mdImagenProducto_Load(object sender, EventArgs e)
        {
            CargarImagenActual();
        }

        private void btnSubirImagen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imágenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                ofd.Title = "Seleccionar imagen del producto";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        byte[] imagenBytes;

                        using (Image imgOriginal = Image.FromFile(ofd.FileName))
                        {
                            picImagen.Image = new Bitmap(imgOriginal);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                picImagen.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                imagenBytes = ms.ToArray();
                            }
                        }

                        using (var context = new AppDbContext())
                        {
                            var service = new ProductoService(context);
                            bool exito = service.ActualizarImagen(_productoId, imagenBytes, out string mensaje);

                            if (exito)
                            {
                                MessageBox.Show("Imagen actualizada y guardada correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.DialogResult = DialogResult.OK;
                            }
                            else
                            {
                                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No se pudo procesar la imagen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEliminarImagen_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("¿Está seguro de eliminar la imagen de este producto?",
                                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                using (var context = new AppDbContext())
                {
                    var service = new ProductoService(context);
                    bool exito = service.EliminarImagen(_productoId, out string mensaje);

                    if (exito)
                    {
                        picImagen.Image = null;
                        MessageBox.Show("Imagen eliminada correctamente.", "Sistema");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void CargarImagenActual()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var service = new ProductoService(context);
                    byte[] imagenBytes = service.ObtenerImagen(_productoId);

                    if (imagenBytes != null && imagenBytes.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(imagenBytes))
                        {
                            picImagen.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        picImagen.Image = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la imagen actual: " + ex.Message);
            }
        }
    }
}
