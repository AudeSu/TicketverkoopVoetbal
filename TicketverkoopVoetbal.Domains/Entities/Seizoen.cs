using System;
using System.Collections.Generic;

namespace TicketverkoopVoetbal.Domains.Entities;

public partial class Seizoen
{
    public int SeizoenId { get; set; }

    public DateTime Startdatum { get; set; }

    public DateTime Einddatum { get; set; }

    public virtual ICollection<Abonnement> Abonnements { get; set; } = new List<Abonnement>();

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}
