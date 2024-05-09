using System;
using System.Collections.Generic;

namespace TicketverkoopVoetbal.Domains.Entities;

public partial class Match
{
    public int MatchId { get; set; }

    public int StadionId { get; set; }

    public int ThuisploegId { get; set; }

    public int UitploegId { get; set; }

    public int SeizoenId { get; set; }

    public DateTime Datum { get; set; }

    public TimeSpan Startuur { get; set; }

    public virtual Seizoen Seizoen { get; set; } = null!;

    public virtual Stadion Stadion { get; set; } = null!;

    public virtual ICollection<Stoeltje> Stoeltjes { get; set; } = new List<Stoeltje>();

    public virtual Club Thuisploeg { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Club Uitploeg { get; set; } = null!;
}
