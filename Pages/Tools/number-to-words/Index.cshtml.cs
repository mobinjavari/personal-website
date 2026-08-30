using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Utils;

namespace MyWebApp.Pages.Tools.NumberToWords;

public class IndexModel : PageModel
{
    [BindProperty]
    public long? Number { get; set; }

    public string? Message { get; set; }
    public string? Words { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (Number is null)
        {
            Message = "لطفاً یک عدد صحیح وارد کنید";
            return Page();
        }

        Words = PersianNumberConverter.ToWords(Number.Value);
        Message = null;
        return Page();
    }
}
