namespace MovieApp.Entities
{
    public class Director : BaseEntity
    {
        public string? Name { get; set; }
        public string? Biography { get; set; }
        public string? Image { get; set; }
        public List<Movie>? Movies { get; set; } = new List<Movie>();
    }
}