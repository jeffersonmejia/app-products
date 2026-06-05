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

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 5;
            page = Math.Max(page, 1);

            var query = _context.Categorias
                .AsNoTracking()
                .Include(c => c.Productos)
                .OrderByDescending(c => c.FechaCreacion)
                .ThenByDescending(c => c.Id);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (totalPages > 0 && page > totalPages)
            {
                page = totalPages;
            }

            var categorias = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedResult = new PagedResult<Categoria>
            {
                Items = categorias,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(pagedResult);
        }

        public async Task<IActionResult> Consultas(string? busqueda)
        {
            busqueda = busqueda?.Trim();
            if (busqueda?.Length > 80)
            {
                busqueda = busqueda[..80];
            }

            var categorias = _context.Categorias.AsNoTracking();

            var resultadoWhere = await categorias
                .Where(c => c.Activa)
                .ToListAsync();

            var resultadoOrderBy = await categorias
                .OrderBy(c => c.Id)
                .ToListAsync();

            var resultadoOrderByDescending = await categorias
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();

            var resultadoTake = await categorias
                .Take(3)
                .ToListAsync();

            var textoBusqueda = string.IsNullOrWhiteSpace(busqueda) ? "a" : busqueda;

            var resultadoContains = await categorias
                .Where(c => c.Nombre.Contains(textoBusqueda) || c.Descripcion.Contains(textoBusqueda))
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            var resultadoILike = await categorias
                .Where(c => EF.Functions.ILike(c.Nombre, $"%{textoBusqueda}%")
                    || EF.Functions.ILike(c.Descripcion, $"%{textoBusqueda}%"))
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            var filtroCombinado = await categorias
                .Where(c => c.Activa
                    && c.DescuentoPorcentaje > 5
                    && (EF.Functions.ILike(c.Nombre, $"%{textoBusqueda}%")
                        || EF.Functions.ILike(c.Descripcion, $"%{textoBusqueda}%")))
                .OrderByDescending(c => c.DescuentoPorcentaje)
                .ToListAsync();

            var viewModel = new ConsultasCategoriasViewModel
            {
                Busqueda = busqueda,
                Where = resultadoWhere,
                OrderBy = resultadoOrderBy,
                OrderByDescending = resultadoOrderByDescending,
                Take = resultadoTake,
                Contains = resultadoContains,
                ILike = resultadoILike,
                FiltroCombinado = filtroCombinado
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
            categoria.Nombre = categoria.Nombre.Trim();
            categoria.Descripcion = categoria.Descripcion.Trim();
            await ValidarCategoriaAsync(categoria);

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,DescuentoPorcentaje,Activa")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            categoria.Nombre = categoria.Nombre.Trim();
            categoria.Descripcion = categoria.Descripcion.Trim();
            await ValidarCategoriaAsync(categoria);

            if (ModelState.IsValid)
            {
                try
                {
                    var categoriaExistente = await _context.Categorias.FindAsync(id);

                    if (categoriaExistente == null)
                    {
                        return NotFound();
                    }

                    categoriaExistente.Nombre = categoria.Nombre;
                    categoriaExistente.Descripcion = categoria.Descripcion;
                    categoriaExistente.DescuentoPorcentaje = categoria.DescuentoPorcentaje;
                    categoriaExistente.Activa = categoria.Activa;

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

        private async Task ValidarCategoriaAsync(Categoria categoria)
        {
            if (await _context.Categorias.AnyAsync(c => c.Id != categoria.Id && c.Nombre == categoria.Nombre))
            {
                ModelState.AddModelError(nameof(Categoria.Nombre), "Ya existe una categoria con exactamente el mismo nombre.");
            }
        }
    }
}
