using System;
using System.Collections.Generic;

namespace RecordEF.Models;

public partial class Disc
{
    public int DiscId { get; set; }

    public int RecordId { get; set; }

    public int DiscNo { get; set; }

    public int? FreeDbDiscId { get; set; }

    public string? FreeDbId { get; set; }

    public int? Length { get; set; }

    public virtual Record Record { get; set; } = null!;

    public virtual ICollection<Track> Tracks { get; } = new List<Track>();
}
