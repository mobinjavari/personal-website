namespace MyWebApp.Utils;

public static class PersianNumberConverter
{
    private static readonly string[] Units =
    {
        "", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه"
    };

    private static readonly string[] Teens =
    {
        "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده"
    };

    private static readonly string[] Tens =
    {
        "", "", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود"
    };

    private static readonly string[] Hundreds =
    {
        "", "صد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد"
    };

    private static readonly string[] Scales =
    {
        "", "هزار", "میلیون", "میلیارد", "تریلیون"
    };

    public static string ToWords(long number)
    {
        if (number == 0)
        {
            return "صفر";
        }

        var isNegative = number < 0;
        var absolute = Math.Abs(number);

        var groups = new List<int>();
        while (absolute > 0)
        {
            groups.Add((int)(absolute % 1000));
            absolute /= 1000;
        }

        var parts = new List<string>();
        for (var i = groups.Count - 1; i >= 0; i--)
        {
            if (groups[i] == 0)
            {
                continue;
            }

            var groupWords = ConvertThreeDigits(groups[i]);
            parts.Add(i > 0 ? $"{groupWords} {Scales[i]}" : groupWords);
        }

        var result = string.Join(" و ", parts);
        return isNegative ? $"منفی {result}" : result;
    }

    private static string ConvertThreeDigits(int number)
    {
        var parts = new List<string>();
        var hundredsDigit = number / 100;
        var remainder = number % 100;

        if (hundredsDigit > 0)
        {
            parts.Add(Hundreds[hundredsDigit]);
        }

        if (remainder is >= 10 and < 20)
        {
            parts.Add(Teens[remainder - 10]);
        }
        else
        {
            var tensDigit = remainder / 10;
            var unitsDigit = remainder % 10;

            if (tensDigit > 0)
            {
                parts.Add(Tens[tensDigit]);
            }

            if (unitsDigit > 0)
            {
                parts.Add(Units[unitsDigit]);
            }
        }

        return string.Join(" و ", parts);
    }
}
