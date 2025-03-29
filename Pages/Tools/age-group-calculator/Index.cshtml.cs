using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyWebApp.Pages.Tools.AgeGroupCalculator;

public class IndexModel : PageModel
{
    [BindProperty]
    public int? Age { get; set; }
    public string? Result { get; set; }
    public string? Error { get; set; }
    public string? ColorClass { get; set; }

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

        (Result, ColorClass) = Age switch
        {
            <= 12 => ("کودک", "border-blue-500"),
            <= 19 => ("نوجوان", "border-purple-500"),
            <= 35 => ("جوان", "border-green-500"),
            <= 60 => ("میان‌سال", "border-yellow-500"),
            _ => ("کهن‌سال", "border-red-500")
        };

        Error = null;
        return Page();
    }
} 