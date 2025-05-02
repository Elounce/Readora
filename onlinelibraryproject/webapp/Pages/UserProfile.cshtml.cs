using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapp.Models;

namespace webapp.Pages
{
    public class UserProfileModel : PageModel
    {
        public new required User User { get; set; }
        public void OnGet(User user)
        {
            User = user;

        }
    }
}
