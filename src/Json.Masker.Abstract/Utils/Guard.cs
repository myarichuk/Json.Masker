using System;

namespace Json.Masker.Abstract.Utils;

/// <summary>
/// Provides guard clauses to validate method arguments.
/// </summary>
public static class Guard
{
    /// <summary>
    /// Ensures that the specified argument is not <c>null</c>.
    /// </summary>
    /// <typeparam name="T">
    /// The reference type of the argument being checked.
    /// </typeparam>
    /// <param name="value">
    /// The argument value to check.
    /// </param>
    /// <param name="parameterName">
    /// The name of the parameter being validated. 
    /// This will be used in the exception if the argument is <c>null</c>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is <c>null</c>.
    /// </exception>
    public static void NotNull<T>(T? value, string parameterName)
        where T : class
    {
        if (value is null)
        {
            throw new ArgumentNullException(parameterName);
        }
    }
}