using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace webapi.Model;

public partial class MkarpovCourseworkLibraryContext : DbContext
{
    private readonly IConfiguration _configuration;

    public MkarpovCourseworkLibraryContext()
    {
    }

    public MkarpovCourseworkLibraryContext(DbContextOptions<MkarpovCourseworkLibraryContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BooksImport> BooksImports { get; set; }

    public virtual DbSet<BooksUser> BooksUsers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserReadingStatus> UserReadingStatuses { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration["ConnectionStrings:kolei"];
        optionsBuilder.UseMySQL(connectionString);
    } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Author");

            entity.HasIndex(e => e.Name, "author_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Book");

            entity.HasIndex(e => e.AuthorId, "Book_Author_FK");

            entity.HasIndex(e => e.GenreId, "Book_Genre_FK");

            entity.HasIndex(e => e.LanguageId, "book_language_FK");

            entity.HasIndex(e => e.PublisherId, "book_publisher_FK");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthorId).HasColumnName("authorId");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.GenreId).HasColumnName("genreId");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("imageUrl");
            entity.Property(e => e.Isbn).HasColumnName("isbn");
            entity.Property(e => e.LanguageId).HasColumnName("languageId");
            entity.Property(e => e.Pages).HasColumnName("pages");
            entity.Property(e => e.PublisherId).HasColumnName("publisherId");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.TitleLong)
                .HasMaxLength(255)
                .HasColumnName("titleLong");
            entity.Property(e => e.YearPublished)
                .HasColumnType("year")
                .HasColumnName("yearPublished");

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Book_Author_FK");

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Book_Genre_FK");

            entity.HasOne(d => d.Language).WithMany(p => p.Books)
                .HasForeignKey(d => d.LanguageId)
                .HasConstraintName("book_language_FK");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("book_publisher_FK");

            entity.HasMany(d => d.Categories).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("bookCategory_category_FK"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("bookCategory_book_FK"),
                    j =>
                    {
                        j.HasKey("BookId", "CategoryId").HasName("PRIMARY");
                        j.ToTable("BookCategory");
                        j.HasIndex(new[] { "CategoryId" }, "bookCategory_category_FK");
                        j.IndexerProperty<int>("BookId").HasColumnName("bookId");
                        j.IndexerProperty<int>("CategoryId").HasColumnName("categoryId");
                    });
        });

        modelBuilder.Entity<BooksImport>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("books_import");

            entity.Property(e => e.Author)
                .HasMaxLength(50)
                .HasColumnName("author");
            entity.Property(e => e.Genre)
                .HasMaxLength(50)
                .HasColumnName("genre");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.Publisher)
                .HasMaxLength(50)
                .HasColumnName("publisher");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<BooksUser>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.BookId }).HasName("PRIMARY");

            entity.ToTable("BooksUser");

            entity.HasIndex(e => e.StatusId, "BooksUser_UserReadingStatus_FK");

            entity.HasIndex(e => e.BookId, "BooksUser_book_FK");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.LastReadPage).HasColumnName("lastReadPage");
            entity.Property(e => e.StatusId).HasColumnName("statusId");

            entity.HasOne(d => d.Book).WithMany(p => p.BooksUsers)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BooksUser_book_FK");

            entity.HasOne(d => d.Status).WithMany(p => p.BooksUsers)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BooksUser_UserReadingStatus_FK");

            entity.HasOne(d => d.User).WithMany(p => p.BooksUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BooksUser_user_FK");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Category");

            entity.HasIndex(e => e.Name, "category_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Genre");

            entity.HasIndex(e => e.Name, "Genre_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Language");

            entity.HasIndex(e => e.NativeName, "Language_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Name, "name_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");
            entity.Property(e => e.NativeName)
                .HasMaxLength(64)
                .HasColumnName("nativeName");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Publisher");

            entity.HasIndex(e => e.Name, "publisher_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Review");

            entity.HasIndex(e => e.BookId, "Review_Book_FK");

            entity.HasIndex(e => e.UserId, "Review_User_FK");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.Comment)
                .HasColumnType("text")
                .HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Rating)
                .HasPrecision(2, 1)
                .HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Book).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Review_Book_FK");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Review_User_FK");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User");

            entity.HasIndex(e => e.RoleId, "User_UserRole_FK");

            entity.HasIndex(e => e.Email, "email_unique").IsUnique();

            entity.HasIndex(e => e.Login, "login_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdultContentRestriction).HasColumnName("adultContentRestriction");
            entity.Property(e => e.BirthDate)
                .HasColumnType("date")
                .HasColumnName("birthDate");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("firstName");
            entity.Property(e => e.HashPassword)
                .HasMaxLength(255)
                .HasColumnName("hashPassword");
            entity.Property(e => e.LastLoginDate)
                .HasColumnType("datetime")
                .HasColumnName("lastLoginDate");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("lastName");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("login");
            entity.Property(e => e.Nickname)
                .HasMaxLength(20)
                .HasColumnName("nickname");
            entity.Property(e => e.PublicAccount).HasColumnName("publicAccount");
            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("'1'")
                .HasColumnName("roleId");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("User_UserRole_FK");
        });

        modelBuilder.Entity<UserReadingStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("UserReadingStatus");

            entity.HasIndex(e => e.Name, "UserReadingStatus_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("UserRole");

            entity.HasIndex(e => e.Name, "UserRole_UNIQUE_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
