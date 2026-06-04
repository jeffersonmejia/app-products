using CrudProductos.Data;
using Microsoft.EntityFrameworkCore;


/*

AddControllersWithViews	Activa MVC con controladores y vistas.
AddDbContext<AppDbContext>	Registra el DbContext para inyección de dependencia.
UseNpgsql	Indica que la base de datos es PostgreSQL.
MapControllerRoute	Define la ruta inicial hacia Productos/Index.
*/
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

await DbInitializer.ApplyMigrationsAndSeedAsync(app);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Index}/{id?}");

app.Run();
