using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Entities
{
    public class Watchlist : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public long UserId { get; set; }
        public List<Movie>? Movies { get; set; }
    }
}