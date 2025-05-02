using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using webapp.Models;

namespace webapp.Pages
{
    public class RegistrationModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Пожалуйста введите имя пользователя")]
        [EmailAddress]
        [Display(Name = "Имя")]
        public required string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Пожалуйста введите пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Пароль должен содержать минимум 8 символов")]
        public required string Password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Пожалуйста введите пароль")]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль не совпадает.")]
        public required string ConfirmPassword { get; set; }

        private Globals global = new Globals();

        private readonly HttpClient _httpClient;
    

        public void OnGet()
        {

        }

        public RegistrationModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnPost(string Email, string Password, string ConfirmPassword) 
        {
            if (!ModelState.IsValid)
                return Page();
            if (string.IsNullOrEmpty(Email) || string.IsNullOrWhiteSpace(Password))
                return Page();
            if (Password != ConfirmPassword)
                return Page();

            // Проверка валидности email
            /*_ = new MailAddress(RegistrationInfo.Email).Address;*/

            User newUser = new User()
            {
                Email = Email,
                HashPassword = ConfirmPassword
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync<User>($"{global.BaseApiUrl}/users", newUser);

                if (!response.IsSuccessStatusCode)
                    {
                        // ModelState.AddModelError(key: "UserNotCreated", "Пользователь не создан");
                        TempData["ErrorMessage"] = "Пользователь не создан";    
                        return Page();
                    }
            }
            catch (HttpRequestException ex) 
            {
                ModelState.AddModelError(key: "NoConnection", "Нет подключения к серверу");
                Console.WriteLine(ex.Message);
                return Page();
            }
        


            return RedirectToPage("Login");
        }
    }
}
