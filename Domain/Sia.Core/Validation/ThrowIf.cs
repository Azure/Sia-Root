using System;

namespace Sia.Core.Validation
{
    public static class ThrowIf
    {
        public static TParam Null<TParam>(TParam o, string paramName)
            where TParam : class
            => o ?? throw new ArgumentNullException(paramName);

        public static string NullOrWhiteSpace(string input, string paramName)
        {
            // if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException("Value cannot be null or white space", paramName);

            return input;
        } 
    }
}
