using System;
using System.Collections.Generic;

namespace TicketverkoopVoetbal.Domains.Entities;

public partial class Ticket
{
    public int TicketId { get; set; }

    public string GebruikersId { get; set; } = null!;

    public int StoeltjeId { get; set; }

    public int MatchId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual AspNetUser Gebruikers { get; set; } = null!;

    public virtual Match Match { get; set; } = null!;

    public virtual Stoeltje Stoeltje { get; set; } = null!;
}
