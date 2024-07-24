using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data.Abstract;
using MovieApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.ViewComponents
{
    public class EventCalendar : ViewComponent
    {
        private readonly IEventRepository _eventRepository;

        public EventCalendar(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var events = await _eventRepository.Events.ToListAsync();
            return View(events);
        }        
    }
}
