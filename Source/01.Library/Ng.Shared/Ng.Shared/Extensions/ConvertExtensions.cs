using System.Diagnostics;
using System.Globalization;

namespace Ng.Shared.Extensions
{
    /// <summary>
    /// 안전한 숫자 타입 변환을 위한 확장 메서드 클래스
    /// 범위를 벗어나거나 유효하지 않은 값에 대해 예외를 발생시킵니다.
    /// </summary>
    public static class ConvertExtensions
    {
        #region Double to Decimal Conversions

        /// <summary>
        /// double을 decimal로 안전하게 변환합니다.
        /// </summary>
        /// <param name="value">변환할 double 값</param>
        /// <returns>변환된 decimal 값</returns>
        /// <exception cref="InvalidCastException">NaN 또는 무한대 값일 때</exception>
        /// <exception cref="OverflowException">decimal 범위를 초과할 때</exception>
        public static decimal ToDecimalSafe(this double value)
        {
            // NaN 체크
            if (double.IsNaN(value))
            {
                Debug.Assert(false, "Cannot convert NaN to decimal");
                throw new InvalidCastException($"Cannot convert NaN to decimal.");
            }

            // 무한대 체크
            if (double.IsPositiveInfinity(value))
            {
                Debug.Assert(false, "Cannot convert positive infinity to decimal");
                throw new InvalidCastException($"Cannot convert positive infinity to decimal.");
            }

            if (double.IsNegativeInfinity(value))
            {
                Debug.Assert(false, "Cannot convert negative infinity to decimal");
                throw new InvalidCastException($"Cannot convert negative infinity to decimal.");
            }

            // decimal 범위 체크
            if (value > (double)decimal.MaxValue)
            {
                Debug.Assert(false, $"Value {value} exceeds decimal.MaxValue");
                throw new OverflowException($"Value {value} exceeds decimal.MaxValue ({decimal.MaxValue}).");
            }

            if (value < (double)decimal.MinValue)
            {
                Debug.Assert(false, $"Value {value} is below decimal.MinValue");
                throw new OverflowException($"Value {value} is below decimal.MinValue ({decimal.MinValue}).");
            }

            try
            {
                return Convert.ToDecimal(value);
            }
            catch (OverflowException ex)
            {
                Debug.Assert(false, $"Failed to convert {value} to decimal");
                throw new OverflowException($"Failed to convert {value} to decimal.", ex);
            }
        }

