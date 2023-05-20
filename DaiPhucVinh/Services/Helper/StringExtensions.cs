using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Hosting;

namespace DaiPhucVinh.Services.Helper
{
    public static class StringExtensions
    {
        public static string[] SplitIds(this string value)
        {
            return value.IsNullOrEmpty() ? new string[] { }
                : value.Split(';');
        }
        public static int ToInt(this string value)
        {
            return Convert.ToInt32(value);
        }
        public static int[] SplitIntIds(this string value)
        {
            var ids = value.SplitIds();
            return ids.Select(x => Convert.ToInt32(x)).ToArray();
        }
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        public static string ConvertToUnsign(string str)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = str.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty)
                        .Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public static string ToIds(this IEnumerable<string> source)
        {
            if (source == null || !source.Any())
                return "";
            return string.Join(";", source);
        }
        public static string ToIds(this IEnumerable<int> source)
        {
            if (source == null || !source.Any())
                return "";
            return string.Join(";", source);
        }
        public static string ConvertToUnSign(this string s)
        {
            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            var temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public static string CreateSlug(this string source)
        {
            var regex = new Regex(@"([^a-z0-9\-]?)");
            var slug = "";

            if (string.IsNullOrEmpty(source))
                return slug;
            slug = source.Trim().ToLower();
            slug = slug.Replace(' ', '-');
            slug = slug.Replace("---", "-");
            slug = slug.Replace("--", "-");
            slug = regex.Replace(slug, "");

            if (slug.Length * 2 < source.Length)
                return "";

            if (slug.Length > 100)
                slug = slug.Substring(0, 100);
            return slug;
        }
        public static string MapPath(this string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }

            //not hosted. For example, run in unit tests
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }


    }
}
