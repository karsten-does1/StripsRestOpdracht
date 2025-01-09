using System;
using System.Collections.Generic;

namespace StripsDL.Models;

public partial class Strip
{
    public int Id { get; set; }

    public string Titel { get; set; } = null!;

    public int UitgeverijId { get; set; }

    public int? ReeksId { get; set; }

    public int? ReeksNummer { get; set; }

    public virtual Reeksen? Reeks { get; set; }

    public virtual Uitgeverijen Uitgeverij { get; set; } = null!;

    public virtual ICollection<Auteur> Auteurs { get; set; } = new List<Auteur>();
}
