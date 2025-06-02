using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class Format
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<BookFormat> BookFormats { get; set; } = new List<BookFormat>();
}
