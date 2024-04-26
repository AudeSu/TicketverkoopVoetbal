using System;
using System.Collections.Generic;

namespace TicketverkoopVoetbal.Domains.Entities;

public partial class Ticket
{
    public int TicketId { get; set; }

    public string GebruikersId { get; set; } = null!;

    public int BestellingId { get; set; }

    public int StoeltjeId { get; set; }

    public int MatchId { get; set; }

    public int ZoneId { get; set; }

    public virtual Bestelling Bestelling { get; set; } = null!;

    public virtual AspNetUser Gebruikers { get; set; } = null!;

    public virtual Match Match { get; set; } = null!;

    public virtual Stoeltje Stoeltje { get; set; } = null!;

    public virtual Zone Zone { get; set; } = null!;
}
