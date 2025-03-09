namespace MyWebApp.Projects.GradeCalculator.Models
{
    public class Grade
    {
        public double Score { get; set; }
        public string? Rank { get; set; }
        public string? Message { get; set; }

        public void CalculateRank()
        {
            if (Score < 0 || Score > 100)
            {
                Message = "نمره نامعتبر است";
                Rank = null;
                return;
            }

            Rank = Score switch
            {
                >= 90 => "عالی",
                >= 75 => "خوب",
                >= 50 => "متوسط",
                _ => "ضعیف"
            };
        }
    }
} 