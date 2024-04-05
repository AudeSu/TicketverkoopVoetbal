using System;
using System.Collections.Generic;

namespace TicketverkoopVoetbal.Domains.Entities;

public partial class Stadion
{
    public int StadionId { get; set; }

    public string Naam { get; set; } = null!;

    public string Adres { get; set; } = null!;

    public string Stad { get; set; } = null!;

    public int Capaciteit { get; set; }

    public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual ICollection<Zitplaat> Zitplaats { get; set; } = new List<Zitplaat>();
}
