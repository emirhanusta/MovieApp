using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Data.Abstract;
using MovieApp.Data.Concrete.Context;
using MovieApp.Entities;

namespace MovieApp.Data.Concrete
{
    public class EventRepository : IEventRepository
    {

        private readonly MovieDbContext _context;

        public EventRepository(MovieDbContext context)
        {
            _context = context;
        }
        public IQueryable<Event> Events => _context.Events;

        public void AddEvent(Event entity)
        {
            _context.Events.Add(entity);
            _context.SaveChanges();
        }

        public void DeleteEvent(Event entity)
        {
            _context.Events.Remove(entity);
            _context.SaveChanges();
        }

        public void UpdateEvent(Event entity)
        {
            _context.Events.Update(entity);
            _context.SaveChanges();
        }
    }
}