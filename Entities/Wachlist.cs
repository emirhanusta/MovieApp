using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Entities
{
    public class Wachlist : BaseEntity
    {
        public long UserId { get; set; }
        public List<Movie>? Movies { get; set; }
    }
}