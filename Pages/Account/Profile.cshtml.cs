using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace MyWebApp.Pages.Account;

public class ProfileModel : AccountPageModel
{
    [TempData]
    public string? StatusMessage { get; set; }
    
    [TempData]
    public bool AlertType { get; set; } = false; // Changed from bool? to bool with default value

    public ProfileModel(ILogger<ProfileModel> logger, ApplicationDbContext context)
        : base(logger, context)
    {
        Input = new InputModel();
    }

    [BindProperty]
    public required InputModel Input { get; set; }

    public new User? UserData { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "نام کاربری الزامی است")]
        [RegularExpression(@"^[a-z]+$", ErrorMessage = "نام کاربری فقط می‌تواند شامل حروف کوچک انگلیسی باشد")]
        [StringLength(50, ErrorMessage = "نام کاربری نمی‌تواند بیشتر از 50 کاراکتر باشد")]
        [Display(Name = "نام کاربری")]
        public string Username { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "نام نمی‌تواند بیشتر از 50 کاراکتر باشد")]
        [Display(Name = "نام")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "نام خانوادگی نمی‌تواند بیشتر از 50 کاراکتر باشد")]
        [Display(Name = "نام خانوادگی")]
        public string? LastName { get; set; }
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var result = await base.OnGetAsync();
        if (result is not PageResult)
        {
            return result;
        }

        var userId = GetUserId();
        if (userId == null)
        {
            return NotFound();
        }

        UserData = await _context.Users.FindAsync(userId);

        if (UserData == null)
        {
            return NotFound();
        }

        Input = new InputModel
        {
            Username = UserData.Username,
            FirstName = UserData.FirstName,
            LastName = UserData.LastName
        };

        return Page();
    }

    public override async Task<IActionResult> OnPostAsync()
    {
        var result = await base.OnPostAsync();
        if (result is not PageResult)
        {
            return result;
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userId = GetUserId();
        if (userId == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        // Check if username is already taken by another user
        if (Input.Username != user.Username)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == Input.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError("Input.Username", "این نام کاربری قبلاً استفاده شده است");
                return Page();
            }
        }

        // Update user data
        user.Username = Input.Username;
        user.FirstName = Input.FirstName;
        user.LastName = Input.LastName;
        user.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
            StatusMessage = "اطلاعات با موفقیت بروزرسانی شد";
            AlertType = true;
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user profile");
            StatusMessage = "خطا در بروزرسانی اطلاعات";
            AlertType = false; 
            return Page();
        }
    }
}