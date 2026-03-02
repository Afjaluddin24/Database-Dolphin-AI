namespace Dolphin_AI.Helpers
{
    public static class ContentFilterHelper
    {
        private static readonly string[] BannedWords =
        {
            "sex",
            "porn",
            "xxx",
            "adult",
            "nude"
        };

        public static bool ContainsAdultContent(params string[] fields)
        {
            foreach (var field in fields)
            {
                if (!string.IsNullOrEmpty(field))
                {
                    foreach (var word in BannedWords)
                    {
                        if (field.Contains(word, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}