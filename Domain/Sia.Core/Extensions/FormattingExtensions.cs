using System.Globalization;

namespace System
{
    public static class NumberFormattingExtensions
    {
        private const string integerFormat = "D";
        public static string ToPathTokenString(this long toConvert)
            => toConvert.ToString(integerFormat, CultureInfo.InvariantCulture);
    }

    public static class DateTimeFormattingExtensions
    {
        private const string UniversalSortableFormat = "u";
        public static string ToPathTokenString(this DateTime toConvert)
            => toConvert.ToString(UniversalSortableFormat, CultureInfo.InvariantCulture);
    }
}
