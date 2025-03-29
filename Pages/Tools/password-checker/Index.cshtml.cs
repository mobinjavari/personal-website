using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;

namespace MyWebApp.Pages.Tools.PasswordChecker;

public class IndexModel : PageModel
{
    [BindProperty]
    public string? Password { get; set; }
    public string? Result { get; set; }
    public string? Message { get; set; }
    public string? ColorClass { get; set; }

    public List<(string Icon, string Text, bool IsMet)> Requirements { get; private set; } = new();

    public void OnGet()
    {
        InitializeRequirements();
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(Password))
        {
            Message = "لطفاً رمز عبور را وارد کنید";
            InitializeRequirements();
            return Page();
        }

        var (isStrong, requirements) = CheckPasswordStrength(Password);
        Requirements = requirements;

        if (isStrong)
        {
            Result = "رمز عبور قوی است";
            ColorClass = "border-green-500";
            Message = null;
        }
        else
        {
            Result = "رمز عبور قوی نیست";
            ColorClass = "border-red-500";
            // Message = "رمز عبور باید شامل موارد زیر باشد";
            Message = null;
        }

        return Page();
    }

    public IActionResult OnGetGeneratePassword()
    {
        var password = GenerateStrongPassword();
        return new JsonResult(new { password });
    }

    private string GenerateStrongPassword()
    {
        const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string special = "!@#$%^&*(),.?\":{}|<>";

        var password = new StringBuilder();
        
        // Add at least one character from each required set
        password.Append(upperCase[RandomNumberGenerator.GetInt32(upperCase.Length)]);
        password.Append(lowerCase[RandomNumberGenerator.GetInt32(lowerCase.Length)]);
        password.Append(digits[RandomNumberGenerator.GetInt32(digits.Length)]);
        password.Append(special[RandomNumberGenerator.GetInt32(special.Length)]);

        // Add random characters until we reach desired length (12)
        var allChars = upperCase + lowerCase + digits + special;
        while (password.Length < 12)
        {
            password.Append(allChars[RandomNumberGenerator.GetInt32(allChars.Length)]);
        }

        // Shuffle the password
        return new string(password.ToString().ToCharArray().OrderBy(x => RandomNumberGenerator.GetInt32(password.Length)).ToArray());
    }

    private void InitializeRequirements()
    {
        Requirements = new List<(string Icon, string Text, bool IsMet)>
        {
            ("fas fa-text-height", "حداقل ۸ کاراکتر", false),
            ("fas fa-hashtag", "حداقل یک عدد", false),
            ("fas fa-font", "حداقل یک حرف بزرگ انگلیسی", false),
            ("fas fa-star", "حداقل یک کاراکتر خاص", false)
        };
    }

    private (bool IsStrong, List<(string Icon, string Text, bool IsMet)> Requirements) CheckPasswordStrength(string password)
    {
        var requirements = new List<(string Icon, string Text, bool IsMet)>
        {
            ("fas fa-text-height", "حداقل ۸ کاراکتر", password.Length >= 8),
            ("fas fa-hashtag", "حداقل یک عدد", Regex.IsMatch(password, @"\d")),
            ("fas fa-font", "حداقل یک حرف بزرگ انگلیسی", Regex.IsMatch(password, @"[A-Z]")),
            ("fas fa-star", "حداقل یک کاراکتر خاص", Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]"))
        };

        return (requirements.All(r => r.IsMet), requirements);
    }
} 