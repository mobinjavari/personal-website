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
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public DownloadModel(
            IWebHostEnvironment environment, 
            IConfiguration configuration,
            ILogger<AccountPageModel> logger,
            ApplicationDbContext context) : base(logger, context)
        {
            _environment = environment;
            _configuration = configuration;
        }

        public override async Task<IActionResult> OnGetAsync()
        {
            await base.OnGetAsync();
            return await DownloadDatabaseAsync();
        }
    }
} 