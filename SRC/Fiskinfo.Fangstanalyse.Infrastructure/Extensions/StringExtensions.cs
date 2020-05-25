namespace Fiskinfo.Fangstanalyse.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static bool IsYearMonth(this string @this, string that)
        {
            // ASSUMES FORMAT (dd.MM.YYYY)
            if (string.IsNullOrEmpty(@this) || string.IsNullOrEmpty(that))
            {
                return false;
            }
            return @this.Contains(that.Substring(3));
        }
    }
}