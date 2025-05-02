using System;
using System.Collections.Generic;

namespace webapi.Model;

public partial class BooksImport
{
    public string? Title { get; set; }

    public string? Author { get; set; }

    public string? Genre { get; set; }

    public int? Height { get; set; }

    public string? Publisher { get; set; }
}
