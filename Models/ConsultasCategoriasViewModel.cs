namespace CrudProductos.Models
{
    public class ConsultasCategoriasViewModel
    {
        public string? Busqueda { get; set; }

        public List<Categoria> Where { get; set; } = new();

        public List<Categoria> OrderBy { get; set; } = new();

        public List<Categoria> OrderByDescending { get; set; } = new();

        public List<Categoria> Take { get; set; } = new();

        public List<Categoria> Contains { get; set; } = new();

        public List<Categoria> ILike { get; set; } = new();

        public List<Categoria> FiltroCombinado { get; set; } = new();
    }
}
