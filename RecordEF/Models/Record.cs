using System;
using System.Collections.Generic;

namespace RecordEF.Models;

public partial class Record
{
    public int RecordId { get; set; }

    public int ArtistId { get; set; }

    public string Name { get; set; } = null!;

    public string Field { get; set; } = null!;

    public int Recorded { get; set; }

    public string Label { get; set; } = null!;

    public string Pressing { get; set; } = null!;

    public string Rating { get; set; } = null!;

    public int Discs { get; set; }

    public string Media { get; set; } = null!;

    public DateTime? Bought { get; set; }

    public decimal? Cost { get; set; }

    public string? CoverName { get; set; }

    public string? Review { get; set; }

    public int? FreeDbId { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual ICollection<Disc> DiscsNavigation { get; } = new List<Disc>();

    public override string ToString()
    {
        return $"Record Id: {RecordId}, Name: {Name}, Recorded: {Recorded}, Media: {Media}";
    }
}
