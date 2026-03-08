using CapaNegocio.DTOs;
using CapaNegocio.Services;
using CapaPresentacion.Utilidades;
using SistemaVentas.Data;
using SistemaVentas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CapaPresentacion.Forms
{
    public partial class frmVentas : Form
    {
        private bool enModoEdicion = false;
        private object[] datosProductoOriginal;
        public frmVentas()
        {
            InitializeComponent();
        }

        private void frmVentas_Load(object sender, EventArgs e)
        {
            dgvVentas.AutoGenerateColumns = false;
            CargarTiposDocumentos();
            CargarMetodosPago();
        }
        private void CargarTiposDocumentos()
        {
            cbxVentaTipoDocumento.Items.Add("Boleta");
            cbxVentaTipoDocumento.Items.Add("Factura");
            cbxVentaTipoDocumento.Items.Add("Ticket");

            cbxVentaTipoDocumento.SelectedIndex = 0;

            cbxVentaTipoDocumento.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void CargarMetodosPago()
        {
            cbxMetodoPago.Items.Add("Efectivo");
            cbxMetodoPago.Items.Add("Credito");
            cbxMetodoPago.Items.Add("Debito");
            cbxMetodoPago.Items.Add("Transferencia");

            cbxMetodoPago.SelectedIndex = 0;

            cbxMetodoPago.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            using (var modal = new mdProducto())
            {
                var result = modal.ShowDialog();
                if (result == DialogResult.OK)
                {
                    tbIdProducto.Text = modal.Producto.Id.ToString();
                    tbVentaCodProducto.Text = modal.Producto.Codigo;
                    tbVentaProducto.Text = modal.Producto.Nombre;
                    tbVentaPrecio.Text = modal.Producto.PrecioVenta.ToString("0.00");
                    tbVentaStock.Text = modal.Producto.Stock.ToString();
                    tbVentaCodProducto.Enabled = false;
                    tbVentaPrecio.Enabled = true;
                    updVentaCantidad.Enabled = true;
                    updVentaCantidad.Focus();
                }
            }
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            using (var modal = new mdCliente())
            {
                var result = modal.ShowDialog();
                if (result == DialogResult.OK)
                {
                    tbIdCliente.Text = modal.Cliente.Id.ToString();
                    tbVentaNombreCompleto.Text = modal.Cliente.NombreCompleto;
                    tbVentaDocumento.Text = modal.Cliente.Documento;
                }
            }
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            decimal precio = 0;
            bool producto_existe = false;

            if (int.Parse(tbIdProducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!decimal.TryParse(tbVentaPrecio.Text, out precio))
            {
                MessageBox.Show("Precio - Formato incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (decimal.Parse(tbVentaPrecio.Text) < 0)
            {
                MessageBox.Show("El precio de venta no puede ser un numero negativo", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!int.TryParse(tbVentaStock.Text, out int stockActual) || stockActual < updVentaCantidad.Value)
            {
                MessageBox.Show("Stock no válido o insuficiente");
                return;
            }

            foreach (DataGridViewRow fila in dgvVentas.Rows)
            {
                if (fila.Cells["IdProducto"].Value.ToString() == tbIdProducto.Text)
                {
                    producto_existe = true;
                    break;
                }
            }

            if (!producto_existe)
            {
                decimal subtotal = updVentaCantidad.Value * precio;
                dgvVentas.Rows.Add(new object[] {
                    Properties.Resources.edit,
                    tbIdProducto.Text,
                    tbVentaCodProducto.Text,
                    tbVentaProducto.Text,
                    precio.ToString("0.00"),
                    tbVentaStock.Text,
                    updVentaCantidad.Value.ToString(),
                    subtotal.ToString("0.00"),
                    Properties.Resources.delete
                });

                btnAgregarProducto.Image = Properties.Resources.add;
                lbAgregar.Text = "Agregar";
                CalcularTotal();
                LimpiarCamposProducto();
            }
        }
        private void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow fila in dgvVentas.Rows)
                total += Convert.ToDecimal(fila.Cells["SubTotal"].Value);

            lblTotalPagar.Text = total.ToString("0.00");
            CalcularCambio();
        }
        private void CalcularCambio()
        {
            decimal total = decimal.Parse(lblTotalPagar.Text);
            decimal pago = 0;

            if (decimal.TryParse(tbPagaCon.Text, out pago))
            {
                if (pago >= total)
                    tbCambio.Text = (pago - total).ToString("0.00");
                else
                {
                    tbCambio.Text = "0.00";
                }
            }
            else
            {
                tbCambio.Text = "0.00";
            }
        }

        private void tbPagaCon_TextChanged(object sender, EventArgs e)
        {
            CalcularCambio();
        }

        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            if (int.Parse(tbIdCliente.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (dgvVentas.Rows.Count < 1)
            {
                MessageBox.Show("Debe ingresar productos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(tbPagaCon.Text) || decimal.Parse(tbPagaCon.Text) < decimal.Parse(lblTotalPagar.Text)) 
            { 
                MessageBox.Show("Debe ingresar un monto de pago válido", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (decimal.Parse(tbCambio.Text) < 0)
            {
                MessageBox.Show("El monto de pago es insuficiente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            using (var context = new AppDbContext())
            {
                var service = new VentaService(context);
                var nuevaVenta = new VentaCreateDto
                {
                    UsuarioId = Sesion.UsuarioActual.Id,
                    ClienteId = int.Parse(tbIdCliente.Text),
                    TipoDocumento = cbxVentaTipoDocumento.Text,
                    NumeroDocumento = tbVentaDocumento.Text,
                    MetodoPago = cbxMetodoPago.Text,
                    MontoTotal = decimal.Parse(lblTotalPagar.Text),
                    MontoPago = decimal.Parse(tbPagaCon.Text),
                    MontoCambio = decimal.Parse(tbCambio.Text),
                    Detalles = new List<DetalleVentaCreateDto>()
                };

                foreach (DataGridViewRow row in dgvVentas.Rows)
                {
                    nuevaVenta.Detalles.Add(new DetalleVentaCreateDto
                    {
                        ProductoId = Convert.ToInt32(row.Cells["IdProducto"].Value),
                        ProductoNombre = row.Cells["Producto"].Value.ToString(),
                        PrecioVenta = Convert.ToDecimal(row.Cells["Precio"].Value),
                        Cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value),
                        SubTotal = Convert.ToDecimal(row.Cells["SubTotal"].Value)
                    });
                }

                bool resultado = service.Registrar(nuevaVenta, out string mensaje);
                if (resultado)
                {
                    MessageBox.Show($"Registro Exitoso. Comprobante Nro: {mensaje}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var resp = MessageBox.Show("¿Desea generar el PDF de compra?", "Reporte", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resp == DialogResult.Yes)
                    {
                        Utilidades.ExportarPDF.DescargarVenta(nuevaVenta);
                    }

                    var confirmExcel = MessageBox.Show("¿Desea exportar esta carga a Excel?", "Sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmExcel == DialogResult.Yes)
                    {
                        Utilidades.ExportarExcel.Descargar(dgvVentas, $"Compra_{nuevaVenta.NumeroDocumento}");
                    }

                    tbIdCliente.Text = "0";
                    tbVentaDocumento.Text = "";
                    tbVentaNombreCompleto.Text = "";
                    dgvVentas.Rows.Clear();
                    CalcularTotal();
                    LimpiarTodo();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void LimpiarCamposProducto()
        {
            tbIdProducto.Text = "0";
            tbVentaCodProducto.Text = "";
            tbVentaProducto.Text = "";
            tbVentaPrecio.Text = "";
            tbVentaStock.Text = "";
            btnCancelar.Visible = false;
            lbCancelar.Visible = false;
            tbPagaCon.Enabled = true;
            tbVentaCodProducto.Enabled = true;
            updVentaCantidad.Enabled = false;
            updVentaCantidad.Value = 1;
        }

        private void LimpiarTodo()
        {
            tbIdCliente.Text = "0";
            tbVentaDocumento.Text = "";
            tbVentaNombreCompleto.Text = "";
            dgvVentas.Rows.Clear();
            tbPagaCon.Text = "";
            tbCambio.Text = "";
            CalcularTotal();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (enModoEdicion && datosProductoOriginal != null)
            {
                dgvVentas.Rows.Add(datosProductoOriginal);
                datosProductoOriginal = null;
                enModoEdicion = false;
                CalcularTotal();
            }
            btnAgregarProducto.Image = Properties.Resources.add;
            lbAgregar.Text = "Agregar";
            LimpiarCamposProducto();
        }

        private void btnLimpiarGrilla_Click(object sender, EventArgs e)
        {
            if (dgvVentas.Rows.Count > 0)
            {
                var result = MessageBox.Show("¿Está seguro de limpiar la lista de productos?", "Confirmar",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    dgvVentas.Rows.Clear();
                    CalcularTotal();
                }
            }
        }

        private void tbVentaCodProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                using (var context = new AppDbContext())
                {
                    var service = new ProductoService(context);
                    var producto = service.GetByCodigo(tbVentaCodProducto.Text);
                    if (producto != null)
                    {
                        tbIdProducto.Text = producto.Id.ToString();
                        tbVentaProducto.Text = producto.Nombre;
                        tbVentaPrecio.Text = producto.PrecioVenta.ToString();
                        tbVentaStock.Text = producto.Stock.ToString();
                        tbVentaPrecio.Enabled = true;
                        updVentaCantidad.Enabled = true;
                        btnAgregarProducto.Image = Properties.Resources.add;
                        lbAgregar.Text = "Agregar";
                        tbVentaPrecio.Focus();
                    }
                    else
                    {
                        LimpiarCamposProducto();
                        MessageBox.Show("Producto no encontrado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        tbVentaCodProducto.Focus();
                    }
                }
            }
        }

        private void dgvVentas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvVentas.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                if (MessageBox.Show("¿Desea eliminar este producto de la lista?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    btnAgregarProducto.Image = Properties.Resources.add;
                    lbAgregar.Text = "Agregar";
                    dgvVentas.Rows.RemoveAt(e.RowIndex);
                    CalcularTotal();
                }
            }

            if (dgvVentas.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                var fila = dgvVentas.Rows[e.RowIndex];

                datosProductoOriginal = new object[] {
                fila.Cells[0].Value,
                fila.Cells["IdProducto"].Value.ToString(),
                fila.Cells["Codigo"].Value.ToString(),
                fila.Cells["Producto"].Value.ToString(),
                fila.Cells["Precio"].Value.ToString(),
                fila.Cells["Stock"].Value.ToString(),
                fila.Cells["Cantidad"].Value.ToString(),
                fila.Cells["SubTotal"].Value.ToString(),
                fila.Cells[8].Value
                };
                enModoEdicion = true;

                tbIdProducto.Text = fila.Cells["IdProducto"].Value.ToString();
                tbVentaCodProducto.Text = fila.Cells["Codigo"].Value.ToString();
                tbVentaProducto.Text = fila.Cells["Producto"].Value.ToString();
                tbVentaPrecio.Text = fila.Cells["Precio"].Value.ToString();
                tbVentaStock.Text = fila.Cells["Stock"].Value.ToString();
                updVentaCantidad.Value = Convert.ToDecimal(fila.Cells["Cantidad"].Value);

                btnAgregarProducto.Image = Properties.Resources.arrow;
                lbAgregar.Text = "Actualizar";
                btnCancelar.Visible = true;
                lbCancelar.Visible = true;

                tbVentaCodProducto.Enabled = false;
                tbVentaPrecio.Enabled = true;
                updVentaCantidad.Enabled = true;

                dgvVentas.Rows.RemoveAt(e.RowIndex);
                CalcularTotal();
            }
        }
    }
}
