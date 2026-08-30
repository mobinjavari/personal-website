using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyWebApp.Pages.Tools.LoanCalculator;

public class IndexModel : PageModel
{
    [BindProperty]
    public decimal? Amount { get; set; }

    [BindProperty]
    public decimal? AnnualRate { get; set; }

    [BindProperty]
    public int? Months { get; set; }

    public string? Message { get; set; }
    public string? MonthlyInstallment { get; set; }
    public string? TotalPayment { get; set; }
    public string? TotalInterest { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (Amount is null or <= 0 || Months is null or <= 0 || AnnualRate is null or < 0)
        {
            Message = "لطفاً مبلغ وام، نرخ سود و مدت بازپرداخت را به‌درستی وارد کنید";
            return Page();
        }

        var monthlyRate = AnnualRate.Value / 12m / 100m;
        var monthlyPayment = monthlyRate == 0
            ? Amount.Value / Months.Value
            : CalculateAmortizedPayment(Amount.Value, monthlyRate, Months.Value);

        var totalPayment = monthlyPayment * Months.Value;
        var totalInterest = totalPayment - Amount.Value;

        MonthlyInstallment = monthlyPayment.ToString("N0");
        TotalPayment = totalPayment.ToString("N0");
        TotalInterest = totalInterest.ToString("N0");
        Message = null;

        return Page();
    }

    private static decimal CalculateAmortizedPayment(decimal principal, decimal monthlyRate, int months)
    {
        var growthFactor = (decimal)Math.Pow((double)(1 + monthlyRate), months);
        return principal * monthlyRate * growthFactor / (growthFactor - 1);
    }
}
