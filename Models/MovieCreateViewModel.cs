using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Entities;

namespace MovieApp.Models
{
    public class MovieCreateViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 1000 characters")]
        public string? Description { get; set; }
        public string? Image { get; set; }
        public List<Genre> Genres { get; set; } = new List<Genre>();
        [Required(ErrorMessage = "Release Date is required")]
        public DateTime ReleaseDate { get; set; }
        [Required(ErrorMessage = "Director is required")]
        public long DirectorId { get; set; }
        [Required(ErrorMessage = "Actors are required")]
        public List<Actor> Actors { get; set; } = new List<Actor>();
    }
}