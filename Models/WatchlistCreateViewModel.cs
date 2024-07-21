using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Entities;


namespace MovieApp.Models
{
    public class WatchlistCreateViewModel
    {
        [Required]
        [StringLength(12, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 12 characters")]
        public string Name { get; set; }
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Description { get; set; }

        [Required]
        public List<Movie> Movies { get; set; }
    }
}