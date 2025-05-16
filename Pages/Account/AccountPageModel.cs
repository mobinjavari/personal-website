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

        if (file.Length > 10 * 1024 * 1024)
        {
            return BadRequest("حجم فایل نباید بیشتر از 10 مگابایت باشد");
        }

        if (!file.FileName.EndsWith(".db"))
        {
            return BadRequest("فقط فایل‌های با پسوند .db مجاز هستند");
        }

        try
        {
            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "app.db");
            _logger.LogInformation($"Starting database upload process. Target path: {dbPath}");

            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Create backup
            if (System.IO.File.Exists(dbPath))
            {
                string backupPath = $"{dbPath}.{DateTime.Now:yyyyMMddHHmmss}.backup";
                System.IO.File.Copy(dbPath, backupPath, true);
                _logger.LogInformation($"Created backup at: {backupPath}");
            }

            // Validate SQLite file
            var tempPath = Path.GetTempFileName();
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            try
            {
                using var connection = new SqliteConnection($"Data Source={tempPath}");
                await connection.OpenAsync();
                await connection.CloseAsync();
            }
            catch (Exception)
            {
                System.IO.File.Delete(tempPath);
                return BadRequest("فایل آپلود شده یک دیتابیس SQLite معتبر نیست");
            }

            // Replace database file
            if (System.IO.File.Exists(dbPath))
            {
                var fileInfo = new FileInfo(dbPath);
                fileInfo.IsReadOnly = false;
            }

            System.IO.File.Copy(tempPath, dbPath, true);
            System.IO.File.Delete(tempPath);

            // Clear DB context and connection pool
            await _context.DisposeAsync();
            SqliteConnection.ClearPool(new SqliteConnection($"Data Source={dbPath}"));

            _logger.LogInformation("Database file replaced successfully");
            return RedirectToPage("/Account/Index", new { message = "دیتابیس با موفقیت آپلود و جایگزین شد" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during database upload");
            return StatusCode(500, $"خطا در آپلود فایل: {ex.Message}");
        }
    }
}