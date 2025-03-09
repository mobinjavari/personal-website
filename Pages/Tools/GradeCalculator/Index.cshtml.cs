using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Models;

namespace MyWebApp.Pages.Tools.GradeCalculator
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public Grade Grade { get; set; } = new();

        public void OnGet()
        {
        }

        public void OnPost()
        {
            Grade.CalculateRank();
        }
    }
} 