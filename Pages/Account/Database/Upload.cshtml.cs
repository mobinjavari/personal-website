using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Data;

namespace MyWebApp.Pages.Account.Database
{
    [Authorize]
    public class UploadModel : AccountPageModel
    {
        [BindProperty]
        public IFormFile? DatabaseFile { get; set; }
        public string? Message { get; set; }
        public string? AlertClass { get; set; }

        public UploadModel(
            ILogger<AccountPageModel> logger,
            ApplicationDbContext context) : base(logger, context)
        {
        }

        public override async Task<IActionResult> OnGetAsync()
        {
            return await base.OnGetAsync();
        }

        public override async Task<IActionResult> OnPostAsync()
        {
            await base.OnGetAsync();
            
            if (DatabaseFile == null)
            {
                Message = "لطفا یک فایل انتخاب کنید";
                AlertClass = "alert-danger";
                return Page();
            }
            
            var result = await UploadDatabaseAsync(DatabaseFile);
            
            if (result is RedirectToPageResult)
            {
                Message = "دیتابیس با موفقیت آپلود و جایگزین شد";
                AlertClass = "alert-success";
            }
            else if (result is BadRequestObjectResult badRequest)
            {
                Message = badRequest.Value?.ToString();
                AlertClass = "alert-danger";
            }
            else if (result is StatusCodeResult)
            {
                Message = "خطا در آپلود فایل";
                AlertClass = "alert-danger";
            }

            return Page();
        }
    }
}
