using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Entities
{
public class Event : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; }
    public long OrganizerId { get; set; }
    public virtual User? Organizer { get; set; }
    public virtual ICollection<EventParticipant>? Participants { get; set; }
}
}