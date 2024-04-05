using System;
using System.Collections.Generic;

namespace TicketverkoopVoetbal.Domains.Entities;

public partial class Zone
{
    public int ZoneId { get; set; }

    public string Naam { get; set; } = null!;

    public decimal Prijs { get; set; }

    public int Aantal { get; set; }

    public virtual ICollection<Zitplaat> Zitplaats { get; set; } = new List<Zitplaat>();
}
