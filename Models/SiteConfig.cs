namespace MyWebApp.Models;

public static class SiteConfig
{
    private static string _twitterUsername = string.Empty;
    private static string _telegramUsername = string.Empty;
    private static string _gitHubUsername = string.Empty;
    private static string _contactEmail = string.Empty;

    public static void Configure(IConfiguration configuration)
    {
        _twitterUsername = configuration["TWITTER_USERNAME"] ?? string.Empty;
        _telegramUsername = configuration["TELEGRAM_USERNAME"] ?? string.Empty;
        _gitHubUsername = configuration["GITHUB_USERNAME"] ?? string.Empty;
        _contactEmail = configuration["CONTACT_EMAIL"] ?? string.Empty;
    }

    public static class Site
    {
        public static string Title => "Mobin Javari";
        public static string Author => "مبین جواری";
        public static string ThemeColor => "#238636";
    }

    public static class Meta
    {
        public static string DefaultAuthor => Site.Author;
        public static string DefaultDescription => "وب‌سایت شخصی مبین جواری - توسعه‌دهنده فول استک";
        public static string DefaultKeywords => "مبین جواری، برنامه نویس، توسعه دهنده وب، طراح سایت";
        public static string DefaultRobots => "index, follow";
        public static string DefaultOgType => "website";
        public static string DefaultTwitterCard => "summary";
        public static string DefaultImage => Content.Hero.ProfileImage;
    }

    public static class Content
    {
        public static class Header
        {
            public static string Title => Site.Author;

            public static List<MenuItem> MenuItems => new()
            {
                new() {
                    Url = "/",
                    Icon = "fas fa-home",
                    Title = "خانه",
                    Description = "صفحه اصلی سایت"
                },
                new() {
                    Url = "/Account",
                    Icon = "fas fa-user",
                    Title = "حساب کاربری",
                    Description = "صفحه حساب کاربری"
                },
                new() {
                    Url = $"/{Tools.Id}",
                    Icon = Tools.Icon,
                    Title = Tools.Title,
                    Description = Tools.Description
                }
            };
        }

        public static class Hero
        {
            public static string Title => "توسعه‌دهنده خلاق و حرفه‌ای";
            public static string Status => "توسعه‌دهنده Full-Stack";
            public static string Description => "متخصص در طراحی و توسعه وب‌سایت‌های مدرن و اپلیکیشن‌های موبایل";
            public static string ProfileImage => "https://avatars.githubusercontent.com/u/87239446?v=4";

            public static List<CallToAction> Actions => new()
            {
                new() {
                    Title = Contact.Title,
                    Target = Contact.Id,
                    IsPrimary = true
                },
                new() {
                    Title = Projects.Title,
                    Target = Projects.Id,
                    IsPrimary = false
                }
            };

            public static List<NavItem> Navigation => new()
            {
                new() {
                    Title = Projects.Subject,
                    Target = Projects.Id
                },
                new() {
                    Title = Skills.Subject,
                    Target = Skills.Id
                },
                new() {
                    Title = Contact.Subject,
                    Target = Contact.Id
                }
            };
        }

        public static class Projects
        {
            public static string Id => "Projects";
            public static string Subject => "نمونه کارها";
            public static string Title => "پروژه‌های من";
            public static string Description => "مجموعه‌ای از پروژه‌های برجسته که نشان‌دهنده تجربه و مهارت‌های من است";

