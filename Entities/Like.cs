namespace MovieApp.Entities
{
    public class Like : BaseEntity
    {
        public long UserId { get; set; }
        public long ReviewId { get; set; }
    }
}