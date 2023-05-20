using System;
using System.ComponentModel;
using System.Reflection;

namespace DaiPhucVinh.Services.Helper
{
    public static class EnumerableExtensions
    {
        public static string GetDescription(this Enum value)
        {
            if (value == null)
                return string.Empty;
            // Get the Description attribute value for the enum value
            var fi = value.GetType().GetField(value.ToString());
            var attributes = fi.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
            return attributes?.Description ?? value.ToString();
        }
    }
}
