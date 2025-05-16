using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Models;
using System.Security.Claims;

namespace MyWebApp.Pages.Account;

public class IndexModel : AccountPageModel
{
    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        : base(logger, context)
    {
    }

    public string? Message { get; set; }
    public string? AlertClass { get; set; }
    
    [BindProperty]
    public IFormFile? DatabaseFile { get; set; }

    public override async Task<IActionResult> OnGetAsync()
    {
        var result = await base.OnGetAsync();
        if (result is not PageResult)
        {
            return result;
        }

        return Page();
    }
}
