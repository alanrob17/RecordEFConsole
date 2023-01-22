using System;
using System.Collections.Generic;

namespace RecordEF.Models;

public partial class FreeDb
{
    public int Id { get; set; }

    public string? Artist { get; set; }

    public int RecordId { get; set; }

    public string Record { get; set; } = null!;

    public int DiscId { get; set; }

    public string? FreeDbId { get; set; }

    public string? OtherFreeDbId { get; set; }

    public string? Genre { get; set; }

    public int? Revision { get; set; }

    public string? Review { get; set; }
}
