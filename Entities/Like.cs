namespace MovieApp.Entities
{
    public class Like : BaseEntity
    {
        public long UserId { get; set; }
        public long ReviewId { get; set; }
        public User User { get; set; } = null!;
        public Review Review { get; set; } = null!;
    }
}