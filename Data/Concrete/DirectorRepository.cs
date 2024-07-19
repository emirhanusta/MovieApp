using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Data.Abstract;
using MovieApp.Data.Concrete.Context;
using MovieApp.Entities;

namespace MovieApp.Data.Concrete
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly MovieDbContext _context;

        public DirectorRepository(MovieDbContext context)
        {
            _context = context;
        }

        public IQueryable<Director> Directors => _context.Directors;

    
    }
}