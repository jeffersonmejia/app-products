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

        public async Task<IActionResult> Index()
        {
            var categorias = await _context.Categorias
                .AsNoTracking()
                .Include(c => c.Productos)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            return View(categorias);
        }

        public async Task<IActionResult> Consultas(string? busqueda)
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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .AsNoTracking()
                .Include(c => c.Productos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,DescuentoPorcentaje,Activa")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                categoria.FechaCreacion = DateTime.UtcNow;
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,DescuentoPorcentaje,Activa,FechaCreacion")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .AsNoTracking()
                .Include(c => c.Productos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(c => c.Id == id);
        }
    }
}
