using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.Sqlite;
using MyWebApp.Data;
using MyWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MyWebApp.Pages.Account;

[Authorize]
public abstract class AccountPageModel : PageModel
{
    protected readonly ILogger<AccountPageModel> _logger;
    protected readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    public User? UserData { get; protected set; }

    public AccountPageModel(ILogger<AccountPageModel> logger, ApplicationDbContext context, IConfiguration configuration)
    {
        _logger = logger;
        _context = context;
        _configuration = configuration;
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

    protected string GetDatabasePath()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        var dataSource = new SqliteConnectionStringBuilder(connectionString).DataSource;
        return Path.IsPathRooted(dataSource) ? dataSource : Path.Combine(Directory.GetCurrentDirectory(), dataSource);
    }

    protected async Task<IActionResult> DownloadDatabaseAsync()
    {
        if (UserData?.Rank != UserRank.Owner)
        {
            return RedirectToPage("/Account/Index");
        }

        var dbPath = GetDatabasePath();
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
            var dbPath = GetDatabasePath();
            _logger.LogInformation("Starting database upload process. Target path: {DbPath}", dbPath);

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