            public static List<Project> Items => new()
            {
                new() {
                    Title = "موتور جستجوی گوگل",
                    Description = "همکاری در توسعه الگوریتم‌های رتبه‌بندی و بهینه‌سازی موتور جستجو",
                    Image = "/images/google.jpg",
                    Url = "https://google.com",
                    Technologies = new() { "Python", "TensorFlow", "BigQuery" }
                },
                new() {
                    Title = "هوش مصنوعی مایکروسافت",
                    Description = "توسعه سیستم‌های یادگیری ماشین برای محصولات آفیس ۳۶۵",
                    Image = "/images/microsoft.png",
                    Url = "https://microsoft.com",
                    Technologies = new() { "C#", ".NET", "Azure ML", "TypeScript" }
                },
                new() {
                    Title = "ChatGPT - OpenAI",
                    Description = "مشارکت در بهبود مدل‌های زبانی و توسعه API های هوش مصنوعی",
                    Image = "/images/openai.jpg",
                    Url = "https://openai.com",
                    Technologies = new() { "Python", "PyTorch", "GPT", "Docker" }
                },
                new() {
                    Title = "سیستم پرداخت اسپیس‌ایکس",
                    Description = "طراحی و پیاده‌سازی زیرساخت مالی برای پروژه‌های فضایی",
                    Image = "/images/spacex.jpg",
                    Url = "https://spacex.com",
                    Technologies = new() { "Rust", "Blockchain", "AWS", "Go" }
                },
                new() {
                    Title = "پلتفرم استریم نتفلیکس",
                    Description = "بهینه‌سازی سیستم پخش ویدیو و الگوریتم‌های پیشنهاددهنده",
                    Image = "/images/netflix.jpg",
                    Url = "https://netflix.com",
                    Technologies = new() { "Java", "Spring", "Redis", "Kafka" }
                },
                new() {
                    Title = "سیستم خودران تسلا",
                    Description = "توسعه نرم‌افزار تشخیص اشیاء و هدایت خودکار",
                    Image = "/images/tesla.jpg",
                    Url = "https://tesla.com",
                    Technologies = new() { "C++", "CUDA", "ROS", "Computer Vision" }
                }
            };
        }

        public static class Skills
        {
            public static string Id => "Skills";
            public static string Subject => "تخصص‌ها";
            public static string Title => "مهارت‌های من";
            public static string Description => "مجموعه توانمندی‌های فنی و تخصصی که در طول سال‌ها کسب کرده‌ام";

            public static List<Skill> Items => new()
            {
                new() {
                    Title = "Frontend",
                    Progress = 90,
                    Icon = "fas fa-code"
                },
                new() {
                    Title = "Backend",
                    Progress = 90,
                    Icon = "fas fa-database"
                },
                new() {
                    Title = "UI/UX",
                    Progress = 80,
                    Icon = "fas fa-palette"
                },
                new() {
                    Title = "DevOps",
                    Progress = 80,
                    Icon = "fas fa-server"
                }
            };
        }

        public static class Contact
        {
            public static string Id => "Contact";
            public static string Subject => "ارتباط با من";
            public static string Title => "تماس با من";
            public static string Description => "آماده همکاری در پروژه‌های جدید و پاسخگویی به سؤالات شما هستم";
            public static string Location => "تهران، ایران";
            public static string Email => _contactEmail;
        }

        public static class Tools
        {
            public static string Id => "Tools";
            public static string Title => "ابزارها";
            public static string Description => "مجموعه ابزارهای کاربردی برای تسهیل کارهای روزمره";
            public static string Icon => "fas fa-toolbox";
            public static MetaData Meta => new()
            {
                Description = "دسترسی به ابزارهای متنوع و کاربردی آنلاین برای مدیریت بهتر کارهای روزمره، افزایش بهره‌وری و صرفه‌جویی در زمان.",
                Keywords = "ابزار کاربردی, ابزار آنلاین, تسهیل کارهای روزمره, ابزار روزانه, ابزار رایگان, افزایش بهره‌وری, مدیریت زمان, ابزارهای مفید, ابزار دیجیتال, ابزار زندگی",
                Image = "/images/meta/tools.jpg",
                TwitterCard = "summary_large_image"
            };

