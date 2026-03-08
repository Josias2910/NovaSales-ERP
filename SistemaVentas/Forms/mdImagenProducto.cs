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
        private int _productoId; // Aquí guardaremos el ID

        // Modificamos el constructor para que pida el ID
        public mdImagenProducto(int idProducto, string nombreProducto)
        {
            InitializeComponent();
            _productoId = idProducto;
            this.Text = "Imagen de: " + nombreProducto; // Un detalle pro
        }

        private void mdImagenProducto_Load(object sender, EventArgs e)
        {
            CargarImagenActual(); // Cargamos la imagen actual al abrir el modal
        }

        private void btnSubirImagen_Click(object sender, EventArgs e)
        {
            // 1. Abrimos el buscador de archivos inmediatamente
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imágenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                ofd.Title = "Seleccionar imagen del producto";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        byte[] imagenBytes;

                        // 2. Cargamos el archivo seleccionado y lo convertimos a Bytes
                        // Usamos 'using' para asegurar que el archivo no quede bloqueado en el disco
                        using (Image imgOriginal = Image.FromFile(ofd.FileName))
                        {
                            // Mostramos la imagen en el PictureBox para que el usuario vea qué subió
                            picImagen.Image = new Bitmap(imgOriginal);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                // Guardamos como Jpeg para optimizar tamaño en la base de datos
                                picImagen.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                imagenBytes = ms.ToArray();
                            }
                        }

                        // 3. Guardado directo en Base de Datos vía Service
                        using (var context = new AppDbContext())
                        {
                            var service = new ProductoService(context);
                            bool exito = service.ActualizarImagen(_productoId, imagenBytes, out string mensaje);

                            if (exito)
                            {
                                MessageBox.Show("Imagen actualizada y guardada correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.DialogResult = DialogResult.OK;
                                // Opcional: podrías cerrar el form o dejarlo abierto para que vea el resultado
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
                        picImagen.Image = null; // Limpiamos la vista
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
                        // El truco: Convertir los bytes de la BD en una Imagen de C#
                        using (MemoryStream ms = new MemoryStream(imagenBytes))
                        {
                            picImagen.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        // Si no hay imagen, podés poner una por defecto o dejarlo vacío
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
