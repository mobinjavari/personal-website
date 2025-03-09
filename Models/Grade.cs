namespace MyWebApp.Models
{
    public class Grade
    {
        public double Score { get; set; }
        public string? Rank { get; set; }
        public string? Message { get; set; }
        public string? ColorClass { get; set; }

        public void CalculateRank()
        {
            if (Score < 0 || Score > 100)
            {
                Message = "نمره نامعتبر است";
                Rank = null;
                ColorClass = null;
                return;
            }

            (Rank, ColorClass) = Score switch
            {
                >= 90 => ("عالی", "text-green-700 dark:text-green-400 bg-green-50 dark:bg-green-900/30 border-green-200 dark:border-green-800"),
                >= 75 => ("خوب", "text-blue-700 dark:text-blue-400 bg-blue-50 dark:bg-blue-900/30 border-blue-200 dark:border-blue-800"),
                >= 50 => ("متوسط", "text-yellow-700 dark:text-yellow-400 bg-yellow-50 dark:bg-yellow-900/30 border-yellow-200 dark:border-yellow-800"),
                _ => ("ضعیف", "text-red-700 dark:text-red-400 bg-red-50 dark:bg-red-900/30 border-red-200 dark:border-red-800")
            };
        }
    }
} 