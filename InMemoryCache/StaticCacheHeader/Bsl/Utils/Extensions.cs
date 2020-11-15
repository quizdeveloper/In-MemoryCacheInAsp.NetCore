using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace StaticCacheHeader.Bsl.Utils
{
    public static class Extensions
    {
        public static int ToInt(this object obj, int defaultValue = default(int))
        {
            if (obj == null)
                return defaultValue;

            int result;
            return !int.TryParse(obj.ToString(), out result) ? defaultValue : result;
        }

        public static long ToLong(this object obj, long defaultValue = default(long))
        {
            if (obj == null)
                return defaultValue;

            long result;
            if (!long.TryParse(obj.ToString(), out result))
                return defaultValue;

            return result;
        }

        public static double ToDouble(this object obj, double defaultValue = default(double))
        {
            if (obj == null)
                return defaultValue;

            double result;
            if (!double.TryParse(obj.ToString(), out result))
                return defaultValue;

            return result;
        }

        public static decimal ToDecimal(this object obj, decimal defaultValue = default(decimal))
        {
            if (obj == null)
                return defaultValue;

            decimal result;
            if (!decimal.TryParse(obj.ToString(), out result))
                return defaultValue;

            return result;
        }

        public static short ToShort(this object obj, short defaultValue = default(short))
        {
            if (obj == null)
                return defaultValue;

            short result;
            if (!short.TryParse(obj.ToString(), out result))
                return defaultValue;

            return result;
        }

        public static byte ToByte(this object obj, byte defaultValue = default(byte))
        {
            if (obj == null)
                return defaultValue;

            byte result;
            if (!byte.TryParse(obj.ToString(), out result))
                return defaultValue;

            return result;
        }

        public static string ToStringEx(this object obj, string defaultValue = default(string))
        {
            if (obj == null || obj.Equals(System.DBNull.Value))
                return defaultValue;

            return obj.ToString().Trim();
        }

        public static DateTime AsDateTime(this object obj, DateTime defaultValue = default(DateTime))
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                return defaultValue;

            DateTime result;
            if (!DateTime.TryParse(string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", obj), out result))
                return defaultValue;

            return result;
        }

        public static DateTime AsDateTimeFull(this object obj, DateTime defaultValue = default(DateTime))
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                return defaultValue;

            DateTime result;
            if (!DateTime.TryParse(string.Format("dd/MM/yyyy HH:mm:ss.fff", obj), out result))
                return defaultValue;

            return result;
        }

        public static DateTime AsDateTimeVn(this object obj, DateTime defaultValue = default(DateTime))
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                return defaultValue;
            try
            {
                return DateTime.ParseExact(obj.ToString().Replace('_','/'), "dd/MM/yyyy", CultureInfo.CurrentCulture);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static DateTime AsDateTimeVnFull(this object obj, DateTime defaultValue = default(DateTime))
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                return defaultValue;
            try
            {
                return DateTime.ParseExact(obj.ToString().Replace('_', '/'), "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static bool ToBool(this object obj, bool defaultValue = default(bool))
        {
            if (obj == null)
                return defaultValue;

            return new List<string>() { "yes", "y", "true", "1" }.Contains(obj.ToString().ToLower());
        }

        public static byte[] ToByteArray(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            return Convert.FromBase64String(s);
        }

        public static string JoinExt<T>(this string s, string separator, IEnumerable<T> values)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            return string.Format("{0}{1}{0}", separator, string.Join(separator, values));
        }

        public static string Base64String(this object obj)
        {
            if (obj == null)
                return null;
            return Convert.ToBase64String((byte[])obj);
        }

        public static System.Guid ToGuid(this object obj)
        {
            try
            {
                return new System.Guid(obj.ToString());
            }
            catch
            {
                return System.Guid.Empty;
            }
        }

        public static string ToGuidString(this object obj)
        {
            try
            {
                return Guid.NewGuid().ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static string FaceBookSubstring(this string str, string startString, string endString)
        {
            if (str.Contains(startString))
            {
                int iStart = str.IndexOf(startString, StringComparison.Ordinal) + startString.Length;
                int iEnd = str.IndexOf(endString, iStart, StringComparison.Ordinal);
                return str.Substring(iStart, (iEnd - iStart));
            }
            return null;
        }

        public static T GetAttribute<T>(this MemberInfo member, bool isRequired)
    where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }

        public static string FormatExt(this string instance, Dictionary<string, string> dicts)
        {
            if (string.IsNullOrEmpty(instance)) return instance;

            if (dicts == null || dicts.Count <= 0) return instance;

            string output = instance;
            //string strRegex = @"(?<ext>{.+?})";
            string strRegex = @"(?<ext>{(?<name>.+?)})";
            
            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Singleline;

            output = Regex.Replace(instance, strRegex, (_match) =>
            {
                string group = _match.Groups["ext"].Value.ToLower();
                string name = _match.Groups["name"].Value.ToLower();
                string value = dicts[name];
                return _match.Value.Replace(group, value);
            }, options);

            return output;
        }

        public static bool HasState(this long me, long validState)
        {
            return (me & validState) == validState;
        }

        public static long TurnOnState(this long me, long validState) { return me | validState; }

        public static long TurnOffState(this long me, long validState) { return me & ~validState; }

        public static string GetExtensionFile(this string fileName)
        {
            try
            {
                Regex reg = new Regex(@"\.[0-9a-z]+$");
                Match match = reg.Match(fileName);
                if (match.Success)
                {
                    return match.Groups[0].Value;
                }
            }
            catch
            {
                return string.Empty;
            }
            return string.Empty;
        }
    }
}