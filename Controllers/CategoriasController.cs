using CrudProductos.Data;
using CrudProductos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudProductos.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? busqueda)
        {
            var categorias = _context.Categorias.AsNoTracking();

            var todas = await categorias
                .OrderBy(c => c.Id)
                .ToListAsync();

            var activas = await categorias
                .Where(c => c.Activa)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            var filtradas = await categorias
                .Where(c => string.IsNullOrWhiteSpace(busqueda)
                    || EF.Functions.ILike(c.Nombre, $"%{busqueda}%")
                    || EF.Functions.ILike(c.Descripcion, $"%{busqueda}%"))
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            var recientes = await categorias
                .OrderByDescending(c => c.FechaCreacion)
                .Take(3)
                .ToListAsync();

            var viewModel = new ConsultasCategoriasViewModel
            {
                Busqueda = busqueda,
                Todas = todas,
                Activas = activas,
                Filtradas = filtradas,
                Recientes = recientes
            };

            return View(viewModel);
        }
    }
}
