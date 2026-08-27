using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Models;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MyWebApp.Pages.Auth;

public class IndexModel : PageModel
{
    private static readonly PasswordHasher<User> _passwordHasher = new();

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

    [BindProperty]
    public string? ReturnUrl { get; set; }

    public bool ShowPassword { get; set; }
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public IActionResult OnGet(string? returnUrl)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToPage("/Account/Index");
        }

        ReturnUrl = returnUrl;

        if (TempData.TryGetValue("Email", out var email))
        {
            Email = email?.ToString() ?? string.Empty;
            ShowPassword = true;
        }

        if (string.IsNullOrEmpty(ReturnUrl) && TempData.TryGetValue("ReturnUrl", out var returnUrlValue))
        {
            ReturnUrl = returnUrlValue?.ToString();
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

            if (user == null)
            {
                user = new User
                {
                    Email = Email.ToLower(),
                    Username = GenerateUsername(Email),
                    Password = string.Empty,
                    Rank = UserRank.Ghost
                };
                _context.Users.Add(user);
            }

            user.Password = _passwordHasher.HashPassword(user, newPassword);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Password generated for email: {Email} ~> {Password}", Email, newPassword);

            TempData["Email"] = Email;
            TempData["ReturnUrl"] = ReturnUrl;
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
            if (user == null || string.IsNullOrEmpty(user.Password) ||
                _passwordHasher.VerifyHashedPassword(user, user.Password, Password) == PasswordVerificationResult.Failed)
            {
                ErrorMessage = "ایمیل یا رمز عبور اشتباه است";
                ShowPassword = true;
                return Page();
            }

            if (user.Rank == UserRank.Ghost)
            {
                user.Rank = UserRank.User;
            }

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
            TempData.Remove("ReturnUrl");

            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return LocalRedirect(ReturnUrl);
            }

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
        var password = new char[12];

        for (int i = 0; i < password.Length; i++)
            password[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];

        return new string(password);
    }
}