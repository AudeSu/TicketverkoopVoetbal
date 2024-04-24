using System;
using System.Collections.Generic;

namespace TicketverkoopVoetbal.Domains.Entities;

public partial class Match
{
    public int MatchId { get; set; }

    public int StadionId { get; set; }

    public int ThuisploegId { get; set; }

    public int UitploegId { get; set; }

    public DateOnly Datum { get; set; }

    public TimeSpan Startuur { get; set; }

    public virtual Stadion Stadion { get; set; } = null!;

    public virtual Club Thuisploeg { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Club Uitploeg { get; set; } = null!;
}
