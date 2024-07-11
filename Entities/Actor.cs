using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Entities
{
    public class Actor : BaseEntity
    {
        public string? Name { get; set; }
        public string? Biography { get; set; }
        public string? Image { get; set; }
        public List<Movie>? Movies { get; set; }        
    }
}