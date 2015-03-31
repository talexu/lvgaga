using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MathNet.Numerics;
using Newtonsoft.Json.Linq;

namespace LvService.Utilities
{
    public static class StringExtensions
    {
        #region Split

        public static string[] SplitByUnderline(this string str)
        {
            return String.IsNullOrEmpty(str) ? null : str.Split('_');
        }

        public static string SubstringByUnderline(this string str, int index)
        {
            return str.SplitByUnderline()[index];
        }

        public static string JoinByUnderline(this string[] strs)
        {
            return string.Join("_", strs);
        }
        #endregion

        #region Verification

        public static bool AllNotNullOrEmpty(this string[] strs)
        {
            return strs.All(str => !String.IsNullOrEmpty(str));
        }

        #endregion

        #region Cosine distance

        public static bool CosineEqual(this string str1, string str2)
        {
            return Math.Abs(str1.GetCosineDistanceByKv(str2)) <= 0;
        }

        public static double GetCosineDistanceByChar(this string str1, string str2)
        {
            if (str1 == null || str2 == null) return 1;
            if (str1.Equals(str2)) return 0;

            var cs1 = str1.ToCharArray();
            var cs2 = str2.ToCharArray();

            IDictionary<char, double> freq1 = new SortedDictionary<char, double>();
            foreach (var c in cs1.Concat(cs2))
            {
                freq1.AddOrUpdateValue(c, 0);
            }
            IDictionary<char, double> freq2 = new SortedDictionary<char, double>(freq1);

            var groupedCs1 = cs1.GroupBy(c => c);
            foreach (var charGroup in groupedCs1)
            {
                freq1[charGroup.Key] = charGroup.Count();
            }

            var groupedCs2 = cs2.GroupBy(c => c);
            foreach (var charGroup in groupedCs2)
            {
                freq2[charGroup.Key] = charGroup.Count();
            }

            var distance = Distance.Cosine(freq1.Values.ToArray(), freq2.Values.ToArray());
            return distance.Equals(double.NaN) ? 1 : distance;
        }

        public static double GetCosineDistanceByKv(this string str1, string str2)
        {
            if (str1 == null || str2 == null) return 1;
            if (str1.Equals(str2)) return 0;

            try
            {
                var node1 = JToken.Parse(str1);
                var node2 = JToken.Parse(str2);
                IDictionary<string, double> freq1 = new SortedDictionary<string, double>();
                IDictionary<string, double> freq2 = new SortedDictionary<string, double>();

                WalkJToken(node1, s =>
                {
                    freq1.TryPlusValue(s, 1);
                    freq2.TryPlusValue(s, 0);
                });
                WalkJToken(node2, s =>
                {
                    freq2.TryPlusValue(s, 1);
                    freq1.TryPlusValue(s, 0);
                });

                var distance = Distance.Cosine(freq1.Values.ToArray(), freq2.Values.ToArray());
                return distance.Equals(double.NaN) ? 1 : distance;
            }
            catch (Exception)
            {
                return str1.GetCosineDistanceByChar(str2);
            }
        }

        private static void TryPlusValue(this IDictionary<string, double> dic, string key, double value)
        {
            double v;
            if (dic.TryGetValue(key, out v))
            {
                dic[key] = v + value;
            }
            else
            {
                dic.Add(key, value);
            }
        }

        private static void WalkJToken(JToken node, Action<string> action)
        {
            switch (node.Type)
            {
                case JTokenType.Object:
                    foreach (var child in node.Children<JProperty>())
                    {
                        action(child.Name);
                        WalkJToken(child.Value, action);
                    }
                    break;
                case JTokenType.Array:
                    foreach (var child in node.Children())
                    {
                        WalkJToken(child, action);
                    }
                    break;
                default:
                    action(node.ToString());
                    break;
            }
        }

        #endregion

        #region Stream

        public static MemoryStream ToMemoryStream(this string str)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(str ?? ""));
        }

        public static string ToStringFromMemoryStream(this MemoryStream stream)
        {
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        #endregion

        #region Enum
        public static TEnum ToEnum<TEnum>(this string str) where TEnum : struct
        {
            TEnum result;
            return Enum.TryParse(str, out result) ? result : default(TEnum);
        }
        #endregion
    }
}