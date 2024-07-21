using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Entities;

namespace MovieApp.Data.Abstract
{
    public interface IDirectorRepository
    {
        IQueryable<Director> Directors { get; }
        void AddDirector(Director director);
        void UpdateDirector(Director director);
        void DeleteDirector(Director director);
    }
}