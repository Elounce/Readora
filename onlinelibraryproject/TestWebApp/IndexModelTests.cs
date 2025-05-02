using webapp.Models;
using webapp.Pages;

namespace TestWebApp
{
    public class IndexModelTests
    {
        [Fact]
        public void SillyTest()
        {
            // Arrange
            var model = new IndexModel();

            // Act
            model.OnGetAsync();

            // Assert
            Assert.NotNull(model.Message);
            Assert.Equal("Hello, world!", model.Message);
        }

        [Fact]
        public async Task OnGetAsync_LoadsBooks_WhenBooksAreNull()
        {
            // Arrange
            var model = new IndexModel();
            model.Books = null;

            // Act
            await model.OnGetAsync();

            // Assert
            Assert.NotNull(model.Books);
        }

        [Fact]
        public async Task SearchBooksByTitle_ReturnsFilteredResults()
        {
            // Arrange
            var model = new IndexModel();
            model.Books = new List<Book>
            {
                new Book { Id = 1, Title = "C# Programming" },
                new Book { Id = 2, Title = "ASP.NET Core Guide" },
                new Book { Id = 3, Title = "Python Basics" }
            };

            // Act
            var results = await model.SearchBooksByTitle("C#");

            // Assert
            Assert.Single(results);
            Assert.Equal("C# Programming", results[0].Title);
        }

        [Fact]
        public async Task SearchBooksByTitle_ReturnsEmptyList_WhenQueryIsEmpty()
        {
            // Arrange
            var model = new IndexModel();
            model.Books = new List<Book>
            {
                new Book { Id = 1, Title = "C# Programming" },
                new Book { Id = 2, Title = "ASP.NET Core Guide" }
            };

            // Act
            var results = await model.SearchBooksByTitle("");

            // Assert
            Assert.Empty(results);
        }
    }
}