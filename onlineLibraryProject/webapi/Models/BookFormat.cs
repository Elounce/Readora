using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class BookFormat
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public int FormatId { get; set; }

    public string FileUrl { get; set; } = null!;

    public int? FileSize { get; set; }

    public int? Duration { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Format Format { get; set; } = null!;
}
