using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Utility
{
    public sealed class ReferencedPropertyFinder : ExpressionVisitor
    {
        private readonly Type _ownerType;
        private readonly List<PropertyInfo> _properties = new List<PropertyInfo>();

        public ReferencedPropertyFinder(Type ownerType)
        {
            _ownerType = ownerType;
        }

        public IReadOnlyList<PropertyInfo> Properties
        {
            get { return _properties; }
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var propertyInfo = node.Member as PropertyInfo;
            if (propertyInfo != null && _ownerType.IsAssignableFrom(propertyInfo.DeclaringType))
            {
                // probably more filtering required
                _properties.Add(propertyInfo);
            }
            return base.VisitMember(node);
        }

        /// <summary>
        /// 获取表达式返回值 属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static IReadOnlyList<PropertyInfo> GetExpProperties<T>(Expression<Func<T, object>> exp)
        {
            var v = new ReferencedPropertyFinder(typeof(T));
            v.Visit(exp);
            return v.Properties;
        }
    }

}
