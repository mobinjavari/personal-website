using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyWebApp.Pages.Auth;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public bool ShowPassword { get; set; }
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToPage("/Account/Index");
        }

        if (TempData.TryGetValue("Email", out var email))
        {
            Email = email?.ToString() ?? string.Empty;
            ShowPassword = true;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostCheckEmailAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(Email))
            {
                ErrorMessage = "لطفا ایمیل خود را وارد کنید";
                return Page();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email.ToLower());
            var newPassword = GenerateRandomPassword();
            var hashedPassword = HashPassword(newPassword);

            if (user != null)
            {
                user.Password = hashedPassword;
            }
            else
            {
                user = new User
                {
                    Email = Email.ToLower(),
                    Username = GenerateUsername(Email),
                    Password = hashedPassword,
                    Rank = UserRank.Ghost
                };
                _context.Users.Add(user);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Password generated for email: {Email} ~> {Password}", Email, newPassword);

            TempData["Email"] = Email;
            ShowPassword = true;
            SuccessMessage = "رمز عبور به ایمیل شما ارسال شد.";
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in OnPostCheckEmailAsync for email: {Email}", Email);
            ErrorMessage = "خطایی رخ داد. لطفاً دوباره تلاش کنید.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostLoginAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "لطفا ایمیل و رمز عبور خود را وارد کنید";
                ShowPassword = true;
                return Page();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email.ToLower());
            if (user == null || user.Password != HashPassword(Password))
            {
                ErrorMessage = "ایمیل یا رمز عبور اشتباه است";
                ShowPassword = true;
                return Page();
            }

            if (user.Rank == UserRank.Ghost)
            {
                user.Rank = UserRank.User;
            }

            // Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

            TempData.Remove("Email");
            return RedirectToPage("/Account/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in OnPostLoginAsync for email: {Email}", Email);
            ErrorMessage = "خطایی رخ داد. لطفاً دوباره تلاش کنید.";
            ShowPassword = true;
            return Page();
        }
    }

    private string GenerateUsername(string email)
    {
        var username = email.Split('@')[0].ToLower();
        username = new string(username.Where(c => char.IsLetter(c)).ToArray());
        
        var baseUsername = username;
        var counter = 1;
        while (_context.Users.AsNoTracking().Any(u => u.Username == username))
        {
            username = $"{baseUsername}{counter}";
            counter++;
        }

        return username;
    }

    private string GenerateRandomPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
        var random = new Random();
        var password = new char[12];
        
        for (int i = 0; i < password.Length; i++)
            password[i] = chars[random.Next(chars.Length)];
        
        return new string(password);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}