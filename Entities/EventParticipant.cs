using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Entities
{
public class EventParticipant : BaseEntity
{
    public long EventId { get; set; }
    public virtual Event Event { get; set; }
    public long UserId { get; set; }
    public virtual User User { get; set; }
}
}