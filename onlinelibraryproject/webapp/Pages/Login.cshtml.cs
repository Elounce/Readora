using webapp.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using webapp.Models;
using System.ComponentModel.DataAnnotations;

namespace webapp.Pages
{
    public class LoginModel : PageModel
    {
        public new User? User { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [EmailAddress]
        [Display(Name = "Почта")]
        public required string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Пароль должен быть не меньше 8 символов")]
        public required string Password { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAuthorization(string Email, string Password)
        {
            Console.WriteLine("OnGetAuthorization method");
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync($"http://localhost:5296/users/user?email={Email}&password={Password}");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(json);

                        User = await response.Content.ReadFromJsonAsync<User>();
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка HTTP: {response.StatusCode} {response.Content}");
                    }
                }
            }
            catch(Exception ex)
            { 
                Console.WriteLine($"Error: {ex.Message}");
            }

            if(User.RoleId == 1)
                return RedirectToPage("UserProfile", User);
            else
                return RedirectToPage("Panel");

        }
    }
}
