using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using CapaNegocio.DTOs;
using SistemaVentas.Domain.Entities;

public static class GeneradorReporte
{
    public static IDocument CrearTicketCompra(NegocioCreateDto negocio, CompraCreateDto compra)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                // Ajustamos el tamaño a uno más estándar de factura/ticket
                page.Size(400, 650);
                page.Margin(0.7f, Unit.Centimetre);
                page.PageColor(Colors.White);

                // --- HEADER ---
                page.Header().Column(headerCol =>
                {
                    headerCol.Item().Row(row =>
                    {
                        if (negocio.Logo != null && negocio.Logo.Length > 0)
                            row.ConstantItem(70).Image(negocio.Logo);

                        row.RelativeItem().PaddingLeft(10).Column(col =>
                        {
                            col.Item().Text(negocio.Nombre.ToUpper()).FontSize(15).ExtraBold().FontColor(Colors.Blue.Medium);
                            col.Item().Text($"CUIT: {negocio.CUIT}").FontSize(9).SemiBold();
                            col.Item().Text(negocio.Direccion).FontSize(8).FontColor(Colors.Grey.Darken2);
                        });

                        row.ConstantItem(100).Column(col =>
                        {
                            col.Item().Text("CONTACTO").FontSize(7).Bold().AlignCenter();
                            col.Item().LineHorizontal(0.5f);
                            col.Item().PaddingTop(2).Text($"📞 {negocio.Telefono}").FontSize(7);
                            col.Item().Text($"✉️ {negocio.Correo}").FontSize(6);
                            col.Item().Text($"🌐 {negocio.SitioWeb}").FontSize(6);
                        });
                    });

                    if (!string.IsNullOrWhiteSpace(negocio.Lema))
                        headerCol.Item().PaddingTop(5).AlignCenter().Text($"\"{negocio.Lema}\"").FontSize(8).Italic().FontColor(Colors.Grey.Medium);

                    headerCol.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Blue.Medium);
                });

                // --- CONTENIDO ---
                page.Content().PaddingVertical(10).Column(c =>
                {
                    c.Item().Row(row => {
                        row.RelativeItem().Column(col => {
                            col.Item().Text($"{compra.TipoDocumento.ToUpper()}").FontSize(10).ExtraBold();

                            // Usamos el NumeroDocumento que generó el Service
                            col.Item().Text($"NRO: {compra.NumeroDocumento}").FontSize(9).Bold();

                            // --- NUEVO: MÉTODO DE PAGO ---
                            col.Item().PaddingTop(2).Text(txt => {
                                txt.Span("MÉTODO DE PAGO: ").FontSize(8).Bold();
                                txt.Span($"{compra.MetodoPago ?? "S/D"}").FontSize(8);
                            });
                        });

                        row.RelativeItem().AlignRight().Column(col => {
                            col.Item().Text($"FECHA: {DateTime.Now:dd/MM/yyyy}").FontSize(8);
                            col.Item().Text($"HORA: {DateTime.Now:HH:mm}").FontSize(8);
                        });
                    });

                    // Tabla de Productos
                    c.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns => {
                            columns.ConstantColumn(35);
                            columns.RelativeColumn();
                            columns.ConstantColumn(65); // Un poco más ancho para precios
                            columns.ConstantColumn(65);
                        });

                        table.Header(h => {
                            h.Cell().Element(EstiloHeader).Text("CANT");
                            h.Cell().Element(EstiloHeader).Text("PRODUCTO");
                            h.Cell().Element(EstiloHeader).AlignRight().Text("P. UNIT");
                            h.Cell().Element(EstiloHeader).AlignRight().Text("SUBTOTAL");
                        });

                        foreach (var det in compra.Detalles)
                        {
                            table.Cell().Element(EstiloCelda).AlignCenter().Text(det.Cantidad.ToString());
                            table.Cell().Element(EstiloCelda).Text(det.ProductoNombre ?? "S/D");
                            table.Cell().Element(EstiloCelda).AlignRight().Text(det.PrecioCompra.ToString("N2"));
                            table.Cell().Element(EstiloCelda).AlignRight().Text(det.MontoTotal.ToString("N2"));
                        }
                    });

                    // Resumen
                    c.Item().PaddingTop(10).Row(row => {
                        row.RelativeItem().Column(col => {
                            col.Item().Text($"SON: {NumeroALetras(compra.MontoTotal)}").FontSize(7).Bold().Italic();
                            col.Item().PaddingTop(5).Text("Observaciones: Mercadería sujeta a revisión conforme a remito.").FontSize(7).FontColor(Colors.Grey.Medium);
                        });

                        row.ConstantItem(130).AlignRight().Column(col => {
                            col.Item().Text($"TOTAL COMPRA:").FontSize(8).Bold();
                            col.Item().Text($"$ {compra.MontoTotal:N2}")
                                     .FontSize(14).ExtraBold().FontColor(Colors.Blue.Medium);
                        });
                    });

                    // ESPACIO DE FIRMA (Fundamental en Compras)
                    c.Item().PaddingTop(40).Row(row => {
                        row.RelativeItem().PaddingHorizontal(20).Column(col => {
                            col.Item().LineHorizontal(0.5f);
                            col.Item().AlignCenter().Text("ENTREGADO (PROVEEDOR)").FontSize(7);
                        });
                        row.RelativeItem().PaddingHorizontal(20).Column(col => {
                            col.Item().LineHorizontal(0.5f);
                            col.Item().AlignCenter().Text("RECIBIDO (DEPÓSITO)").FontSize(7);
                        });
                    });
                });

                // FOOTER
                page.Footer().Column(f => {
                    f.Item().AlignCenter().Text("NovaSales ERP - Sistema de Gestión Interna").FontSize(6).FontColor(Colors.Grey.Medium);
                    f.Item().AlignCenter().Text(x => {
                        x.Span("Página ").FontSize(6);
                        x.CurrentPageNumber().FontSize(6);
                    });
                });
            });
        });
    }


    public static IDocument CrearTicketCompra(NegocioCreateDto negocio, CompraDetalleDto compra)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                // Ajustamos el tamaño a uno más estándar de factura/ticket
                page.Size(400, 650);
                page.Margin(0.7f, Unit.Centimetre);
                page.PageColor(Colors.White);

                // --- HEADER ---
                page.Header().Column(headerCol =>
                {
                    headerCol.Item().Row(row =>
                    {
                        if (negocio.Logo != null && negocio.Logo.Length > 0)
                            row.ConstantItem(70).Image(negocio.Logo);

                        row.RelativeItem().PaddingLeft(10).Column(col =>
                        {
                            col.Item().Text(negocio.Nombre.ToUpper()).FontSize(15).ExtraBold().FontColor(Colors.Blue.Medium);
                            col.Item().Text($"CUIT: {negocio.CUIT}").FontSize(9).SemiBold();
                            col.Item().Text(negocio.Direccion).FontSize(8).FontColor(Colors.Grey.Darken2);
                        });

                        row.ConstantItem(100).Column(col =>
                        {
                            col.Item().Text("CONTACTO").FontSize(7).Bold().AlignCenter();
                            col.Item().LineHorizontal(0.5f);
                            col.Item().PaddingTop(2).Text($"📞 {negocio.Telefono}").FontSize(7);
                            col.Item().Text($"✉️ {negocio.Correo}").FontSize(6);
                            col.Item().Text($"🌐 {negocio.SitioWeb}").FontSize(6);
                        });
                    });

                    if (!string.IsNullOrWhiteSpace(negocio.Lema))
                        headerCol.Item().PaddingTop(5).AlignCenter().Text($"\"{negocio.Lema}\"").FontSize(8).Italic().FontColor(Colors.Grey.Medium);

                    headerCol.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Blue.Medium);
                });

                // --- CONTENIDO ---
                page.Content().PaddingVertical(10).Column(c =>
                {
                    c.Item().Row(row => {
                        row.RelativeItem().Column(col => {
                            col.Item().Text($"{compra.TipoDocumento.ToUpper()}").FontSize(10).ExtraBold();

                            // Usamos el NumeroDocumento que generó el Service
                            col.Item().Text($"NRO: {compra.NumeroDocumento}").FontSize(9).Bold();

                            // --- NUEVO: MÉTODO DE PAGO ---
                            col.Item().PaddingTop(2).Text(txt => {
                                txt.Span("MÉTODO DE PAGO: ").FontSize(8).Bold();
                                txt.Span($"{compra.MetodoPago ?? "S/D"}").FontSize(8);
                            });
                        });

                        row.RelativeItem().AlignRight().Column(col => {
                            col.Item().Text($"FECHA: {DateTime.Now:dd/MM/yyyy}").FontSize(8);
                            col.Item().Text($"HORA: {DateTime.Now:HH:mm}").FontSize(8);
                        });
                    });

                    // Tabla de Productos
                    c.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns => {
                            columns.ConstantColumn(35);
                            columns.RelativeColumn();
                            columns.ConstantColumn(65); // Un poco más ancho para precios
                            columns.ConstantColumn(65);
                        });

                        table.Header(h => {
                            h.Cell().Element(EstiloHeader).Text("CANT");
                            h.Cell().Element(EstiloHeader).Text("PRODUCTO");
                            h.Cell().Element(EstiloHeader).AlignRight().Text("P. UNIT");
                            h.Cell().Element(EstiloHeader).AlignRight().Text("SUBTOTAL");
                        });

                        foreach (var det in compra.Detalles)
                        {
                            table.Cell().Element(EstiloCelda).AlignCenter().Text(det.Cantidad.ToString());
                            table.Cell().Element(EstiloCelda).Text(det.ProductoNombre ?? "S/D");
                            table.Cell().Element(EstiloCelda).AlignRight().Text(det.PrecioCompra.ToString("N2"));
                            table.Cell().Element(EstiloCelda).AlignRight().Text(det.MontoTotal.ToString("N2"));
                        }
                    });

                    // Resumen
                    c.Item().PaddingTop(10).Row(row => {
                        row.RelativeItem().Column(col => {
                            col.Item().Text($"SON: {NumeroALetras(compra.MontoTotal)}").FontSize(7).Bold().Italic();
                            col.Item().PaddingTop(5).Text("Observaciones: Mercadería sujeta a revisión conforme a remito.").FontSize(7).FontColor(Colors.Grey.Medium);
                        });

                        row.ConstantItem(130).AlignRight().Column(col => {
                            col.Item().Text($"TOTAL COMPRA:").FontSize(8).Bold();
                            col.Item().Text($"$ {compra.MontoTotal:N2}")
                                     .FontSize(14).ExtraBold().FontColor(Colors.Blue.Medium);
                        });
                    });

                    // ESPACIO DE FIRMA (Fundamental en Compras)
                    c.Item().PaddingTop(40).Row(row => {
                        row.RelativeItem().PaddingHorizontal(20).Column(col => {
                            col.Item().LineHorizontal(0.5f);
                            col.Item().AlignCenter().Text("ENTREGADO (PROVEEDOR)").FontSize(7);
                        });
                        row.RelativeItem().PaddingHorizontal(20).Column(col => {
                            col.Item().LineHorizontal(0.5f);
                            col.Item().AlignCenter().Text("RECIBIDO (DEPÓSITO)").FontSize(7);
                        });
                    });
                });

                // FOOTER
                page.Footer().Column(f => {
                    f.Item().AlignCenter().Text("NovaSales ERP - Sistema de Gestión Interna").FontSize(6).FontColor(Colors.Grey.Medium);
                    f.Item().AlignCenter().Text(x => {
                        x.Span("Página ").FontSize(6);
                        x.CurrentPageNumber().FontSize(6);
                    });
                });
            });
        });
    }

    public static IDocument CrearTicketVenta(NegocioCreateDto negocio, VentaCreateDto venta)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                // Ajustamos el tamaño a uno más estándar de factura/ticket
                page.Size(400, 650);
                page.Margin(0.7f, Unit.Centimetre);
                page.PageColor(Colors.White);

                // --- HEADER ---
                page.Header().Column(headerCol =>
                {
                    headerCol.Item().Row(row =>
                    {
                        if (negocio.Logo != null && negocio.Logo.Length > 0)
                            row.ConstantItem(70).Image(negocio.Logo);

                        row.RelativeItem().PaddingLeft(10).Column(col =>
                        {
                            col.Item().Text(negocio.Nombre.ToUpper()).FontSize(15).ExtraBold().FontColor(Colors.Blue.Medium);
                            col.Item().Text($"CUIT: {negocio.CUIT}").FontSize(9).SemiBold();
                            col.Item().Text(negocio.Direccion).FontSize(8).FontColor(Colors.Grey.Darken2);
                        });

                        row.ConstantItem(100).Column(col =>
                        {
                            col.Item().Text("CONTACTO").FontSize(7).Bold().AlignCenter();
                            col.Item().LineHorizontal(0.5f);
                            col.Item().PaddingTop(2).Text($"📞 {negocio.Telefono}").FontSize(7);
                            col.Item().Text($"✉️ {negocio.Correo}").FontSize(6);
                            col.Item().Text($"🌐 {negocio.SitioWeb}").FontSize(6);
                        });
                    });

                    if (!string.IsNullOrWhiteSpace(negocio.Lema))
                        headerCol.Item().PaddingTop(5).AlignCenter().Text($"\"{negocio.Lema}\"").FontSize(8).Italic().FontColor(Colors.Grey.Medium);

                    headerCol.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Blue.Medium);
                });

                // --- CONTENIDO ---
                page.Content().PaddingVertical(10).Column(c =>
                {
                    c.Item().Row(row => {
                        row.RelativeItem().Column(col => {
                            col.Item().Text($"{venta.TipoDocumento.ToUpper()}").FontSize(10).ExtraBold();

                            // Usamos el NumeroDocumento que generó el Service
                            col.Item().Text($"NRO: {venta.NumeroDocumento}").FontSize(9).Bold();

                            // --- NUEVO: MÉTODO DE PAGO ---
                            col.Item().PaddingTop(2).Text(txt => {
                                txt.Span("MÉTODO DE PAGO: ").FontSize(8).Bold();
                                txt.Span($"{venta.MetodoPago ?? "S/D"}").FontSize(8);
                            });
                        });

                        row.RelativeItem().AlignRight().Column(col => {
                            col.Item().Text($"FECHA: {DateTime.Now:dd/MM/yyyy}").FontSize(8);
                            col.Item().Text($"HORA: {DateTime.Now:HH:mm}").FontSize(8);
                        });
                    });

                    // Tabla de Productos
                    c.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns => {
                            columns.ConstantColumn(35);
                            columns.RelativeColumn();
                            columns.ConstantColumn(65); // Un poco más ancho para precios
                            columns.ConstantColumn(65);
                        });

                        table.Header(h => {
                            h.Cell().Element(EstiloHeader).Text("CANT");
                            h.Cell().Element(EstiloHeader).Text("PRODUCTO");
                            h.Cell().Element(EstiloHeader).AlignRight().Text("P. UNIT");
                            h.Cell().Element(EstiloHeader).AlignRight().Text("SUBTOTAL");
                        });

                        foreach (var det in venta.Detalles)
                        {
                            table.Cell().Element(EstiloCelda).AlignCenter().Text(det.Cantidad.ToString());
                            table.Cell().Element(EstiloCelda).Text(det.ProductoNombre ?? "S/D");
                            table.Cell().Element(EstiloCelda).AlignRight().Text(det.PrecioVenta.ToString("N2"));
                            table.Cell().Element(EstiloCelda).AlignRight().Text(det.SubTotal.ToString("N2"));
                        }
                    });

                    // Resumen
                    c.Item().PaddingTop(10).Row(row => {
                        row.RelativeItem().Column(col => {
                            col.Item().Text($"SON: {NumeroALetras(venta.MontoTotal)}").FontSize(7).Bold().Italic();
                            col.Item().PaddingTop(5).Text("Observaciones: Mercadería sujeta a revisión conforme a remito.").FontSize(7).FontColor(Colors.Grey.Medium);
                        });

                        row.ConstantItem(130).AlignRight().Column(col => {
                            col.Item().Text($"TOTAL VENTA:").FontSize(8).Bold();
                            col.Item().Text($"$ {venta.MontoTotal:N2}")
                                     .FontSize(14).ExtraBold().FontColor(Colors.Blue.Medium);
                        });
                    });

                    // ESPACIO DE FIRMA (Fundamental en Compras)
                    c.Item().PaddingTop(40).Row(row => {
                        row.RelativeItem().PaddingHorizontal(20).Column(col => {
                            col.Item().LineHorizontal(0.5f);
                            col.Item().AlignCenter().Text("ENTREGADO (PROVEEDOR)").FontSize(7);
                        });
                        row.RelativeItem().PaddingHorizontal(20).Column(col => {
                            col.Item().LineHorizontal(0.5f);
                            col.Item().AlignCenter().Text("RECIBIDO (DEPÓSITO)").FontSize(7);
                        });
                    });
                });

                // FOOTER
                page.Footer().Column(f => {
                    f.Item().AlignCenter().Text("NovaSales ERP - Sistema de Gestión Interna").FontSize(6).FontColor(Colors.Grey.Medium);
                    f.Item().AlignCenter().Text(x => {
                        x.Span("Página ").FontSize(6);
                        x.CurrentPageNumber().FontSize(6);
                    });
                });
            });
        });
    }

    public static IDocument CrearTicketVenta(NegocioCreateDto negocio, VentaDetalleDto venta)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                // Ajustamos el tamaño a uno más estándar de factura/ticket
                page.Size(400, 650);
                page.Margin(0.7f, Unit.Centimetre);
                page.PageColor(Colors.White);

                // --- HEADER ---
                page.Header().Column(headerCol =>
                {
                    headerCol.Item().Row(row =>
                    {
                        if (negocio.Logo != null && negocio.Logo.Length > 0)
                            row.ConstantItem(70).Image(negocio.Logo);

                        row.RelativeItem().PaddingLeft(10).Column(col =>
                        {
                            col.Item().Text(negocio.Nombre.ToUpper()).FontSize(15).ExtraBold().FontColor(Colors.Blue.Medium);
                            col.Item().Text($"CUIT: {negocio.CUIT}").FontSize(9).SemiBold();
                            col.Item().Text(negocio.Direccion).FontSize(8).FontColor(Colors.Grey.Darken2);
                        });

                        row.ConstantItem(100).Column(col =>
                        {
                            col.Item().Text("CONTACTO").FontSize(7).Bold().AlignCenter();
                            col.Item().LineHorizontal(0.5f);
                            col.Item().PaddingTop(2).Text($"📞 {negocio.Telefono}").FontSize(7);
                            col.Item().Text($"✉️ {negocio.Correo}").FontSize(6);
                            col.Item().Text($"🌐 {negocio.SitioWeb}").FontSize(6);
                        });
                    });

                    if (!string.IsNullOrWhiteSpace(negocio.Lema))
                        headerCol.Item().PaddingTop(5).AlignCenter().Text($"\"{negocio.Lema}\"").FontSize(8).Italic().FontColor(Colors.Grey.Medium);

                    headerCol.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Blue.Medium);
                });

                // --- CONTENIDO ---
                page.Content().PaddingVertical(10).Column(c =>
                {
                    c.Item().Row(row => {
                        row.RelativeItem().Column(col => {
                            col.Item().Text($"{venta.TipoDocumento.ToUpper()}").FontSize(10).ExtraBold();

                            // Usamos el NumeroDocumento que generó el Service
                            col.Item().Text($"NRO: {venta.NumeroDocumento}").FontSize(9).Bold();

                            // --- NUEVO: MÉTODO DE PAGO ---
                            col.Item().PaddingTop(2).Text(txt => {
                                txt.Span("MÉTODO DE PAGO: ").FontSize(8).Bold();
                                txt.Span($"{venta.MetodoPago ?? "S/D"}").FontSize(8);
                            });
                        });

                        row.RelativeItem().AlignRight().Column(col => {
                            col.Item().Text($"FECHA: {DateTime.Now:dd/MM/yyyy}").FontSize(8);
                            col.Item().Text($"HORA: {DateTime.Now:HH:mm}").FontSize(8);
                        });
                    });

                    // Tabla de Productos
                    c.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns => {
                            columns.ConstantColumn(35);
                            columns.RelativeColumn();
                            columns.ConstantColumn(65); // Un poco más ancho para precios
                            columns.ConstantColumn(65);
                        });

                        table.Header(h => {
                            h.Cell().Element(EstiloHeader).Text("CANT");
                            h.Cell().Element(EstiloHeader).Text("PRODUCTO");
                            h.Cell().Element(EstiloHeader).AlignRight().Text("P. UNIT");
                            h.Cell().Element(EstiloHeader).AlignRight().Text("SUBTOTAL");
                        });

                        foreach (var det in venta.Detalles)
                        {
                            table.Cell().Element(EstiloCelda).AlignCenter().Text(det.Cantidad.ToString());
                            table.Cell().Element(EstiloCelda).Text(det.ProductoNombre ?? "S/D");
                            table.Cell().Element(EstiloCelda).AlignRight().Text(det.PrecioVenta.ToString("N2"));
                            table.Cell().Element(EstiloCelda).AlignRight().Text(det.MontoTotal.ToString("N2"));
                        }
                    });

                    // Resumen
                    c.Item().PaddingTop(10).Row(row => {
                        row.RelativeItem().Column(col => {
                            col.Item().Text($"SON: {NumeroALetras(venta.MontoTotal)}").FontSize(7).Bold().Italic();
                            col.Item().PaddingTop(5).Text("Observaciones: Mercadería sujeta a revisión conforme a remito.").FontSize(7).FontColor(Colors.Grey.Medium);
                        });

                        row.ConstantItem(130).AlignRight().Column(col => {
                            col.Item().Text($"TOTAL VENTA:").FontSize(8).Bold();
                            col.Item().Text($"$ {venta.MontoTotal:N2}")
                                     .FontSize(14).ExtraBold().FontColor(Colors.Blue.Medium);
                        });
                    });

                    // ESPACIO DE FIRMA (Fundamental en Compras)
                    c.Item().PaddingTop(40).Row(row => {
                        row.RelativeItem().PaddingHorizontal(20).Column(col => {
                            col.Item().LineHorizontal(0.5f);
                            col.Item().AlignCenter().Text("ENTREGADO (PROVEEDOR)").FontSize(7);
                        });
                        row.RelativeItem().PaddingHorizontal(20).Column(col => {
                            col.Item().LineHorizontal(0.5f);
                            col.Item().AlignCenter().Text("RECIBIDO (DEPÓSITO)").FontSize(7);
                        });
                    });
                });

                // FOOTER
                page.Footer().Column(f => {
                    f.Item().AlignCenter().Text("NovaSales ERP - Sistema de Gestión Interna").FontSize(6).FontColor(Colors.Grey.Medium);
                    f.Item().AlignCenter().Text(x => {
                        x.Span("Página ").FontSize(6);
                        x.CurrentPageNumber().FontSize(6);
                    });
                });
            });
        });
    }

    private static IContainer EstiloHeader(IContainer container) =>
        container.Background(Colors.Grey.Lighten3).Padding(2).DefaultTextStyle(x => x.FontSize(7).Bold());

    private static IContainer EstiloCelda(IContainer container) =>
        container.BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(2).DefaultTextStyle(x => x.FontSize(7));

    private static string NumeroALetras(decimal total)
    {
        long entero = (long)total;
        int centavos = (int)((total - entero) * 100);
        return $"{entero} PESOS CON {centavos:00}/100 CTVS.";
    }
}