using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapp;
using webapp.Models;

namespace webapp.Pages
{
    public class IndexModel : PageModel
    {
        /*private readonly ILogger<IndexModel> _logger;*/

        /*public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }*/

        public List<string> Genres { get; set; }
        public List<Book> Books { get; set; }
        public List<Book> RandomBooks { get; set; }
        public List<Book> BooksSearchResults { get; set; }
        public string SelectedGenre { get; private set; }
        public string SelectedSort { get; private set; }
        public string Message { get; private set; } = "Hello, world!";
        public string SearchQuery { get; set; }

        private readonly static Globals _global = new Globals();

        public async Task OnGetAsync()
        {
            if(Books is null)
                Books = await GetBooksByPage();

            Genres = ["1", "2"];

        }

        public async Task<IActionResult> OnPostSearchAsync(string searchQuery)
        {
            BooksSearchResults = await SearchBooksByTitle(searchQuery);
            return Page();
        }

        public async Task<List<Book>> SearchBooksByTitle(string searchTerm)
        {
            if (Books is null)
                Books = await GetBooksByPage();

            if (string.IsNullOrEmpty(searchTerm))
            {
                return new List<Book>();
            }

            var filtered = Books
                .Where(b => b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return filtered;
        }

        public static async Task<List<Book>> GetBooksByPage(int page = 1, int limit = 25)
        {
            List<Book> books = [];

            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync($"{_global.BaseApiUrl}/books?page={page}&limit={limit}");
                Console.WriteLine($"STATUS: {response.StatusCode}");
                // по какой-то причине не десереализует данные
                /*books = JsonSerializer.Deserialize<List<Book>>(json);*/
                books = await response.Content.ReadFromJsonAsync<List<Book>>() ?? [];
                return books;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return books;
            }
        }

       /* public async static Task<List<Book>> GetRandomBooks(int limit = 5)
        {
            return [];
            *//*return list.OrderBy(arg => arg.Title).Take(limit).ToList();*//*
        }*/


    }
}
