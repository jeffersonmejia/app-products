using CrudProductos.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudProductos.Data
{
    public static class DbInitializer
    {
        private static readonly Dictionary<string, decimal> DescuentosPorCategoria = new()
        {
            ["Computadoras"] = 5.00m,
            ["Monitores"] = 7.50m,
            ["Perifericos"] = 10.00m,
            ["Almacenamiento"] = 8.00m,
            ["Redes"] = 6.00m,
            ["Impresion"] = 4.50m,
            ["Software"] = 12.00m,
            ["Mobiliario"] = 3.00m
        };

        public static async Task ApplyMigrationsAndSeedAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();
            await SeedCategoriasAsync(context);
            await SeedProductosAsync(context);
        }

        private static async Task SeedProductosAsync(AppDbContext context)
        {
            var productos = new List<Producto>
            {
                new() { Nombre = "Laptop Lenovo ThinkPad E14", Descripcion = "Laptop empresarial para desarrollo y oficina", Precio = 850.00m, Stock = 12 },
                new() { Nombre = "Laptop HP Pavilion 15", Descripcion = "Equipo portatil para estudiantes universitarios", Precio = 720.50m, Stock = 8 },
                new() { Nombre = "Laptop Dell Inspiron 14", Descripcion = "Laptop de uso academico y administrativo", Precio = 690.00m, Stock = 5 },
                new() { Nombre = "MacBook Air M2", Descripcion = "Laptop ultraligera para diseno y programacion", Precio = 1250.00m, Stock = 3 },
                new() { Nombre = "Monitor Samsung 24 pulgadas", Descripcion = "Monitor Full HD para oficina y laboratorio", Precio = 180.00m, Stock = 15 },
                new() { Nombre = "Monitor LG UltraWide 29 pulgadas", Descripcion = "Monitor panoramico para productividad", Precio = 320.00m, Stock = 6 },
                new() { Nombre = "Teclado Logitech K120", Descripcion = "Teclado USB basico para laboratorio", Precio = 14.99m, Stock = 40 },
                new() { Nombre = "Teclado Mecanico Redragon Kumara", Descripcion = "Teclado mecanico para programacion y gaming", Precio = 55.00m, Stock = 10 },
                new() { Nombre = "Mouse Logitech M170", Descripcion = "Mouse inalambrico compacto", Precio = 12.50m, Stock = 35 },
                new() { Nombre = "Mouse Gamer Razer DeathAdder", Descripcion = "Mouse ergonomico de alta precision", Precio = 65.00m, Stock = 7 },
                new() { Nombre = "Disco SSD Kingston 480GB", Descripcion = "Unidad de estado solido SATA para computadoras", Precio = 42.00m, Stock = 20 },
                new() { Nombre = "Disco SSD NVMe Samsung 1TB", Descripcion = "Unidad NVMe de alto rendimiento", Precio = 110.00m, Stock = 9 },
                new() { Nombre = "Memoria RAM Kingston 8GB DDR4", Descripcion = "Memoria RAM para equipos de escritorio", Precio = 35.00m, Stock = 25 },
                new() { Nombre = "Memoria RAM Corsair 16GB DDR4", Descripcion = "Memoria RAM para estaciones de trabajo", Precio = 72.00m, Stock = 11 },
                new() { Nombre = "Router TP-Link Archer C6", Descripcion = "Router inalambrico dual band para red local", Precio = 48.00m, Stock = 13 },
                new() { Nombre = "Switch TP-Link 8 puertos", Descripcion = "Switch de red para laboratorio", Precio = 28.50m, Stock = 18 },
                new() { Nombre = "Cable HDMI 2 metros", Descripcion = "Cable HDMI para proyector o monitor", Precio = 8.00m, Stock = 60 },
                new() { Nombre = "Cable de red Cat6 3 metros", Descripcion = "Cable Ethernet para conexion LAN", Precio = 4.50m, Stock = 75 },
                new() { Nombre = "Impresora Epson EcoTank L3250", Descripcion = "Impresora multifuncional con sistema continuo", Precio = 235.00m, Stock = 4 },
                new() { Nombre = "Impresora HP LaserJet Pro", Descripcion = "Impresora laser para oficina", Precio = 290.00m, Stock = 2 },
                new() { Nombre = "Webcam Logitech C920", Descripcion = "Camara web Full HD para clases virtuales", Precio = 89.00m, Stock = 6 },
                new() { Nombre = "Microfono Fifine USB", Descripcion = "Microfono para grabacion de clases y reuniones", Precio = 49.00m, Stock = 9 },
                new() { Nombre = "Audifonos JBL Tune 510BT", Descripcion = "Audifonos inalambricos para uso diario", Precio = 45.00m, Stock = 14 },
                new() { Nombre = "Parlantes Genius USB", Descripcion = "Parlantes compactos para computador", Precio = 18.00m, Stock = 22 },
                new() { Nombre = "Tablet Samsung Galaxy Tab A9", Descripcion = "Tablet para lectura, clases y navegacion", Precio = 210.00m, Stock = 5 },
                new() { Nombre = "Proyector Epson X49", Descripcion = "Proyector para aula y presentaciones", Precio = 620.00m, Stock = 1 },
                new() { Nombre = "UPS Forza 1000VA", Descripcion = "Sistema de respaldo electrico para equipos", Precio = 95.00m, Stock = 7 },
                new() { Nombre = "Regulador de voltaje CDP", Descripcion = "Proteccion electrica para computadoras", Precio = 28.00m, Stock = 16 },
                new() { Nombre = "Licencia Antivirus ESET", Descripcion = "Licencia anual de seguridad informatica", Precio = 32.00m, Stock = 30 },
                new() { Nombre = "Licencia Microsoft 365 Personal", Descripcion = "Suscripcion para herramientas de oficina", Precio = 69.00m, Stock = 12 },
                new() { Nombre = "Silla ergonomica ejecutiva", Descripcion = "Silla para oficina con soporte lumbar", Precio = 145.00m, Stock = 6 },
                new() { Nombre = "Escritorio metalico moderno", Descripcion = "Escritorio para laboratorio o aula", Precio = 180.00m, Stock = 4 },
                new() { Nombre = "Pizarra acrilica 120x80", Descripcion = "Pizarra blanca para clases", Precio = 55.00m, Stock = 8 },
                new() { Nombre = "Marcadores borrables paquete x4", Descripcion = "Marcadores para pizarra acrilica", Precio = 6.50m, Stock = 50 },
                new() { Nombre = "Pendrive Kingston 64GB", Descripcion = "Unidad USB para respaldo de archivos", Precio = 9.50m, Stock = 45 },
                new() { Nombre = "Disco externo Seagate 2TB", Descripcion = "Disco duro externo para copias de seguridad", Precio = 88.00m, Stock = 10 },
                new() { Nombre = "Adaptador USB-C a HDMI", Descripcion = "Adaptador para conectar laptop a monitor", Precio = 18.00m, Stock = 17 },
                new() { Nombre = "Hub USB 4 puertos", Descripcion = "Concentrador USB para perifericos", Precio = 15.00m, Stock = 21 },
                new() { Nombre = "Cargador universal para laptop", Descripcion = "Cargador compatible con varios modelos", Precio = 38.00m, Stock = 9 },
                new() { Nombre = "Soporte para laptop ajustable", Descripcion = "Base ergonomica para laptop", Precio = 24.00m, Stock = 19 }
            };

            var categoriasPorNombre = await context.Categorias
                .ToDictionaryAsync(c => c.Nombre, c => c.Id);

            foreach (var producto in productos)
            {
                var categoriaNombre = ObtenerCategoriaNombre(producto.Nombre, producto.Descripcion);

                if (categoriasPorNombre.TryGetValue(categoriaNombre, out var categoriaId))
                {
                    producto.CategoriaId = categoriaId;
                }
            }

            var nombresExistentes = await context.Productos
                .Select(p => p.Nombre)
                .ToListAsync();

            var productosFaltantes = productos
                .Where(p => !nombresExistentes.Contains(p.Nombre))
                .ToList();

            if (productosFaltantes.Count == 0)
            {
                return;
            }

            context.Productos.AddRange(productosFaltantes);
            await context.SaveChangesAsync();
        }

        private static string ObtenerCategoriaNombre(string nombre, string descripcion)
        {
            var texto = $"{nombre} {descripcion}".ToLowerInvariant();

            if (texto.Contains("laptop") || texto.Contains("macbook") || texto.Contains("tablet"))
            {
                return "Computadoras";
            }

            if (texto.Contains("monitor") || texto.Contains("proyector"))
            {
                return "Monitores";
            }

            if (texto.Contains("teclado") || texto.Contains("mouse") || texto.Contains("webcam")
                || texto.Contains("microfono") || texto.Contains("audifonos") || texto.Contains("parlantes")
                || texto.Contains("adaptador") || texto.Contains("hub") || texto.Contains("cargador")
                || texto.Contains("soporte"))
            {
                return "Perifericos";
            }

            if (texto.Contains("ssd") || texto.Contains("nvme") || texto.Contains("pendrive")
                || texto.Contains("externo") || texto.Contains("ram"))
            {
                return "Almacenamiento";
            }

            if (texto.Contains("router") || texto.Contains("switch") || texto.Contains("red")
                || texto.Contains("cat6"))
            {
                return "Redes";
            }

            if (texto.Contains("impresora"))
            {
                return "Impresion";
            }

            if (texto.Contains("licencia") || texto.Contains("antivirus") || texto.Contains("microsoft"))
            {
                return "Software";
            }

            return "Mobiliario";
        }

        private static async Task SeedCategoriasAsync(AppDbContext context)
        {
            if (await context.Categorias.AnyAsync())
            {
                await ActualizarDescuentosCategoriasAsync(context);
                return;
            }

            var categorias = new List<Categoria>
            {
                new() { Nombre = "Computadoras", Descripcion = "Equipos portatiles y de escritorio para laboratorio", DescuentoPorcentaje = DescuentosPorCategoria["Computadoras"], Activa = true },
                new() { Nombre = "Monitores", Descripcion = "Pantallas para oficina, aula y productividad", DescuentoPorcentaje = DescuentosPorCategoria["Monitores"], Activa = true },
                new() { Nombre = "Perifericos", Descripcion = "Teclados, mouse, webcam y accesorios USB", DescuentoPorcentaje = DescuentosPorCategoria["Perifericos"], Activa = true },
                new() { Nombre = "Almacenamiento", Descripcion = "Discos SSD, NVMe, externos y unidades USB", DescuentoPorcentaje = DescuentosPorCategoria["Almacenamiento"], Activa = true },
                new() { Nombre = "Redes", Descripcion = "Routers, switches y cableado de red", DescuentoPorcentaje = DescuentosPorCategoria["Redes"], Activa = true },
                new() { Nombre = "Impresion", Descripcion = "Impresoras laser, tinta y consumibles", DescuentoPorcentaje = DescuentosPorCategoria["Impresion"], Activa = true },
                new() { Nombre = "Software", Descripcion = "Licencias, suscripciones y seguridad informatica", DescuentoPorcentaje = DescuentosPorCategoria["Software"], Activa = true },
                new() { Nombre = "Mobiliario", Descripcion = "Sillas, escritorios y elementos para aula", DescuentoPorcentaje = DescuentosPorCategoria["Mobiliario"], Activa = false }
            };

            context.Categorias.AddRange(categorias);
            await context.SaveChangesAsync();
        }

        private static async Task ActualizarDescuentosCategoriasAsync(AppDbContext context)
        {
            var categorias = await context.Categorias
                .Where(c => c.DescuentoPorcentaje == 0)
                .ToListAsync();

            foreach (var categoria in categorias)
            {
                if (DescuentosPorCategoria.TryGetValue(categoria.Nombre, out var descuento))
                {
                    categoria.DescuentoPorcentaje = descuento;
                }
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
