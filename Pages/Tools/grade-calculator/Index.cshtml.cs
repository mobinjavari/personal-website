using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyWebApp.Pages.Tools.GradeCalculator
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public double Score { get; set; }
        public string Rank { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string ColorClass { get; set; } = string.Empty;

        public List<(string Color, string Text)> GradeScales { get; } = new()
        {
            ("bg-green-500", "90 تا 100: عالی"),
            ("bg-blue-500", "75 تا 89: خوب"),
            ("bg-yellow-500", "50 تا 74: متوسط"),
            ("bg-red-500", "کمتر از 50: ضعیف")
        };

        public void OnGet()
        {
        }

        public void OnPost()
        {
            CalculateRank();
        }

        private void CalculateRank()
        {
            if (Score < 0 || Score > 100)
            {
                Message = "نمره باید بین 0 تا 100 باشد";
                Rank = string.Empty;
                ColorClass = "border-red-500";
                return;
            }

            (Rank, ColorClass) = Score switch
            {
                >= 90 => ("عالی", "border-green-500"),
                >= 75 => ("خوب", "border-blue-500"),
                >= 50 => ("متوسط", "border-yellow-500"),
                _ => ("ضعیف", "border-red-500")
            };

            Message = string.Empty;
        }

        public string GetRankIcon(string rank) => rank switch
        {
            "عالی" => "fas fa-medal",
            "خوب" => "fas fa-thumbs-up",
            "متوسط" => "fas fa-arrow-right",
            _ => "fas fa-exclamation"
        };
    }
} 