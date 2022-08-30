using System;
using System.Globalization;

namespace ds.pms.helpers.Extentions
{
    public static class General
    {
        /// <summary>
        /// Converts a value to a double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this string value)
        {
            double val = default;
            double.TryParse(value, out val);
            return val;
        }

        /// <summary>
        /// Converts a value to a int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this string value)
        {
            int val = default;
            int.TryParse(value, out val);
            return val;
        }

        /// <summary>
        /// Converts a value to a long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this object value)
        {
            if (value != null)
            {
                int val = default;
                int.TryParse(value.ToString(), out val);
                return val;
            }
            else
                return default;
        }

        /// <summary>
        /// Converts nullable value to int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this int? value)
        {
            if (value != null && value.HasValue)
                return value.Value;
            return default;
        }

        /// <summary>
        /// Converts a value to a long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(this string value)
        {
            long val = default;
            long.TryParse(value, out val);
            return val;
        }

        /// <summary>
        /// Converts nullable value to long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(this long? value)
        {
            if (value != null && value.HasValue)
                return value.Value;
            return default(int);
        }

        /// <summary>
        /// Converts a value to a long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(this decimal value)
        {
            decimal rounded = Math.Round(value.ToDecimal(), 0, MidpointRounding.AwayFromZero);
            return (long)rounded;
        }

        /// <summary>
        /// Converts a value to a long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(this object value)
        {
            if (value != null)
            {
                long val = default;
                long.TryParse(value.ToString(), out val);
                return val;
            }
            else
                return default;
        }

        /// <summary>
        /// Converts a value to a decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value)
        {
            decimal val = default;
            decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out val);
            return val;
        }

        /// <summary>
        /// Converts a value to a decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object value)
        {
            if (value != null)
            {
                decimal val = default;
                decimal.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out val);
                return val;
            }
            else
                return default;
        }

        /// <summary>
        /// Round decimal away from zero
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal RoundDecimal(this decimal value, int decimals = 0)
        {
            return Math.Round(value, decimals, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Round object value away from zero decimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal RoundDecimal(this object value, int decimals = 0)
        {
            return Math.Round(value.ToDecimal(), decimals, MidpointRounding.AwayFromZero);
        }


        /// <summary>
        /// Converts a value to a float
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this string value)
        {
            float val = default;
            if (!string.IsNullOrEmpty(value))
                float.TryParse(value, out val);
            return val;
        }

        /// <summary>
        /// Converts a value to a float
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this object value)
        {
            if (value != null)
            {
                float val = default;
                float.TryParse(value.ToString(), out val);
                return val;
            }
            else
                return default;
        }

        /// <summary>
        /// Converts a value to a boolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(this string value)
        {
            bool val = default;
            bool.TryParse(value, out val);
            return val;
        }

        /// <summary>
        /// Converts a nullable value to a boolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(this bool? value)
        {
            if (value != null) return value.Value;
            return false;
        }

        /// <summary>
        /// Converts a value to a TimeSpan
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this string value)
        {
            TimeSpan val = default;
            TimeSpan.TryParse(value, out val);
            return val;
        }

        /// <summary>
        /// Converts a string to DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value)
        {
            if (value != null)
            {
                DateTime val = default;
                DateTime.TryParse(value, out val);
                return val;
            }
            else
                return default;
        }

        /// <summary>
        /// Converts a string to DateTime with spercific format
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value, string format)
        {
            if (value != null)
            {
                DateTime val = default;
                DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out val);
                return val;
            }
            else
                return default;
        }

        /// <summary>
        /// Converts a string to DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object value)
        {
            if (value != null)
            {
                DateTime val = default;
                DateTime.TryParse(value.ToString(), out val);
                return val;
            }
            else
                return default;
        }

        /// <summary>
        /// Converts a nullable DateTime? to DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this DateTime? value)
        {
            if (value != null) return value.Value;
            return default;
        }

        /// <summary>
        /// Converts a value to a Guid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string value)
        {
            Guid val = default;
            if (!string.IsNullOrEmpty(value))
                Guid.TryParse(value, out val);
            return val;
        }

        /// <summary>
        /// Converts a nullable Guid to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(this Guid? value)
        {
            if (value != null && value.HasValue && value.Value != Guid.Empty)
                return value.Value.ToString();
            return string.Empty;
        }
    }
}
