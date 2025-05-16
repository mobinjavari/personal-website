using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.Sqlite;
using System.Security.Claims;
using MyWebApp.Data;
using MyWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MyWebApp.Pages.Account;

[Authorize]
public abstract class AccountPageModel : PageModel
{
    protected readonly ILogger<AccountPageModel> _logger;
    protected readonly ApplicationDbContext _context;
    public User? UserData { get; protected set; }

    public AccountPageModel(ILogger<AccountPageModel> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        var userId = GetUserId();
        if (!userId.HasValue)
        {
            return NotFound();
        }

        UserData = await _context.Users.FindAsync(userId.Value);

        if (UserData == null)
        {
            return NotFound();
        }

        return Page();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return RedirectToPage("/Auth/Index", new { returnUrl = Request.Path + Request.QueryString });
        }

        var userId = GetUserId();
        if (userId.HasValue)
        {
            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null)
            {
                return NotFound();
            }
        }

        return Page();
    }

    protected int? GetUserId()
    {
        var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return !string.IsNullOrEmpty(idClaim) && int.TryParse(idClaim, out var id) ? id : null;
    }

    protected string? GetUserEmail()
    {
        return User.FindFirstValue(ClaimTypes.Email);
    }

    protected async Task<IActionResult> DownloadDatabaseAsync()
    {
        if (UserData?.Rank != UserRank.Owner)
        {
            return RedirectToPage("/Account/Index");
        }

        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "app.db");
        if (!System.IO.File.Exists(dbPath))
        {
            return NotFound();
        }

        var memory = new MemoryStream();
        using (var stream = new FileStream(dbPath, FileMode.Open, FileAccess.Read))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;

        return File(memory, "application/octet-stream", "app.db");
    }

    protected async Task<IActionResult> UploadDatabaseAsync(IFormFile file)
    {
        if (UserData?.Rank != UserRank.Owner)
        {
            return RedirectToPage("/Account/Index");
        }

        if (file == null || file.Length == 0)
        {
            return BadRequest("لطفا یک فایل انتخاب کنید");
        }

        try
        {
            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "app.db");
            _logger.LogInformation($"Starting database upload process. Target path: {dbPath}");

            await _context.Database.CloseConnectionAsync();
            await _context.DisposeAsync();
            SqliteConnection.ClearPool(new SqliteConnection($"Data Source={dbPath}"));
            
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (System.IO.File.Exists(dbPath))
            {
                string backupPath = $"{dbPath}.{DateTime.Now:yyyyMMddHHmmss}.backup";
                try 
                {
                    System.IO.File.Copy(dbPath, backupPath, true);
                    System.IO.File.Delete(dbPath);
                }
                catch (IOException)
                {
                    await Task.Delay(1000);
                    System.IO.File.Copy(dbPath, backupPath, true);
                    System.IO.File.Delete(dbPath);
                }
            }

            using (var fileStream = new FileStream(dbPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            try
            {
                using var testConnection = new SqliteConnection($"Data Source={dbPath}");
                await testConnection.OpenAsync();
                await testConnection.CloseAsync();
            }
            catch
            {
                return BadRequest("فایل آپلود شده یک دیتابیس SQLite معتبر نیست");
            }

            return RedirectToPage("/Account/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during database upload");
            return StatusCode(500, $"خطا در آپلود فایل: {ex.Message}");
        }
    }
}