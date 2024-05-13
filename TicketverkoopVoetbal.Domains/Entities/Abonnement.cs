using System;
using System.Collections.Generic;

namespace TicketverkoopVoetbal.Domains.Entities;

public partial class Abonnement
{
    public int AbonnementId { get; set; }

    public string GebruikerId { get; set; } = null!;

    public int StoeltjeId { get; set; }

    public int ClubId { get; set; }

    public int SeizoenId { get; set; }

    public decimal? Prijs { get; set; }

    public virtual Club Club { get; set; } = null!;

    public virtual AspNetUser Gebruiker { get; set; } = null!;

    public virtual Seizoen Seizoen { get; set; } = null!;

    public virtual Stoeltje Stoeltje { get; set; } = null!;
}
