using System;
using System.Collections.Generic;

namespace webapi.Model;

public partial class Book
{
    public int Id { get; set; }

    public int? Isbn { get; set; }

    public string Title { get; set; } = null!;

    public string? TitleLong { get; set; }

    public int? Pages { get; set; }

    public string? Description { get; set; }

    public int? YearPublished { get; set; }

    public string? ImageUrl { get; set; }

    public int GenreId { get; set; }

    public int? LanguageId { get; set; }

    public int PublisherId { get; set; }

    public int AuthorId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<BooksUser> BooksUsers { get; set; } = new List<BooksUser>();

    public virtual Genre Genre { get; set; } = null!;

    public virtual Language? Language { get; set; }

    public virtual Publisher Publisher { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
