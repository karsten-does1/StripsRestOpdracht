using System;
using System.Collections.Generic;

namespace StripsDL.Models;

public partial class Reeksen
{
    public int Id { get; set; }

    public string Naam { get; set; } = null!;

    public virtual ICollection<Strip> Strips { get; set; } = new List<Strip>();
}
