using Microsoft.AspNetCore.Identity;

namespace MovieApp.Entities
{
    public class User : IdentityUser<long>
    {
        
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Biography { get; set; }
        public List<Review>? Reviews { get; set; }
        public List<Like>? Likes { get; set; }
        public virtual ICollection<Event>?OrganizedEvents { get; set; }
        public virtual ICollection<EventParticipant>? ParticipatedEvents { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}