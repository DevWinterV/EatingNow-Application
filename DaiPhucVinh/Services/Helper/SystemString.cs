using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static partial class String
    {
        public static bool HasValue(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }
    }
}