        /// <summary>
        /// double을 decimal로 변환 시도하고, 성공 여부를 반환합니다.
        /// </summary>
        public static bool TryToDecimalSafe(this double value, out decimal result, out string errorMessage)
        {
            result = 0m;
            errorMessage = string.Empty;

            try
            {
                result = value.ToDecimalSafe();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        #endregion

        #region Float to Decimal Conversions

        /// <summary>
        /// float을 decimal로 안전하게 변환합니다.
        /// </summary>
        /// <exception cref="InvalidCastException">NaN 또는 무한대 값일 때</exception>
        /// <exception cref="OverflowException">decimal 범위를 초과할 때</exception>
        public static decimal ToDecimalSafe(this float value)
        {
            if (float.IsNaN(value))
            {
                Debug.Assert(false, "Cannot convert NaN to decimal");
                throw new InvalidCastException($"Cannot convert NaN to decimal.");
            }

            if (float.IsPositiveInfinity(value))
            {
                Debug.Assert(false, "Cannot convert positive infinity to decimal");
                throw new InvalidCastException($"Cannot convert positive infinity to decimal.");
            }

            if (float.IsNegativeInfinity(value))
            {
                Debug.Assert(false, "Cannot convert negative infinity to decimal");
                throw new InvalidCastException($"Cannot convert negative infinity to decimal.");
            }

            if (value > (float)decimal.MaxValue)
            {
                Debug.Assert(false, $"Value {value} exceeds decimal.MaxValue");
                throw new OverflowException($"Value {value} exceeds decimal.MaxValue.");
            }

            if (value < (float)decimal.MinValue)
            {
                Debug.Assert(false, $"Value {value} is below decimal.MinValue");
                throw new OverflowException($"Value {value} is below decimal.MinValue.");
            }

            try
            {
                return Convert.ToDecimal(value);
            }
            catch (OverflowException ex)
            {
                Debug.Assert(false, $"Failed to convert {value} to decimal");
                throw new OverflowException($"Failed to convert {value} to decimal.", ex);
            }
        }

        /// <summary>
        /// float을 decimal로 변환 시도하고, 성공 여부를 반환합니다.
        /// </summary>
        public static bool TryToDecimalSafe(this float value, out decimal result, out string errorMessage)
        {
            result = 0m;
            errorMessage = string.Empty;

            try
            {
                result = value.ToDecimalSafe();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        #endregion

        #region Decimal to Other Types

        /// <summary>
        /// decimal을 double로 안전하게 변환합니다.
        /// </summary>
        /// <exception cref="OverflowException">double 범위를 초과할 때</exception>
        public static double ToDoubleSafe(this decimal value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch (OverflowException)
            {
                Debug.Assert(false, $"Value {value} cannot be converted to double");
                throw new OverflowException($"Value {value} cannot be converted to double.");
            }
        }

        /// <summary>
        /// decimal을 double로 변환 시도합니다.
        /// </summary>
        public static bool TryToDoubleSafe(this decimal value, out double result, out string errorMessage)
        {
            result = 0d;
            errorMessage = string.Empty;

            try
            {
                result = value.ToDoubleSafe();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// decimal을 float로 안전하게 변환합니다.
        /// </summary>
        /// <exception cref="OverflowException">float 범위를 초과할 때</exception>
        public static float ToFloatSafe(this decimal value)
        {
            double doubleValue = (double)value;
            if (doubleValue > float.MaxValue)
            {
                Debug.Assert(false, $"Value {value} exceeds float.MaxValue");
                throw new OverflowException($"Value {value} exceeds float.MaxValue.");
            }

            if (doubleValue < float.MinValue)
            {
                Debug.Assert(false, $"Value {value} is below float.MinValue");
                throw new OverflowException($"Value {value} is below float.MinValue.");
            }

            try
            {
                return Convert.ToSingle(value);
            }
            catch (OverflowException)
            {
                Debug.Assert(false, $"Value {value} cannot be converted to float");
                throw new OverflowException($"Value {value} cannot be converted to float.");
            }
        }

        /// <summary>
        /// decimal을 float로 변환 시도합니다.
        /// </summary>
        public static bool TryToFloatSafe(this decimal value, out float result, out string errorMessage)
        {
            result = 0f;
            errorMessage = string.Empty;

            try
            {
                result = value.ToFloatSafe();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        #endregion

        #region Integer Conversions

        /// <summary>
        /// decimal을 int로 안전하게 변환합니다.
        /// </summary>
        /// <exception cref="OverflowException">int 범위를 초과할 때</exception>
        public static int ToInt32Safe(this decimal value)
        {
            if (value > int.MaxValue)
            {
                Debug.Assert(false, $"Value {value} exceeds int.MaxValue");
                throw new OverflowException($"Value {value} exceeds int.MaxValue ({int.MaxValue}).");
            }

            if (value < int.MinValue)
            {
                Debug.Assert(false, $"Value {value} is below int.MinValue");
                throw new OverflowException($"Value {value} is below int.MinValue ({int.MinValue}).");
            }

            try
            {
                return Convert.ToInt32(value);
            }
            catch (OverflowException ex)
            {
                Debug.Assert(false, $"Failed to convert {value} to int");
                throw new OverflowException($"Failed to convert {value} to int.", ex);
            }
        }

        /// <summary>
        /// decimal을 int로 변환 시도합니다.
        /// </summary>
        public static bool TryToInt32Safe(this decimal value, out int result, out string errorMessage)
        {
            result = 0;
            errorMessage = string.Empty;

            try
            {
                result = value.ToInt32Safe();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// double을 int로 안전하게 변환합니다.
        /// </summary>
        /// <exception cref="InvalidCastException">NaN 또는 무한대 값일 때</exception>
        /// <exception cref="OverflowException">int 범위를 초과할 때</exception>
        public static int ToInt32Safe(this double value)
        {
            if (double.IsNaN(value))
            {
                Debug.Assert(false, "Cannot convert NaN to int");
                throw new InvalidCastException($"Cannot convert NaN to int.");
            }

            if (double.IsInfinity(value))
            {
                Debug.Assert(false, "Cannot convert infinity to int");
                throw new InvalidCastException($"Cannot convert infinity to int.");
            }

            if (value > int.MaxValue)
            {
                Debug.Assert(false, $"Value {value} exceeds int.MaxValue");
                throw new OverflowException($"Value {value} exceeds int.MaxValue ({int.MaxValue}).");
            }

            if (value < int.MinValue)
            {
                Debug.Assert(false, $"Value {value} is below int.MinValue");
                throw new OverflowException($"Value {value} is below int.MinValue ({int.MinValue}).");
            }

            try
            {
                return Convert.ToInt32(value);
            }
            catch (OverflowException ex)
            {
                Debug.Assert(false, $"Failed to convert {value} to int");
                throw new OverflowException($"Failed to convert {value} to int.", ex);
            }
        }

        /// <summary>
        /// double을 int로 변환 시도합니다.
        /// </summary>
        public static bool TryToInt32Safe(this double value, out int result, out string errorMessage)
        {
            result = 0;
            errorMessage = string.Empty;

            try
            {
                result = value.ToInt32Safe();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// decimal을 long으로 안전하게 변환합니다.
        /// </summary>
        /// <exception cref="OverflowException">long 범위를 초과할 때</exception>
        public static long ToInt64Safe(this decimal value)
        {
            if (value > long.MaxValue)
            {
                Debug.Assert(false, $"Value {value} exceeds long.MaxValue");
                throw new OverflowException($"Value {value} exceeds long.MaxValue ({long.MaxValue}).");
            }

            if (value < long.MinValue)
            {
                Debug.Assert(false, $"Value {value} is below long.MinValue");
                throw new OverflowException($"Value {value} is below long.MinValue ({long.MinValue}).");
            }

            try
            {
                return Convert.ToInt64(value);
            }
            catch (OverflowException ex)
            {
                Debug.Assert(false, $"Failed to convert {value} to long");
                throw new OverflowException($"Failed to convert {value} to long.", ex);
            }
        }

        /// <summary>
        /// decimal을 long으로 변환 시도합니다.
        /// </summary>
        public static bool TryToInt64Safe(this decimal value, out long result, out string errorMessage)
        {
            result = 0L;
            errorMessage = string.Empty;

            try
            {
                result = value.ToInt64Safe();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// decimal을 short로 안전하게 변환합니다.
        /// </summary>
        /// <exception cref="OverflowException">short 범위를 초과할 때</exception>
        public static short ToInt16Safe(this decimal value)
        {
            if (value > short.MaxValue)
            {
                Debug.Assert(false, $"Value {value} exceeds short.MaxValue");
                throw new OverflowException($"Value {value} exceeds short.MaxValue ({short.MaxValue}).");
            }

            if (value < short.MinValue)
            {
                Debug.Assert(false, $"Value {value} is below short.MinValue");
                throw new OverflowException($"Value {value} is below short.MinValue ({short.MinValue}).");
            }

            return Convert.ToInt16(value);
        }

        /// <summary>
        /// decimal을 byte로 안전하게 변환합니다.
        /// </summary>
        /// <exception cref="OverflowException">byte 범위를 초과할 때</exception>
        public static byte ToByteSafe(this decimal value)
        {
            if (value > byte.MaxValue)
            {
                Debug.Assert(false, $"Value {value} exceeds byte.MaxValue");
                throw new OverflowException($"Value {value} exceeds byte.MaxValue ({byte.MaxValue}).");
            }

            if (value < byte.MinValue)
            {
                Debug.Assert(false, $"Value {value} is below byte.MinValue");
                throw new OverflowException($"Value {value} is below byte.MinValue ({byte.MinValue}).");
            }

            return Convert.ToByte(value);
        }

        #endregion

        #region Nullable Conversions

        /// <summary>
        /// nullable double을 decimal로 안전하게 변환합니다.
        /// </summary>
        /// <exception cref="ArgumentNullException">값이 null일 때</exception>
        /// <exception cref="InvalidCastException">NaN 또는 무한대 값일 때</exception>
        /// <exception cref="OverflowException">decimal 범위를 초과할 때</exception>
        public static decimal ToDecimalSafe(this double? value)
        {
            if (!value.HasValue)
            {
                Debug.Assert(false, "Cannot convert null to decimal");
                throw new ArgumentNullException(nameof(value), "Cannot convert null to decimal.");
            }

            return value.Value.ToDecimalSafe();
        }

        /// <summary>
        /// nullable double을 decimal?로 안전하게 변환합니다. (null 허용)
        /// </summary>
        public static decimal? ToDecimalSafeOrNull(this double? value)
        {
            if (!value.HasValue)
            {
                return null;
            }

            return value.Value.ToDecimalSafe();
        }

        /// <summary>
        /// nullable float을 decimal로 안전하게 변환합니다.
        /// </summary>
        /// <exception cref="ArgumentNullException">값이 null일 때</exception>
        /// <exception cref="InvalidCastException">NaN 또는 무한대 값일 때</exception>
        /// <exception cref="OverflowException">decimal 범위를 초과할 때</exception>
        public static decimal ToDecimalSafe(this float? value)
        {
            if (!value.HasValue)
            {
                Debug.Assert(false, "Cannot convert null to decimal");
                throw new ArgumentNullException(nameof(value), "Cannot convert null to decimal.");
            }

            return value.Value.ToDecimalSafe();
        }

        /// <summary>
        /// nullable float을 decimal?로 안전하게 변환합니다. (null 허용)
        /// </summary>
        public static decimal? ToDecimalSafeOrNull(this float? value)
        {
            if (!value.HasValue)
            {
                return null;
            }

            return value.Value.ToDecimalSafe();
        }

        /// <summary>
        /// nullable decimal을 int로 안전하게 변환합니다.
        /// </summary>
        /// <exception cref="ArgumentNullException">값이 null일 때</exception>
        /// <exception cref="OverflowException">int 범위를 초과할 때</exception>
        public static int ToInt32Safe(this decimal? value)
        {
            if (!value.HasValue)
            {
                Debug.Assert(false, "Cannot convert null to int");
                throw new ArgumentNullException(nameof(value), "Cannot convert null to int.");
            }

            return value.Value.ToInt32Safe();
        }

        /// <summary>
        /// nullable decimal을 int?로 안전하게 변환합니다. (null 허용)
        /// </summary>
        public static int? ToInt32SafeOrNull(this decimal? value)
        {
            if (!value.HasValue)
            {
                return null;
            }

            return value.Value.ToInt32Safe();
        }

        #endregion

        #region String to Numeric Conversions

        /// <summary>
        /// 문자열을 decimal로 안전하게 변환합니다.
        /// </summary>
        /// <exception cref="ArgumentNullException">문자열이 null이거나 비어있을 때</exception>
        /// <exception cref="FormatException">잘못된 형식일 때</exception>
        /// <exception cref="OverflowException">decimal 범위를 초과할 때</exception>
        public static decimal ToDecimalSafe(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Debug.Assert(false, "Cannot convert null or empty string to decimal");
                throw new ArgumentNullException(nameof(value), "Cannot convert null or empty string to decimal.");
            }

            if (!decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                Debug.Assert(false, $"'{value}' is not a valid decimal format");
                throw new FormatException($"'{value}' is not a valid decimal format.");
            }

            return result;
        }

        /// <summary>
        /// 문자열을 decimal로 변환 시도합니다.
        /// </summary>
        public static bool TryToDecimalSafe(this string value, out decimal result)
        {
            result = 0m;

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// 문자열을 double로 안전하게 변환합니다.
        /// </summary>
        /// <exception cref="ArgumentNullException">문자열이 null이거나 비어있을 때</exception>
        /// <exception cref="FormatException">잘못된 형식일 때</exception>
        public static double ToDoubleSafe(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Debug.Assert(false, "Cannot convert null or empty string to double");
                throw new ArgumentNullException(nameof(value), "Cannot convert null or empty string to double.");
            }

            if (!double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                Debug.Assert(false, $"'{value}' is not a valid double format");
                throw new FormatException($"'{value}' is not a valid double format.");
            }

            if (double.IsNaN(result))
            {
                Debug.Assert(false, "Parsed value is NaN");
                throw new InvalidCastException($"Parsed value is NaN.");
            }

            if (double.IsInfinity(result))
            {
                Debug.Assert(false, "Parsed value is infinity");
                throw new InvalidCastException($"Parsed value is infinity.");
            }

            return result;
        }

        /// <summary>
        /// 문자열을 double로 변환 시도합니다.
        /// </summary>
        public static bool TryToDoubleSafe(this string value, out double result)
        {
            result = 0d;

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (!double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                return false;
            }

            if (double.IsNaN(result) || double.IsInfinity(result))
            {
                result = 0d;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 문자열을 int로 안전하게 변환합니다.
        /// </summary>
        /// <exception cref="ArgumentNullException">문자열이 null이거나 비어있을 때</exception>
        /// <exception cref="FormatException">잘못된 형식일 때</exception>
        public static int ToInt32Safe(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Debug.Assert(false, "Cannot convert null or empty string to int");
                throw new ArgumentNullException(nameof(value), "Cannot convert null or empty string to int.");
            }

            if (!int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out int result))
            {
                Debug.Assert(false, $"'{value}' is not a valid int format");
                throw new FormatException($"'{value}' is not a valid int format.");
            }

            return result;
        }

        /// <summary>
        /// 문자열을 int로 변환 시도합니다.
        /// </summary>
        public static bool TryToInt32Safe(this string value, out int result)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// 문자열을 long으로 안전하게 변환합니다.
        /// </summary>
        /// <exception cref="ArgumentNullException">문자열이 null이거나 비어있을 때</exception>
        /// <exception cref="FormatException">잘못된 형식일 때</exception>
        public static long ToInt64Safe(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Debug.Assert(false, "Cannot convert null or empty string to long");
                throw new ArgumentNullException(nameof(value), "Cannot convert null or empty string to long.");
            }

            if (!long.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out long result))
            {
                Debug.Assert(false, $"'{value}' is not a valid long format");
                throw new FormatException($"'{value}' is not a valid long format.");
            }

            return result;
        }

        /// <summary>
        /// 문자열을 long으로 변환 시도합니다.
        /// </summary>
        public static bool TryToInt64Safe(this string value, out long result)
        {
            result = 0L;

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return long.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
        }

        #endregion

        #region Rounding Helpers

        /// <summary>
        /// decimal 값을 지정된 소수점 자리수로 반올림합니다.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">소수점 자리수가 유효하지 않을 때</exception>
        public static decimal RoundSafe(this decimal value, int decimals = 2)
        {
            if (decimals < 0 || decimals > 28)
            {
                Debug.Assert(false, $"Decimal places must be between 0 and 28. Provided: {decimals}");
                throw new ArgumentOutOfRangeException(nameof(decimals), $"Decimal places must be between 0 and 28. Provided: {decimals}");
            }

            return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// double 값을 decimal로 변환 후 지정된 소수점 자리수로 반올림합니다.
        /// </summary>
        public static decimal RoundToDecimalSafe(this double value, int decimals = 2)
        {
            var decimalValue = value.ToDecimalSafe();
            return decimalValue.RoundSafe(decimals);
        }

        /// <summary>
        /// double 값을 지정된 소수점 자리수로 반올림합니다.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">소수점 자리수가 유효하지 않을 때</exception>
        /// <exception cref="InvalidCastException">NaN 또는 무한대 값일 때</exception>
        public static double RoundSafe(this double value, int decimals = 2)
        {
            if (double.IsNaN(value))
            {
                Debug.Assert(false, "Cannot round NaN value");
                throw new InvalidCastException("Cannot round NaN value.");
            }

            if (double.IsInfinity(value))
            {
                Debug.Assert(false, "Cannot round infinity value");
                throw new InvalidCastException("Cannot round infinity value.");
            }

            if (decimals < 0 || decimals > 15)
            {
                Debug.Assert(false, $"Decimal places must be between 0 and 15 for double. Provided: {decimals}");
                throw new ArgumentOutOfRangeException(nameof(decimals), $"Decimal places must be between 0 and 15 for double. Provided: {decimals}");
            }

            return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// decimal 값을 올림합니다.
        /// </summary>
        public static decimal CeilingSafe(this decimal value)
        {
            return Math.Ceiling(value);
        }

        /// <summary>
        /// decimal 값을 내림합니다.
        /// </summary>
        public static decimal FloorSafe(this decimal value)
        {
            return Math.Floor(value);
        }

        /// <summary>
        /// decimal 값을 0에서 먼 방향으로 절사합니다.
        /// </summary>
        public static decimal TruncateSafe(this decimal value)
        {
            return Math.Truncate(value);
        }

        #endregion

        #region Collection Extensions

        /// <summary>
        /// 컬렉션의 평균을 decimal로 안전하게 계산합니다.
        /// </summary>
        /// <exception cref="ArgumentNullException">소스가 null일 때</exception>
        /// <exception cref="InvalidOperationException">컬렉션이 비어있을 때</exception>
        public static decimal AverageToDecimalSafe<T>(this IEnumerable<T> source, Func<T, double> selector)
        {
            if (source == null)
            {
                Debug.Assert(false, "Source cannot be null");
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                Debug.Assert(false, "Selector cannot be null");
                throw new ArgumentNullException(nameof(selector));
            }

            var list = source.ToList();
            if (!list.Any())
            {
                Debug.Assert(false, "Cannot calculate average of empty collection");
                throw new InvalidOperationException("Cannot calculate average of empty collection.");
            }

            var average = list.Average(selector);
            return average.ToDecimalSafe();
        }

        /// <summary>
        /// 컬렉션의 평균을 decimal로 계산 시도합니다. (비어있을 경우 기본값 반환)
        /// </summary>
        public static decimal AverageToDecimalSafeOrDefault<T>(this IEnumerable<T> source, Func<T, double> selector, decimal defaultValue = 0m)
        {
            if (source == null || selector == null)
            {
                return defaultValue;
            }

            var list = source.ToList();
            if (!list.Any())
            {
                return defaultValue;
            }

            try
            {
                var average = list.Average(selector);
                return average.ToDecimalSafe();
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 컬렉션의 합계를 decimal로 안전하게 계산합니다.
        /// </summary>
        /// <exception cref="ArgumentNullException">소스가 null일 때</exception>
        public static decimal SumToDecimalSafe<T>(this IEnumerable<T> source, Func<T, double> selector)
        {
            if (source == null)
            {
                Debug.Assert(false, "Source cannot be null");
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                Debug.Assert(false, "Selector cannot be null");
                throw new ArgumentNullException(nameof(selector));
            }

            var sum = source.Sum(selector);
            return sum.ToDecimalSafe();
        }

        /// <summary>
        /// 컬렉션의 합계를 decimal로 계산 시도합니다. (비어있을 경우 0 반환)
        /// </summary>
        public static decimal SumToDecimalSafeOrDefault<T>(this IEnumerable<T> source, Func<T, double> selector, decimal defaultValue = 0m)
        {
            if (source == null || selector == null)
            {
                return defaultValue;
            }

            try
            {
                var sum = source.Sum(selector);
                return sum.ToDecimalSafe();
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region Validation Helpers

        /// <summary>
        /// 값이 decimal로 안전하게 변환 가능한지 검증합니다.
        /// </summary>
        public static bool CanConvertToDecimal(this double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                return false;
            }

            if (value > (double)decimal.MaxValue || value < (double)decimal.MinValue)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 값이 decimal로 안전하게 변환 가능한지 검증합니다.
        /// </summary>
        public static bool CanConvertToDecimal(this float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                return false;
            }

            if (value > (float)decimal.MaxValue || value < (float)decimal.MinValue)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 값이 int로 안전하게 변환 가능한지 검증합니다.
        /// </summary>
        public static bool CanConvertToInt32(this double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                return false;
            }

            if (value > int.MaxValue || value < int.MinValue)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 값이 int로 안전하게 변환 가능한지 검증합니다.
        /// </summary>
        public static bool CanConvertToInt32(this decimal value)
        {
            return value >= int.MinValue && value <= int.MaxValue;
        }

        /// <summary>
        /// 값이 long으로 안전하게 변환 가능한지 검증합니다.
        /// </summary>
        public static bool CanConvertToInt64(this decimal value)
        {
            return value >= long.MinValue && value <= long.MaxValue;
        }

        /// <summary>
        /// 문자열이 유효한 decimal 형식인지 검증합니다.
        /// </summary>
        public static bool IsValidDecimal(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        }

        /// <summary>
        /// 문자열이 유효한 double 형식인지 검증합니다.
        /// </summary>
        public static bool IsValidDouble(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (!double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return false;
            }

            return !double.IsNaN(result) && !double.IsInfinity(result);
        }

        /// <summary>
        /// 문자열이 유효한 int 형식인지 검증합니다.
        /// </summary>
        public static bool IsValidInt32(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        }

        #endregion

        #region Percentage Helpers

        /// <summary>
        /// 백분율을 계산합니다. (value / total * 100)
        /// </summary>
        /// <exception cref="DivideByZeroException">총합이 0일 때</exception>
        public static decimal CalculatePercentage(this decimal value, decimal total)
        {
            if (total == 0)
            {
                Debug.Assert(false, "Cannot calculate percentage with zero total");
                throw new DivideByZeroException("Cannot calculate percentage with zero total.");
            }

            return (value / total * 100m).RoundSafe(2);
        }

        /// <summary>
        /// 백분율을 계산 시도합니다. (value / total * 100)
        /// </summary>
        public static decimal CalculatePercentageOrDefault(this decimal value, decimal total, decimal defaultValue = 0m)
        {
            if (total == 0)
            {
                return defaultValue;
            }

            return (value / total * 100m).RoundSafe(2);
        }

        /// <summary>
        /// 백분율 값을 소수로 변환합니다. (50 -> 0.5)
        /// </summary>
        public static decimal ToDecimalFraction(this decimal percentage)
        {
            return percentage / 100m;
        }

        /// <summary>
        /// 소수를 백분율 값으로 변환합니다. (0.5 -> 50)
        /// </summary>
        public static decimal ToPercentage(this decimal fraction)
        {
            return (fraction * 100m).RoundSafe(2);
        }

        #endregion
    }

    /// <summary>
    /// 변환 관련 사용자 정의 예외
    /// </summary>
    public class SafeConversionException : Exception
    {
        public object AttemptedValue { get; }
        public Type SourceType { get; }
        public Type TargetType { get; }

        public SafeConversionException(object attemptedValue, Type sourceType, Type targetType, string message)
            : base(message)
        {
            AttemptedValue = attemptedValue;
            SourceType = sourceType;
            TargetType = targetType;
        }

        public SafeConversionException(object attemptedValue, Type sourceType, Type targetType, string message, Exception innerException)
            : base(message, innerException)
        {
            AttemptedValue = attemptedValue;
            SourceType = sourceType;
            TargetType = targetType;
        }
    }
}
