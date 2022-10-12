using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.Text.RegularExpressions;

namespace APITools.CommonTools
{
    public static class AssertHelper
    {
        public static Result<TSubject> NotNull<TSubject>(this TSubject? value, string parameterName)
        {
            return value is null ? new ArgumentNullException(parameterName) : value;
        }

        public static Result<string> NotNull(this string? value, string parameterName, int maxLength = int.MaxValue, int minLength = 0)
        {
            if (value is null)
            {
                return new ArgumentException($"{parameterName} can not be null!", parameterName);
            }
            if (value.Length > maxLength)
            {
                return new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
            }
            if (minLength > 0 && value.Length < minLength)
            {
                return new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
            }
            return value;
        }

        public static Result<string> NotNullOrWhiteSpace(this string? value, string parameterName, int maxLength = int.MaxValue, int minLength = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new ArgumentException($"{parameterName} can not be null, empty or white space!", parameterName);
            }
            if (value.Length > maxLength)
            {
                return new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
            }
            if (minLength > 0 && value.Length < minLength)
            {
                return new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
            }
            return value;
        }

        public static Result<string> NotNullOrEmpty(this string? value, string parameterName, int maxLength = int.MaxValue, int minLength = 0)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new ArgumentException($"{parameterName} can not be null or empty!", parameterName);
            }
            if (value.Length > maxLength)
            {
                return new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
            }
            if (minLength > 0 && value.Length < minLength)
            {
                return new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
            }
            return value;
        }

        public static Result<IEnumerable<TSubject>> NotNullOrEmpty<TSubject>(this IEnumerable<TSubject>? value, string parameterName)
        {
            return value is IEnumerable<TSubject> result && result.Any()
                ? Result.Success(result)
                : new ArgumentException($"{parameterName} can not be null or empty!", parameterName);
        }

        public static Result Match(this string value, string parameterName, Regex regex)
        {
            return regex.IsMatch(value)
                ? Result.Success()
                : new ArgumentException($"{parameterName} must match the regex '{regex}'", parameterName);
        }

        public static Result Length(this string? value, string parameterName, int maxLength, int minLength = 0)
        {
            if (minLength > 0)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return new ArgumentException($"{parameterName} can not be null or empty!", parameterName);
                }
                if (value.Length < minLength)
                {
                    return new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
                }
            }
            return value != null && value.Length > maxLength
                ? throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName)
                : Result.Success();
        }

        public static Result Positive(this sbyte value, string parameterName)
        {
            return value == 0
                ? new ArgumentException($"{parameterName} is equal to zero!")
                : (value < 0
                    ? new ArgumentException($"{parameterName} is less than zero!")
                    : Result.Success());
        }

        public static Result Positive(this short value, string parameterName)
        {
            return value == 0
                ? new ArgumentException($"{parameterName} is equal to zero!")
                : (value < 0
                    ? new ArgumentException($"{parameterName} is less than zero!")
                    : Result.Success());
        }

        public static Result Positive(this int value, string parameterName)
        {
            return value == 0
                ? new ArgumentException($"{parameterName} is equal to zero!")
                : (value < 0
                    ? new ArgumentException($"{parameterName} is less than zero!")
                    : Result.Success());
        }

        public static Result Positive(this long value, string parameterName)
        {
            return value == 0
                ? new ArgumentException($"{parameterName} is equal to zero!")
                : (value < 0
                    ? new ArgumentException($"{parameterName} is less than zero!")
                    : Result.Success());
        }

        public static Result Positive(this float value, string parameterName)
        {
            return value == 0
                ? new ArgumentException($"{parameterName} is equal to zero!")
                : (value < 0
                    ? new ArgumentException($"{parameterName} is less than zero!")
                    : Result.Success());
        }

        public static Result Positive(this double value, string parameterName)
        {
            return value == 0
                ? new ArgumentException($"{parameterName} is equal to zero!")
                : (value < 0
                    ? new ArgumentException($"{parameterName} is less than zero!")
                    : Result.Success());
        }

        public static Result Positive(this decimal value, string parameterName)
        {
            return value == 0
                ? new ArgumentException($"{parameterName} is equal to zero!")
                : (value < 0
                    ? new ArgumentException($"{parameterName} is less than zero!")
                    : Result.Success());
        }

        public static Result PositiveOrZero(this sbyte value, string parameterName)
        {
            return value < 0 ? new ArgumentException($"{parameterName} is less than zero!") : Result.Success();
        }

        public static Result PositiveOrZero(this short value, string parameterName)
        {
            return value < 0 ? new ArgumentException($"{parameterName} is less than zero!") : Result.Success();
        }

        public static Result PositiveOrZero(this int value, string parameterName)
        {
            return value < 0 ? new ArgumentException($"{parameterName} is less than zero!") : Result.Success();
        }

        public static Result PositiveOrZero(this long value, string parameterName)
        {
            return value < 0 ? new ArgumentException($"{parameterName} is less than zero!") : Result.Success();
        }

        public static Result PositiveOrZero(this float value, string parameterName)
        {
            return value < 0 ? new ArgumentException($"{parameterName} is less than zero!") : Result.Success();
        }

        public static Result PositiveOrZero(this double value, string parameterName)
        {
            return value < 0 ? new ArgumentException($"{parameterName} is less than zero!") : Result.Success();
        }

        public static Result PositiveOrZero(this decimal value, string parameterName)
        {
            return value < 0 ? new ArgumentException($"{parameterName} is less than zero!") : Result.Success();
        }

        public static Result Range(this byte value, string parameterName, byte minimumValue, byte maximumValue)
        {
            return value < minimumValue || value > maximumValue
                ? new ArgumentException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}!")
                : Result.Success();
        }

        public static Result Range(this sbyte value, string parameterName, sbyte minimumValue, sbyte maximumValue)
        {
            return value < minimumValue || value > maximumValue
                ? new ArgumentException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}!")
                : Result.Success();
        }

        public static Result Range(this ushort value, string parameterName, ushort minimumValue, ushort maximumValue)
        {
            return value < minimumValue || value > maximumValue
                ? new ArgumentException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}!")
                : Result.Success();
        }

        public static Result Range(this short value, string parameterName, short minimumValue, short maximumValue)
        {
            return value < minimumValue || value > maximumValue
                ? new ArgumentException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}!")
                : Result.Success();
        }

        public static Result Range(this uint value, string parameterName, uint minimumValue, uint maximumValue)
        {
            return value < minimumValue || value > maximumValue
                ? new ArgumentException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}!")
                : Result.Success();
        }

        public static Result Range(this int value, string parameterName, int minimumValue, int maximumValue)
        {
            return value < minimumValue || value > maximumValue
                ? new ArgumentException($"{parameterName} is ouTSubject of range min: {minimumValue} - max: {maximumValue}!")
                : Result.Success();
        }

        public static Result Range(this ulong value, string parameterName, ulong minimumValue, ulong maximumValue)
        {
            return value < minimumValue || value > maximumValue
                ? new ArgumentException($"{parameterName} is out of range min: {minimumValue} - max: {maximumValue}!")
                : Result.Success();
        }

        public static Result Range(this long value, string parameterName, long minimumValue, long maximumValue)
        {
            return value < minimumValue || value > maximumValue
                ? new ArgumentException($"{parameterName} is ouTSubject of range min: {minimumValue} - max: {maximumValue}!")
                : Result.Success();
        }

        public static Result Range(this float value, string parameterName, float minimumValue, float maximumValue)
        {
            return value < minimumValue || value > maximumValue
                ? new ArgumentException($"{parameterName} is ouTSubject of range min: {minimumValue} - max: {maximumValue}!")
                : Result.Success();
        }

        public static Result Range(this double value, string parameterName, double minimumValue, double maximumValue)
        {
            return value < minimumValue || value > maximumValue
                ? new ArgumentException($"{parameterName} is ouTSubject of range min: {minimumValue} - max: {maximumValue}!")
                : Result.Success();
        }

        public static Result Range(this decimal value, string parameterName, decimal minimumValue, decimal maximumValue)
        {
            return value < minimumValue || value > maximumValue
                ? new ArgumentException($"{parameterName} is ouTSubject of range min: {minimumValue} - max: {maximumValue}!")
                : Result.Success();
        }

        public static Result<TSubject> NotDefault<TSubject>(this TSubject value, string parameterName) where TSubject : struct
        {
            return value.Equals(default(TSubject))
                ? new ArgumentException($"{parameterName} has a default value!", parameterName)
                : value;
        }
    }
}
