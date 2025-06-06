﻿using System;
using System.Collections.Generic;

namespace TicketverkoopVoetbal.Domains.Entities;

public partial class Club
{
    public int ClubId { get; set; }

    public string Naam { get; set; } = null!;

    public int ThuisstadionId { get; set; }

    public string LogoPath { get; set; } = null!;

    public virtual ICollection<Abonnement> Abonnements { get; set; } = new List<Abonnement>();

    public virtual ICollection<Match> MatchThuisploegs { get; set; } = new List<Match>();

    public virtual ICollection<Match> MatchUitploegs { get; set; } = new List<Match>();

    public virtual ICollection<Stoeltje> Stoeltjes { get; set; } = new List<Stoeltje>();

    public virtual Stadion Thuisstadion { get; set; } = null!;
}
