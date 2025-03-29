using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyWebApp.Pages.Tools.DiscountCalculator;

public class IndexModel : PageModel
{
    [BindProperty]
    public decimal Price { get; set; }

    public string? Message { get; set; }
    public string? FinalPrice { get; set; }
    public string? DiscountPercent { get; set; }
    public string? DiscountAmount { get; set; }
    public string ColorClass { get; set; } = "border-gray-500";

    private readonly Dictionary<decimal, decimal> _discountRanges = new()
    {
        { 100_000m, 0m },      // کمتر از 100 هزار: بدون تخفیف
        { 500_000m, 10m },     // 100 تا 500 هزار: 10 درصد
        { 1_000_000m, 15m },   // 500 هزار تا 1 میلیون: 15 درصد
        { decimal.MaxValue, 20m }  // بیش از 1 میلیون: 20 درصد
    };

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (Price <= 0)
        {
            Message = "لطفاً قیمت معتبر وارد کنید";
            return Page();
        }

        var discount = GetDiscountPercent(Price);
        var discountAmount = Price * (discount / 100m);
        var finalPrice = Price - discountAmount;

        FinalPrice = finalPrice.ToString("N0");
        DiscountPercent = discount.ToString("N0");
        DiscountAmount = discountAmount.ToString("N0");
        ColorClass = GetColorClass(discount);

        return Page();
    }

    private decimal GetDiscountPercent(decimal price)
    {
        foreach (var range in _discountRanges.OrderBy(x => x.Key))
        {
            if (price <= range.Key)
                return range.Value;
        }
        return _discountRanges[decimal.MaxValue];
    }

    private string GetColorClass(decimal discount) => discount switch
    {
        0 => "border-gray-500",
        10 => "border-blue-500",
        15 => "border-purple-500",
        20 => "border-green-500",
        _ => "border-gray-500"
    };
} 