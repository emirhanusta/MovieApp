using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MovieApp.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MovieApp.Models
{
    public class WatchlistEditViewModel
    {
        public long Id { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 12 characters")]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 100 characters")]
        public string Description { get; set; }

        public long[] MovieIds { get; set; }
    }
}