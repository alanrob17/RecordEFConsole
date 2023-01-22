using System;
using System.Collections.Generic;

namespace RecordEF.Models;

public partial class Review
{
    public int PitchforkId { get; set; }

    public int? ReviewId { get; set; }

    public int? ArtistId { get; set; }

    public int? RecordId { get; set; }

    public string? Name { get; set; }

    public string? RecordName { get; set; }

    public string? Author { get; set; }

    public DateTime? Published { get; set; }

    public string? Review1 { get; set; }
}
