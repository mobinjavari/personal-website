using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyWebApp.Pages.Tools.DateConverter;

public class IndexModel : PageModel
{
    private static readonly string[] PersianMonths =
    {
        "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
        "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
    };

    private static readonly string[] GregorianMonths =
    {
        "ژانویه", "فوریه", "مارس", "آوریل", "مه", "ژوئن",
        "ژوئیه", "اوت", "سپتامبر", "اکتبر", "نوامبر", "دسامبر"
    };

    [BindProperty]
    public string Direction { get; set; } = "ToGregorian";

    [BindProperty]
    public int? Year { get; set; }

    [BindProperty]
    public int? Month { get; set; }

    [BindProperty]
    public int? Day { get; set; }

    public string? Message { get; set; }
    public string? ResultDate { get; set; }
    public string? ResultWeekday { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (Year is null || Month is null || Day is null)
        {
            Message = "لطفاً سال، ماه و روز را وارد کنید";
            return Page();
        }

        var persianCalendar = new PersianCalendar();

        try
        {
            if (Direction == "ToGregorian")
            {
                var date = persianCalendar.ToDateTime(Year.Value, Month.Value, Day.Value, 0, 0, 0, 0);
                ResultDate = $"{date.Day} {GregorianMonths[date.Month - 1]} {date.Year}";
                ResultWeekday = date.ToString("dddd", new CultureInfo("fa-IR"));
            }
            else
            {
                var date = new DateTime(Year.Value, Month.Value, Day.Value);
                var persianYear = persianCalendar.GetYear(date);
                var persianMonth = persianCalendar.GetMonth(date);
                var persianDay = persianCalendar.GetDayOfMonth(date);
                ResultDate = $"{persianDay} {PersianMonths[persianMonth - 1]} {persianYear}";
                ResultWeekday = date.ToString("dddd", new CultureInfo("fa-IR"));
            }

            Message = null;
        }
        catch (ArgumentOutOfRangeException)
        {
            Message = "تاریخ واردشده معتبر نیست";
            ResultDate = null;
        }

        return Page();
    }
}
