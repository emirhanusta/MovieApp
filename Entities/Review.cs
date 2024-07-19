namespace MovieApp.Entities
{
    public class Review : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public float Rating { get; set; }
        public long MovieId { get; set; }
        public long UserId { get; set; }
        public Movie Movie { get; set; } = null!;
        public User User { get; set; } = null!;
        public List<Like> Likes { get; set; } = new List<Like>();
    }
}