using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Utility
{
    public static partial class Extensions
    {
        public static T GetPrivateField<T>(this object instance, string fieldname)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            FieldInfo field = type.GetField(fieldname, flag);
            return (T)field.GetValue(instance);
        }

        public static void AddNoRepeat<T>(this ICollection<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }
        public static void AddRangeNoRepeat<T>(this ICollection<T> list, ICollection<T> listAdd)
        {
            foreach (var item in listAdd)
            {
                list.AddNoRepeat(item);
            }
        }

    }
}
