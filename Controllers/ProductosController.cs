using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrudProductos.Data;
using CrudProductos.Models;

namespace CrudProductos.Controllers
{
    public class ProductosController : Controller
    {

        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }


        //Mostrar los productos que tengan stock disponible, ordenar los 
        // resultados desde el precio más alto al más bajo, saltar los 
        // 2 primeros registros y mostrar únicamente los 5 siguientes productos.
        /*public async Task<IActionResult> Index()
        {
            return View(await _context.Productos
                .Where(p => p.Stock > 0)              // stock disponible
                .OrderByDescending(p => p.Precio)     // precio más alto a más bajo
                .Skip(2)                               // saltar los 2 primeros
                .Take(5)                               // tomar los siguientes 5
                .ToListAsync());
        }
        */
        /*public async Task<IActionResult> Index()
        {
            return View(await _context.Productos
                .Where(p => p.Precio >= 300 && p.Precio <= 1000)              // stock disponible
                .OrderBy(p => p.Stock)     // precio más alto a más bajo
                                           //.Skip(2)                               // saltar los 2 primeros
                                           //.Take(5)                               // tomar los siguientes 5
                .ToListAsync());
        }*/
        /*public async Task<IActionResult> Index()
        {
            String palabra = "RAM";
            return View(await _context.Productos
            .Where(p=> EF.Functions.ILike(p.Descripcion, $"%{palabra}$")) 
           //     .Where(p => p.Nombre.Contains("a", StringComparison.OrdinalIgnoreCase)) 
                .OrderBy(p => p.Nombre)     
                .ToListAsync());
        }*/
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 5;
            page = Math.Max(page, 1);

            var query = _context.Productos
                .AsNoTracking()
                .OrderBy(p => p.Id);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (totalPages > 0 && page > totalPages)
            {
                page = totalPages;
            }

            var productos = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedResult = new PagedResult<Producto>
            {
                Items = productos,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(pagedResult);
        }
        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,Stock,FechaRegistro")] Producto producto)
         {
             if (ModelState.IsValid)
             {
                 _context.Add(producto);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }
             return View(producto);
         }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,Stock")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                producto.FechaRegistro = DateTime.UtcNow;
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,Stock,FechaRegistro")] Producto producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    producto.FechaRegistro = DateTime.UtcNow;
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