            public static List<Tool> Items => new()
            {
                new() {
                    Id = "Grade Calculator",
                    Name = "محاسبه نمره",
                    Description = "محاسبه رتبه دانش‌آموز بر اساس نمره",
                    Icon = "fas fa-calculator",
                    LastUpdate = DateTimeOffset.FromUnixTimeSeconds(1741592305).DateTime,
                    Status = ToolStatus.Active,
                    Meta = new()
                    {
                        Description = "محاسبه رتبه دانش‌آموزان بر اساس نمرات. با وارد کردن نمره‌ها، رتبه کلاس یا گروه را به‌صورت دقیق محاسبه کنید.",
                        Keywords = "محاسبه رتبه, رتبه, رتبه دانش‌آموز, نمره, محاسبه نمره, ارزیابی نمره, ابزار رتبه‌بندی, نمره‌گذاری, مقایسه نمره, رتبه کلاسی, محاسبه رتبه دانش‌آموز, student ranking, grade calculator",
                    }
                },
                new() {
                    Id = "Age Group Calculator",
                    Name = "محاسبه گروه سنی",
                    Description = "محاسبه گروه سنی بر اساس سن",
                    Icon = "fas fa-calendar-alt",
                    LastUpdate = DateTimeOffset.FromUnixTimeSeconds(1743209105).DateTime,
                    Status = ToolStatus.Active,
                    Meta = new()
                    {
                        Description = "ابزار محاسبه گروه سنی بر اساس سن واقعی شما. با وارد کردن سن یا تاریخ تولد، گروه سنی خود را به‌صورت دقیق مشخص کنید.",
                        Keywords = "محاسبه سن, سن, گروه سنی, محاسبه گروه سنی, ابزار سن, سن دقیق, محاسبه سن آنلاین, تعیین گروه سنی, سن تولد, سن واقعی, age calculator, گروه‌بندی سنی",
                    }
                },
                new() {
                    Id = "Discount Calculator",
                    Name = "محاسبه تخفیف",
                    Description = "محاسبه تخفیف بر اساس درصد",
                    Icon = "fas fa-percent",
                    LastUpdate = DateTimeOffset.FromUnixTimeSeconds(1743209105).DateTime,
                    Status = ToolStatus.Active,
                    Meta = new()
                    {
                        Description = "ابزار آنلاین محاسبه تخفیف بر اساس درصد. کافیست قیمت و درصد تخفیف را وارد کنید تا قیمت نهایی را دقیق ببینید.",
                        Keywords = "تخفیف, محاسبه تخفیف, درصد تخفیف, قیمت با تخفیف, محاسبه قیمت, قیمت نهایی, ابزار تخفیف, محاسبه درصد, محاسبه قیمت نهایی, قیمت پس از تخفیف, discount calculator, درصد قیمت",
                    }
                },
                new() {
                    Id = "Password Checker",
                    Name = "بررسی رمز عبور",
                    Description = "بررسی رمز عبور بر اساس استانداردها",
                    Icon = "fas fa-lock",
                    LastUpdate = DateTimeOffset.FromUnixTimeSeconds(1743209105).DateTime,
                    Status = ToolStatus.Active,
                    Meta = new()
                    {
                        Description = "بررسی سریع و دقیق رمز عبور بر اساس استانداردهای امنیتی. ابزار رایگان برای سنجش قدرت پسورد و جلوگیری از نفوذ.",
                        Keywords = "رمز عبور, بررسی رمز عبور, امنیت رمز عبور, پسورد امن, امنیت سایبری, ابزار رمز عبور, ارزیابی پسورد, استاندارد رمز عبور, پسورد قوی, تست امنیت پسورد, password checker, امنیت آنلاین",
                    }
                },
                new() {
                    Id = "Date Converter",
                    Name = "تبدیل تاریخ",
                    Description = "تبدیل تاریخ شمسی به میلادی و برعکس",
                    Icon = "fas fa-exchange-alt",
                    LastUpdate = DateTimeOffset.FromUnixTimeSeconds(1787832000).DateTime,
                    Status = ToolStatus.Active,
                    Meta = new()
                    {
                        Description = "ابزار آنلاین تبدیل تاریخ شمسی به میلادی و برعکس، همراه با نمایش روز هفته. سریع، دقیق و رایگان.",
                        Keywords = "تبدیل تاریخ, تاریخ شمسی, تاریخ میلادی, تبدیل تاریخ شمسی به میلادی, تبدیل تاریخ میلادی به شمسی, تقویم شمسی, تقویم میلادی, محاسبه تاریخ, date converter, تبدیل سال",
                    }
                },
                new() {
                    Id = "BMI Calculator",
                    Name = "محاسبه شاخص توده بدنی",
                    Description = "محاسبه BMI بر اساس قد و وزن",
                    Icon = "fas fa-weight-scale",
                    LastUpdate = DateTimeOffset.FromUnixTimeSeconds(1787832000).DateTime,
                    Status = ToolStatus.Active,
                    Meta = new()
                    {
                        Description = "محاسبه شاخص توده بدنی (BMI) بر اساس قد و وزن. با یک محاسبه ساده، وضعیت وزنی خود را مشاهده کنید.",
                        Keywords = "BMI, شاخص توده بدنی, محاسبه BMI, محاسبه وزن ایده‌آل, کم‌وزنی, اضافه‌وزن, چاقی, ابزار سلامت, محاسبه چربی بدن, bmi calculator, تناسب اندام",
                    }
                },
                new() {
                    Id = "Loan Calculator",
                    Name = "محاسبه قسط وام",
                    Description = "محاسبه قسط ماهانه وام بر اساس مبلغ و نرخ سود",
                    Icon = "fas fa-landmark",
                    LastUpdate = DateTimeOffset.FromUnixTimeSeconds(1787832000).DateTime,
                    Status = ToolStatus.Active,
                    Meta = new()
                    {
                        Description = "محاسبه‌گر آنلاین قسط وام بر اساس مبلغ، نرخ سود سالانه و مدت بازپرداخت. مجموع سود و بازپرداخت را هم ببینید.",
                        Keywords = "محاسبه قسط وام, قسط وام, محاسبه سود وام, وام بانکی, اقساط مساوی, محاسبه اقساط, نرخ سود وام, loan calculator, محاسبه وام بانکی, برآورد قسط",
                    }
                },
                new() {
                    Id = "Number to Words",
                    Name = "تبدیل عدد به حروف",
                    Description = "تبدیل عدد به حروف فارسی برای چک و فاکتور",
                    Icon = "fas fa-spell-check",
                    LastUpdate = DateTimeOffset.FromUnixTimeSeconds(1787832000).DateTime,
                    Status = ToolStatus.Active,
                    Meta = new()
                    {
                        Description = "تبدیل آنلاین عدد به حروف فارسی، مناسب برای نوشتن مبلغ چک، فاکتور و قرارداد.",
                        Keywords = "تبدیل عدد به حروف, عدد به حروف فارسی, مبلغ به حروف, نوشتن چک, حروف عدد, تبدیل رقم به حروف, number to words, مبلغ فاکتور, عدد نویسی",
                    }
                }
            };
        }

