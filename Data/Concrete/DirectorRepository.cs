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

        public void AddDirector(Director director)
        {
            _context.Directors.Add(director);
            _context.SaveChanges();
        }

        public void UpdateDirector(Director director)
        {
            _context.Update(director);
            _context.SaveChanges();
        }

        public void DeleteDirector(Director director)
        {
            _context.Directors.Remove(director);
            _context.SaveChanges();
        }
    }
}