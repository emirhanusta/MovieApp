namespace MovieApp.Entities
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateOnly CreatedDate { get; set; }
        public DateOnly? UpdatedDate { get; set; }
    }
}