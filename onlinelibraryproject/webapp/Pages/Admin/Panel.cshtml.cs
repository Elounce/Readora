using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapp.Models;

namespace webapp.Pages.Admin
{
    public class PanelModel : PageModel
    {
        private readonly static Globals _global = new Globals();
        public bool ShowBooksTable { get; private set; }
        public List<Book> Books { get; private set; } = [];
        public List<User> Users { get; private set; } = [];


        public async Task OnGet(string table = "books")
        {
            ShowBooksTable = (table == "books");
            if (ShowBooksTable)
                Books = await GetBooksByPage();
            else
                Users = await GetUsersByPage();


        }

        private static async Task<List<Book>> GetBooksByPage(int page = 1, int limit = 25)
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

        private static async Task<List<User>> GetUsersByPage(int page = 1, int limit = 25)
        {
            List<User> users = [];
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync($"{_global.BaseApiUrl}/Users?page={page}&limit={limit}");
                Console.WriteLine($"STATUS: {response.StatusCode}");
                // по какой-то причине не десереализует данные
                /*books = JsonSerializer.Deserialize<List<Book>>(json);*/
                users = await response.Content.ReadFromJsonAsync<List<User>>() ?? [];
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return users;
            }
        }
    }
}
