using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Entities;

namespace MovieApp.Data.Abstract
{
    public interface IEventParticipantRepository
    {
        IQueryable<EventParticipant> EventParticipants { get; }
        void AddEventParticipant(EventParticipant entity);
        void UpdateEventParticipant(EventParticipant entity);
        void DeleteEventParticipant(EventParticipant entity);
    }
}