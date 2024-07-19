using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Entities
{
    public class Movie : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public DateTime ReleaseDate { get; set; }
        public long DirectorId { get; set; }
        public Director Director { get; set; } = null!;
        public List<Actor>? Actors { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<Watchlist>? Wachlists { get; set; }        
    }
}