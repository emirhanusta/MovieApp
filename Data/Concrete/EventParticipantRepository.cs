using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Data.Abstract;
using MovieApp.Data.Concrete.Context;
using MovieApp.Entities;

namespace MovieApp.Data.Concrete
{
    public class EventParticipantRepository : IEventParticipantRepository
    {

        private readonly MovieDbContext _context;

        public EventParticipantRepository(MovieDbContext context)
        {
            _context = context;
        }

        public IQueryable<EventParticipant> EventParticipants => _context.EventParticipants;

        public void AddEventParticipant(EventParticipant entity)
        {
            _context.EventParticipants.Add(entity);
            _context.SaveChanges();
        }

        public void DeleteEventParticipant(EventParticipant entity)
        {
            _context.EventParticipants.Remove(entity);
            _context.SaveChanges();
        }

        public void UpdateEventParticipant(EventParticipant entity)
        {
            _context.EventParticipants.Update(entity);
            _context.SaveChanges();
        }
    }

}