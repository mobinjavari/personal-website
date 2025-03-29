namespace MyWebApp.Utils;

public static class StringUtils
{
    public static string TruncateWords(string text, int wordCount = 3)
    {
        if (string.IsNullOrEmpty(text)) return text;
        var words = text.Split(' ');
        return words.Length > wordCount 
            ? string.Join(" ", words.Take(wordCount)) + "..."
            : text;
    }

    public static string ToUrl(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        return text.Replace(" ", "-").ToLower();
    }
}