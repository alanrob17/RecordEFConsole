using System;
using System.Collections.Generic;

namespace RecordEF.Models;

public partial class Artist
{
    public int ArtistId { get; set; }

    public string? FirstName { get; set; }

    public string LastName { get; set; } = null!;

    public string? Name { get; set; }

    public string? Biography { get; set; }

    public virtual ICollection<Record> Records { get; } = new List<Record>();

    public override string ToString()
    {
        return $"Artist Id: {ArtistId}, Artist: {Name}, Biography: {Biography.Substring(0, 30)}...";
    }

}
