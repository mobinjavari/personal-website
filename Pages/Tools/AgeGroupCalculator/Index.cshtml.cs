using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyWebApp.Pages.Tools.AgeGroupCalculator;

public class IndexModel : PageModel
{
    [BindProperty]
    public int? Age { get; set; }
    public string? Result { get; set; }
    public string? Error { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!Age.HasValue)
        {
            Error = "لطفاً سن را وارد کنید";
            return Page();
        }

        if (Age < 0)
        {
            Error = "سن نمی‌تواند منفی باشد";
            return Page();
        }

        Result = Age switch
        {
            <= 12 => "کودک",
            <= 19 => "نوجوان",
            <= 35 => "جوان",
            <= 60 => "میان‌سال",
            _ => "کهن‌سال"
        };

        return Page();
    }
} 