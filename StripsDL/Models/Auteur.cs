using System;
using System.Collections.Generic;

namespace StripsDL.Models;

public partial class Auteur
{
    public int Id { get; set; }

    public string Naam { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Strip> Strips { get; set; } = new List<Strip>();
}
