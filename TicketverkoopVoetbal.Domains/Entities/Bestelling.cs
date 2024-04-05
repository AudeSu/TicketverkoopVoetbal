using System;
using System.Collections.Generic;

namespace TicketverkoopVoetbal.Domains.Entities;

public partial class Bestelling
{
    public int BestellingId { get; set; }

    public string GebruikerId { get; set; } = null!;

    public int AantalTickets { get; set; }

    public DateTime Datum { get; set; }

    public virtual AspNetUser Gebruiker { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
