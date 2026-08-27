using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Models;
using MyWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace MyWebApp.Pages.Account.Database
{
    [Authorize]
    public class DownloadModel : AccountPageModel
    {
        public DownloadModel(
            ILogger<AccountPageModel> logger,
            ApplicationDbContext context,
            IConfiguration configuration) : base(logger, context, configuration)
        {
        }

        public override async Task<IActionResult> OnGetAsync()
        {
            await base.OnGetAsync();
            return await DownloadDatabaseAsync();
        }
    }
}