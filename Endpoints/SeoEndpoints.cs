using System.Xml.Linq;
using MyWebApp.Models;
using MyWebApp.Utils;

namespace MyWebApp.Endpoints;

public static class SeoEndpoints
{
    public static void MapSeoEndpoints(this WebApplication app)
    {
        app.MapGet("/robots.txt", (IWebHostEnvironment environment, IConfiguration configuration) =>
        {
            var siteUrl = GetSiteUrl(configuration);
            var body = environment.IsProduction()
                ? $"User-agent: *\nAllow: /\nDisallow: /Account\nDisallow: /Auth\n\nSitemap: {siteUrl}/sitemap.xml\n"
                : "User-agent: *\nDisallow: /\n";

            return Results.Text(body, "text/plain");
        });

        app.MapGet("/sitemap.xml", (IConfiguration configuration) =>
        {
            var siteUrl = GetSiteUrl(configuration);
            var pageUrls = GetIndexablePageUrls();
            XNamespace sitemapNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";

            var document = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement(sitemapNamespace + "urlset",
                    pageUrls.Select(url => new XElement(sitemapNamespace + "url",
                        new XElement(sitemapNamespace + "loc", $"{siteUrl}{url}")))));

            return Results.Text($"{document.Declaration}\n{document}", "application/xml");
        });

        app.MapGet("/llms.txt", (IConfiguration configuration) =>
        {
            var siteUrl = GetSiteUrl(configuration);
            var toolsUrl = StringUtils.ToUrl($"/{SiteConfig.Content.Tools.Id}");
            var toolLinks = SiteConfig.Content.Tools.Items.Select(tool =>
                $"- [{tool.Name}]({siteUrl}{StringUtils.ToUrl($"{toolsUrl}/{tool.Id}")}): {tool.Description}");

            var body = $"""
                # {SiteConfig.Site.Title}

                > {SiteConfig.Meta.DefaultDescription}

                - [صفحه اصلی]({siteUrl}/): معرفی، نمونه‌کارها و مهارت‌های {SiteConfig.Site.Author}
                - [{SiteConfig.Content.Tools.Title}]({siteUrl}{toolsUrl}): {SiteConfig.Content.Tools.Description}

                ## Tools

                {string.Join('\n', toolLinks)}
                """;

            return Results.Text(body, "text/plain");
        });
    }

    private static List<string> GetIndexablePageUrls()
    {
        var toolsUrl = StringUtils.ToUrl($"/{SiteConfig.Content.Tools.Id}");
        var urls = new List<string> { "/", toolsUrl };
        urls.AddRange(SiteConfig.Content.Tools.Items.Select(tool =>
            StringUtils.ToUrl($"{toolsUrl}/{tool.Id}")));

        return urls;
    }

    private static string GetSiteUrl(IConfiguration configuration) =>
        configuration["SITE_URL"]?.TrimEnd('/') ?? string.Empty;
}
