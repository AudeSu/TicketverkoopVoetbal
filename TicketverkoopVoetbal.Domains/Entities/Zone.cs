using System;
using System.Collections.Generic;

namespace TicketverkoopVoetbal.Domains.Entities;

public partial class Zone
{
    public int ZoneId { get; set; }

    public string Naam { get; set; } = null!;

    public decimal PrijsTicket { get; set; }

    public decimal? PrijsAbonnement { get; set; }

    public int Capaciteit { get; set; }

    public int StadionId { get; set; }

    public virtual Stadion Stadion { get; set; } = null!;

    public virtual ICollection<Stoeltje> Stoeltjes { get; set; } = new List<Stoeltje>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
