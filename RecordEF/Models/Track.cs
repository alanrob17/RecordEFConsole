using System;
using System.Collections.Generic;

namespace RecordEF.Models;

public partial class Track
{
    public int TrackId { get; set; }

    public int DiscId { get; set; }

    public int? TrackNo { get; set; }

    public string? Name { get; set; }

    public int? TrackLength { get; set; }

    public string? Extended { get; set; }

    public virtual Disc Disc { get; set; } = null!;
}
