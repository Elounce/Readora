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
        [Required(ErrorMessage = "���� �� ������ ���� ������")]
        [EmailAddress]
        [Display(Name = "�����")]
        public required string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "���� �� ������ ���� ������")]
        [Display(Name = "������")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "������ ������ ���� �� ������ 8 ��������")]
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
                        Console.WriteLine($"������ HTTP: {response.StatusCode} {response.Content}");
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
