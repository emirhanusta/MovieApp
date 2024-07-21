using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Data.Abstract;
using MovieApp.Data.Concrete.Context;
using MovieApp.Entities;

namespace MovieApp.Data.Concrete
{
    public class ActorRepository : IActorRepository
    {
        private readonly MovieDbContext _context;

        public ActorRepository(MovieDbContext context)
        {
            _context = context;
        }

        public IQueryable<Actor> Actors => _context.Actors;

        public void AddActor(Actor actor)
        {
            _context.Actors.Add(actor);
            _context.SaveChanges();
        }

        public void UpdateActor(Actor actor)
        {
            _context.Update(actor);
            _context.SaveChanges();
        }

        public void DeleteActor(Actor actor)
        {
            _context.Actors.Remove(actor);
            _context.SaveChanges();
        }
    }
}