namespace MovieApp.Entities
{
    public class User : BaseEntity
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? image { get; set; }
        public string? biography { get; set; }
        public List<Review>? Reviews { get; set; }
        public List<Like>? Likes { get; set; }

    }
}