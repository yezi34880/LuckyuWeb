using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Utility
{
    public static partial class Extensions
    {
        public static string GetDescription(this Type type)
        {
            var description = type.GetCustomAttribute<DescriptionAttribute>();
            if (description == null)
            {
                return null;
            }
            return description.Description;
        }

        public static string GetDescription(this MemberInfo member)
        {
            var description = member.GetCustomAttribute<DescriptionAttribute>();
            if (description == null)
            {
                return null;
            }
            return description.Description;
        }
        public static bool HasAttribute<T>(this MemberInfo type) where T : Attribute
        {
            var attribute = type.GetCustomAttribute<T>();
            return attribute != null;
        }

        public static bool HasAttribute<T>(this Type type) where T : Attribute
        {
            var attribute = type.GetCustomAttribute<T>();
            return attribute != null;
        }

    }
}
