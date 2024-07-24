namespace MovieApp.Entities
{
    public class User : BaseEntity
    {
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }
        public string? Biography { get; set; }
        public List<Review>? Reviews { get; set; }
        public List<Like>? Likes { get; set; }
        public virtual ICollection<Event>?OrganizedEvents { get; set; }
        public virtual ICollection<EventParticipant>? ParticipatedEvents { get; set; }

    }
}