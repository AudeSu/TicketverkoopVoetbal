using System;
using System.Collections.Generic;

namespace TicketverkoopVoetbal.Domains.Entities;

public partial class Stoeltje
{
    public int StoeltjeId { get; set; }

    public int StadionId { get; set; }

    public int ZoneId { get; set; }

    public int? MatchId { get; set; }

    public int? ClubId { get; set; }

    public bool Bezet { get; set; }

    public virtual Club? Club { get; set; }

    public virtual Match? Match { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Zone Zone { get; set; } = null!;
}
