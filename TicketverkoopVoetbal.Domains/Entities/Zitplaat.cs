using System;
using System.Collections.Generic;

namespace TicketverkoopVoetbal.Domains.Entities;

public partial class Zitplaat
{
    public int ZitplaatsId { get; set; }

    public int StadionId { get; set; }

    public int ZoneId { get; set; }

    public bool Bezet { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Zone Zone { get; set; } = null!;
}
