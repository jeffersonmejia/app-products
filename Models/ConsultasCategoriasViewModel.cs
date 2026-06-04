namespace CrudProductos.Models
{
    public class ConsultasCategoriasViewModel
    {
        public string? Busqueda { get; set; }

        public List<Categoria> Todas { get; set; } = new();

        public List<Categoria> Activas { get; set; } = new();

        public List<Categoria> Filtradas { get; set; } = new();

        public List<Categoria> Recientes { get; set; } = new();
    }
}