        public static class Authentication
        {
            public static string Id => "Authentication";
            public static MetaData Meta => new()
            {
                Description = "ورود به حساب کاربری برای دسترسی به امکانات ویژه",
                Keywords = "ورود, حساب کاربری, ورود به سایت, ثبت نام, احراز هویت, امنیت, ورود امن, دسترسی ویژه",
                Image = "/images/meta/authentication.jpg",
                TwitterCard = "summary_large_image"
            };
        }

        public static class Footer
        {
            public static string AboutMe => "توسعه‌دهنده خلاق با تجربه در ساخت راه‌حل‌های دیجیتال و علاقه‌مند به تکنولوژی‌های نوین";

            public static List<SocialLink> SocialLinks => new()
            {
                new() {
                    Url = $"https://x.com/{_twitterUsername}",
                    Icon = "fab fa-twitter",
                    Color = "blue"
                },
                new() {
                    Url = $"https://t.me/{_telegramUsername}",
                    Icon = "fab fa-telegram",
                    Color = "blue"
                },
                new() {
                    Url = $"https://github.com/{_gitHubUsername}",
                    Icon = "fab fa-github",
                    Color = "slate"
                }
            };

            public static List<UsefulLink> UsefulLinks => new()
            {
                new() {
                    Url = $"https://github.com/{_gitHubUsername}/personal-website-cs",
                    Title = "لینک پروژه (گیتهاب)"
                },
                new() {
                    Url = $"https://t.me/{_telegramUsername}",
                    Title = "کانال تلگرام"
                }
            };
        }
    }
}

public record SocialLink
{
    public required string Url { get; init; }
    public required string Icon { get; init; }
    public required string Color { get; init; }
}

public record MenuItem
{
    public required string Url { get; init; }
    public required string Icon { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
}

public record UsefulLink
{
    public required string Url { get; init; }
    public required string Title { get; init; }
}

public record CallToAction
{
    public required string Title { get; init; }
    public required string Target { get; init; }
    public required bool IsPrimary { get; init; }
}

public record NavItem
{
    public required string Title { get; init; }
    public required string Target { get; init; }
}

public record Project
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Image { get; init; }
    public required string Url { get; init; }
    public required List<string> Technologies { get; init; }
}

public record Skill
{
    public required string Title { get; init; }
    public required int Progress { get; init; }
    public required string Icon { get; init; }
}

public record Tool
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Icon { get; init; }
    public required DateTime LastUpdate { get; init; }
    public required ToolStatus Status { get; init; }
    public required MetaData Meta { get; init; }
}

public record MetaData
{
    public string? Title { get; init; } = null;
    public required string Description { get; init; }
    public required string Keywords { get; init; }
    public string? Image { get; init; } = null;
    public string? OgType { get; init; } = null;
    public string? TwitterCard { get; init; } = null;
}

public enum ToolStatus
{
    Active,
    Maintenance,
    Development,
    Deprecated
}