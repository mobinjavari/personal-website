using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyWebApp.Pages.Tools.BmiCalculator;

public class IndexModel : PageModel
{
    [BindProperty]
    public decimal? Weight { get; set; }

    [BindProperty]
    public decimal? Height { get; set; }

    public string? Message { get; set; }
    public string? BmiValue { get; set; }
    public string? Category { get; set; }
    public string? ColorClass { get; set; }

    public List<(string Color, string Text)> BmiScales { get; } = new()
    {
        ("bg-blue-500", "کمتر از ۱۸.۵: کم‌وزن"),
        ("bg-green-500", "۱۸.۵ تا ۲۴.۹: طبیعی"),
        ("bg-yellow-500", "۲۵ تا ۲۹.۹: اضافه‌وزن"),
        ("bg-orange-500", "۳۰ تا ۳۹.۹: چاق"),
        ("bg-red-500", "۴۰ به بالا: چاقی شدید")
    };

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (Weight is null or <= 0 || Height is null or <= 0)
        {
            Message = "لطفاً قد و وزن معتبر وارد کنید";
            return Page();
        }

        var heightInMeters = Height.Value / 100m;
        var bmi = Weight.Value / (heightInMeters * heightInMeters);

        (Category, ColorClass) = bmi switch
        {
            < 18.5m => ("کم‌وزن", "border-blue-500"),
            < 25m => ("طبیعی", "border-green-500"),
            < 30m => ("اضافه‌وزن", "border-yellow-500"),
            < 40m => ("چاق", "border-orange-500"),
            _ => ("چاقی شدید", "border-red-500")
        };

        BmiValue = bmi.ToString("N1");
        Message = null;
        return Page();
    }
}
