using System;
using System.Runtime.CompilerServices;

namespace Sia.Core.Validation
{
    public static class ThrowIf
    {
        public static TParam Null<TParam>(
            TParam o,
            string paramName,
            [CallerMemberName]string calledFromMember = null,
            [CallerFilePath]string calledFromFile = null,
            [CallerLineNumber]int sourceLineNumber = -1
        ) where TParam : class
            => o ?? throw (calledFromMember == ".ctor"
                ? (Exception)new ArgumentNullException(
                    paramName,
                    $"Null argument {paramName} passed to {calledFromMember}in file {calledFromFile} at line {sourceLineNumber}"
                )
                : new NullReferenceException(
                    $"{paramName} found to be null in method {calledFromMember} in file {calledFromFile} at line {sourceLineNumber}"
                )
            );

        public static string NullOrWhiteSpace(string input,
            string paramName,
            [CallerMemberName]string calledFromMember = null,
            [CallerFilePath]string calledFromFile = null,
            [CallerLineNumber]int sourceLineNumber = -1)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw (calledFromMember == ".ctor"
                    ? (Exception)new ArgumentException(
                        $"{paramName} found to be null or whitespace in method {calledFromMember} in file {calledFromFile} at line {sourceLineNumber}",
                        paramName
                    )
                    : new NullReferenceException(
                        $"{paramName} found to be null or whitespace in method {calledFromMember} in file {calledFromFile} at line {sourceLineNumber}"
                    )
                );

            return input;
        } 
    }
}
