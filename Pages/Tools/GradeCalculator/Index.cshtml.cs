using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Projects.GradeCalculator.Models;

namespace MyWebApp.Projects.GradeCalculator.Pages
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