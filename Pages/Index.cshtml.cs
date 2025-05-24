using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Data;
using MyWebApp.Models;

namespace MyWebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _context;

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [BindProperty]
    public ContactMessage ContactForm { get; set; } = new();

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostContactAsync()
    {
        if (!ModelState.IsValid)
        {
            TempData["ContactMessage"] = "لطفا تمام فیلدها را به درستی پر کنید.";
            return Page();
        }

        try
        {
            ContactForm.CreatedAt = DateTime.UtcNow;
            _context.ContactMessages.Add(ContactForm);
            await _context.SaveChangesAsync();

            TempData["ContactSuccess"] = true;
            TempData["ContactMessage"] = "پیام شما با موفقیت ارسال شد.";
            
            // Clear the form
            ModelState.Clear();
            ContactForm = new ContactMessage();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving contact message");
            TempData["ContactMessage"] = "خطا در ارسال پیام. لطفا دوباره تلاش کنید.";
        }

        return RedirectToPage();
    }
}
