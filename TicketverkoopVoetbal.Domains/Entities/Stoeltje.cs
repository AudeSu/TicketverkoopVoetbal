using System;
using System.Collections.Generic;

namespace TicketverkoopVoetbal.Domains.Entities;

public partial class Stoeltje
{
    public int StoeltjeId { get; set; }

    public int StadionId { get; set; }

    public int ZoneId { get; set; }

    public int? MatchId { get; set; }

    public int ClubId { get; set; }

    public int SeizoenId { get; set; }

    public bool Bezet { get; set; }

    public virtual ICollection<Abonnement> Abonnements { get; set; } = new List<Abonnement>();

    public virtual Club Club { get; set; } = null!;

    public virtual Match? Match { get; set; }

    public virtual Seizoen Seizoen { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Zone Zone { get; set; } = null!;
}
