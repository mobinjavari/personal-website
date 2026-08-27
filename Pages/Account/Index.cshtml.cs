using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Models;
using System.Collections.Generic;

namespace MyWebApp.Pages.Account;

public class IndexModel : AccountPageModel
{
    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context, IConfiguration configuration)
        : base(logger, context, configuration)
    {
        ContactMessages = new();
    }

    public List<ContactMessage> ContactMessages { get; set; }
    public string? Message { get; set; }
    public string? AlertClass { get; set; }

    [BindProperty]
    public IFormFile? DatabaseFile { get; set; }

    public override async Task<IActionResult> OnGetAsync()
    {
        var result = await base.OnGetAsync();
        if (result is not Microsoft.AspNetCore.Mvc.RazorPages.PageResult)
        {
            return result;
        }

        ContactMessages = await _context.ContactMessages
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostMessageDetailsAsync(int id)
    {
        var userId = GetUserId();
        var caller = userId.HasValue ? await _context.Users.FindAsync(userId.Value) : null;
        if (caller is not { Rank: UserRank.Owner or UserRank.Admin })
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        var message = await _context.ContactMessages.FindAsync(id);
        if (message == null)
        {
            return NotFound();
        }

        message.IsRead = true;
        await _context.SaveChangesAsync();

        return new JsonResult(new
        {
            message.Name,
            message.Email,
            message.Subject,
            message.Message,
            CreatedAt = message.CreatedAt.ToString("yyyy/MM/dd HH:mm")
        });
    }
}