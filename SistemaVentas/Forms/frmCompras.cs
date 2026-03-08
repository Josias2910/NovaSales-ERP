using CapaNegocio.DTOs;
using CapaNegocio.Services;
using CapaPresentacion.Utilidades;
using DocumentFormat.OpenXml.Bibliography;
using QuestPDF.Fluent;
using SistemaVentas.Data;
using SistemaVentas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace CapaPresentacion.Forms
{
    public partial class frmCompras : Form
    {
        private bool enModoEdicion = false;
        private object[] datosProductoOriginal;
        public frmCompras()
        {
            InitializeComponent();
        }
        private void frmCompras_Load(object sender, EventArgs e)
        {
            CargarTiposDocumentos();
            CargarMetodosPago();
        }
        private void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            using (var modal = new mdProveedor())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    tbIdProveedor.Text = modal.Proveedor.Id.ToString();
                    tbCompraDocumento.Text = modal.Proveedor.Documento;
                    tbCompraRazonSocial.Text = modal.Proveedor.RazonSocial;
                }
            }
        }
        private void CargarTiposDocumentos()
        {
            cbxCompraTipoDocumento.Items.Add("Boleta");
            cbxCompraTipoDocumento.Items.Add("Factura");
            cbxCompraTipoDocumento.Items.Add("Ticket");

            cbxCompraTipoDocumento.SelectedIndex = 0;

            cbxCompraTipoDocumento.DropDownStyle = ComboBoxStyle.DropDownList;
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
                    tbCompraCodProducto.Text = modal.Producto.Codigo;
                    tbCompraProducto.Text = modal.Producto.Nombre;
                    tbCompraPrecioCompra.Enabled = true;
                    tbCompraPrecioVenta.Enabled = true;
                    updCompraCantidad.Enabled = true;
                }
            }
        }

        private void btnAgregarProduto_Click(object sender, EventArgs e)
        {
            decimal precioCompra = 0;
            decimal precioVenta = 0;
            bool producto_existe = false;

            if (int.Parse(tbIdProducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un producto primero", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!decimal.TryParse(tbCompraPrecioCompra.Text, out precioCompra))
            {
                MessageBox.Show("Precio Compra - Formato de moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbCompraPrecioCompra.Select();
                return;
            }

            if (!decimal.TryParse(tbCompraPrecioVenta.Text, out precioVenta))
            {
                MessageBox.Show("Precio Venta - Formato de moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tbCompraPrecioVenta.Select();
                return;
            }
            if (precioCompra <= 0)
            {
                MessageBox.Show("El precio de compra debe ser mayor a 0", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            foreach (DataGridViewRow fila in dgvCompras.Rows)
            {
                if (fila.Cells["IdProducto"].Value != null && fila.Cells["IdProducto"].Value.ToString() == tbIdProducto.Text)
                {
                    producto_existe = true;
                    break;
                }
            }

            if (producto_existe)
            {
                MessageBox.Show("El producto ya está en la lista", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!producto_existe)
            {
                decimal subtotal = updCompraCantidad.Value * precioCompra;
                Image iconoEditar = Properties.Resources.edit;
                Image iconoEliminar = Properties.Resources.delete;

                dgvCompras.Rows.Add(new object[] {
                iconoEditar,
                tbIdProducto.Text,
                tbCompraCodProducto.Text,
                tbCompraProducto.Text,
                precioCompra.ToString("0.00"),
                precioVenta.ToString("0.00"),
                updCompraCantidad.Value.ToString(),
                subtotal.ToString("0.00"),
                iconoEliminar
                });

                enModoEdicion = false;
                datosProductoOriginal = null;
                CalcularTotal();
                LimpiarCamposProducto();
                btnAgregarProduto.Image = Properties.Resources.add;
                lbAgregar.Text = "Agregar";
            }
        }
        private void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow fila in dgvCompras.Rows)
            {
                total += Convert.ToDecimal(fila.Cells["SubTotal"].Value);
            }
            lblTotalPagar.Text = total.ToString("0.00");
        }
        private void LimpiarCamposProducto()
        {
            tbIdProducto.Text = "0";
            tbCompraCodProducto.Text = "";
            tbCompraProducto.Text = "";
            tbCompraPrecioCompra.Text = "";
            tbCompraPrecioVenta.Text = "";
            updCompraCantidad.Value = 1;
            tbCompraCodProducto.Enabled = true;
            btnCancelar.Visible = false;
            lbCancelar.Visible = false;
            tbCompraPrecioCompra.Enabled = false;
            tbCompraPrecioVenta.Enabled = false;
            updCompraCantidad.Enabled = false;
            tbCompraCodProducto.Focus();
        }

        private void dgvCompras_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvCompras.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                if (MessageBox.Show("¿Desea eliminar este producto de la lista?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    btnAgregarProduto.Image = Properties.Resources.add;
                    lbAgregar.Text = "Agregar";
                    dgvCompras.Rows.RemoveAt(e.RowIndex);
                    CalcularTotal();
                }
            }

            if (dgvCompras.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                var fila = dgvCompras.Rows[e.RowIndex];

                datosProductoOriginal = new object[] {
                fila.Cells[0].Value,
                fila.Cells["IdProducto"].Value.ToString(),
                fila.Cells["Codigo"].Value.ToString(),
                fila.Cells["Producto"].Value.ToString(),
                fila.Cells["PrecioCompra"].Value.ToString(),
                fila.Cells["PrecioVenta"].Value.ToString(),
                fila.Cells["Cantidad"].Value.ToString(),
                fila.Cells["SubTotal"].Value.ToString(),
                fila.Cells[8].Value 
                };
                enModoEdicion = true;

                tbIdProducto.Text = fila.Cells["IdProducto"].Value.ToString();
                tbCompraCodProducto.Text = fila.Cells["Codigo"].Value.ToString();
                tbCompraProducto.Text = fila.Cells["Producto"].Value.ToString();
                tbCompraPrecioCompra.Text = fila.Cells["PrecioCompra"].Value.ToString();
                tbCompraPrecioVenta.Text = fila.Cells["PrecioVenta"].Value.ToString();
                updCompraCantidad.Value = Convert.ToDecimal(fila.Cells["Cantidad"].Value);
                btnAgregarProduto.Image = Properties.Resources.arrow;
                tbCompraCodProducto.Enabled = false;
                btnCancelar.Visible = true;
                lbCancelar.Visible = true;
                tbCompraPrecioCompra.Enabled = true;
                tbCompraPrecioVenta.Enabled = true;
                updCompraCantidad.Enabled = true;
                lbAgregar.Text = "Actualizar";

                dgvCompras.Rows.RemoveAt(e.RowIndex);
                CalcularTotal();
            }
        }

        private void tbCompraCodProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                using (var context = new AppDbContext())
                {
                    var service = new ProductoService(context);
                    var producto = service.GetByCodigo(tbCompraCodProducto.Text);
                    if (producto != null)
                    {
                        tbIdProducto.Text = producto.Id.ToString();
                        tbCompraProducto.Text = producto.Nombre;
                        tbCompraPrecioCompra.Enabled = true;
                        tbCompraPrecioVenta.Enabled = true;
                        updCompraCantidad.Enabled = true;
                        btnAgregarProduto.Image = Properties.Resources.add;
                        lbAgregar.Text = "Agregar";
                        tbCompraPrecioCompra.Focus();
                    }
                    else
                    {
                        LimpiarCamposProducto();
                        MessageBox.Show("Producto no encontrado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        tbCompraCodProducto.Focus();
                    }
                }
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (enModoEdicion && datosProductoOriginal != null)
            {
                dgvCompras.Rows.Add(datosProductoOriginal);
                datosProductoOriginal = null;
                enModoEdicion = false;
                CalcularTotal();
            }
            btnAgregarProduto.Image = Properties.Resources.add;
            lbAgregar.Text = "Agregar";
            LimpiarCamposProducto();
        }

        private void btnRegistrarCompra_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(tbIdProveedor.Text) == 0)
                {
                    MessageBox.Show("Debe seleccionar un proveedor", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (dgvCompras.Rows.Count < 1)
                {
                    MessageBox.Show("Debe agregar al menos un producto a la compra", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (cbxMetodoPago.SelectedIndex == -1)
                {
                    MessageBox.Show("Debe seleccionar un método de pago", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                using (var context = new AppDbContext())
                {
                    var service = new CompraService(context);
                    var nuevaCompra = new CompraCreateDto
                    {
                        UsuarioId = Sesion.UsuarioActual.Id,
                        ProveedorId = int.Parse(tbIdProveedor.Text),
                        TipoDocumento = cbxCompraTipoDocumento.Text,
                        MetodoPago = cbxMetodoPago.Text,
                        NumeroDocumento = tbCompraDocumento.Text,
                        MontoTotal = Convert.ToDecimal(lblTotalPagar.Text),
                        Detalles = new List<DetalleCompraCreateDto>()
                    };

                    foreach (DataGridViewRow row in dgvCompras.Rows)
                    {
                        nuevaCompra.Detalles.Add(new DetalleCompraCreateDto()
                        {
                            ProductoId = Convert.ToInt32(row.Cells[1].Value),
                            ProductoNombre = row.Cells[3].Value.ToString(),
                            PrecioCompra = Convert.ToDecimal(row.Cells[4].Value),
                            PrecioVenta = Convert.ToDecimal(row.Cells[5].Value),
                            Cantidad = Convert.ToInt32(row.Cells[6].Value),
                            MontoTotal = Convert.ToDecimal(row.Cells[7].Value)
                        });
                    }
                    bool resultado = service.Registrar(nuevaCompra, out string mensaje);
                    if (resultado)
                    {
                        MessageBox.Show($"Registro Exitoso. Comprobante Nro: {mensaje}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        var resp = MessageBox.Show("¿Desea generar el PDF de compra?", "Reporte", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (resp == DialogResult.Yes)
                        {
                            Utilidades.ExportarPDF.DescargarCompra(nuevaCompra);
                        }

                        var confirmExcel = MessageBox.Show("¿Desea exportar esta carga a Excel?", "Sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (confirmExcel == DialogResult.Yes)
                        {
                            Utilidades.ExportarExcel.Descargar(dgvCompras, $"Compra_{nuevaCompra.NumeroDocumento}");
                        }

                        tbIdProveedor.Text = "0";
                        tbCompraDocumento.Text = "";
                        tbCompraRazonSocial.Text = "";
                        dgvCompras.Rows.Clear();
                        CalcularTotal();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la compra: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiarGrilla_Click(object sender, EventArgs e)
        {
            if (dgvCompras.Rows.Count > 0)
            {
                var result = MessageBox.Show("¿Está seguro de limpiar la lista de productos?", "Confirmar",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    dgvCompras.Rows.Clear();
                    CalcularTotal();
                }
            }
        }
    }
}
