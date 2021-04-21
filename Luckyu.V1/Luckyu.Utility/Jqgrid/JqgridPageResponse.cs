using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Luckyu.Utility
{
    public class JqgridPageResponseBase
    {
        /// <summary>
        /// 每页条数
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int total
        {
            get
            {
                return count == 0 ? 0 : (records / count + (records % count > 0 ? 1 : 0));
            }
        }

        /// <summary>
        /// 总条数
        /// </summary>
        public int records { get; set; }

        protected IEnumerable<T> OrderChilds<T>(IEnumerable<T> enumerable, IEnumerable<T> all, PropertyInfo id, PropertyInfo parentId) where T : class, new()
        {
            foreach (var tempTable in enumerable)
            {
                yield return tempTable;

                T table = tempTable;

                var children = all.Where(x => parentId.GetValue(x).ToString() == id.GetValue(table).ToString());
                foreach (var child in OrderChilds(children, all, id, parentId))
                {
                    yield return child;
                }
            }
        }

    }

    public class JqgridPageResponse<T> : JqgridPageResponseBase where T : class, new()
    {
        public List<T> rows { get; set; }

        public JObject ToTreeGrid(string IdFeild, string parentIdFeild)
        {
            var treegrid = new JqgridPageResponse<JqgridTreeModel<T>>();
            treegrid.page = this.page;
            treegrid.records = this.records;
            treegrid.count = this.count;
            treegrid.rows = new List<JqgridTreeModel<T>>();

            var id = typeof(T).GetProperty(IdFeild);
            var parentId = typeof(T).GetProperty(parentIdFeild);
            var root = this.rows.Where(x =>
            {
                var temp = parentId.GetValue(x).ToString();
                return !this.rows.Exists(y => id.GetValue(y).ToString() == temp);
                // 自己的parentID不等于数据中的任意ID，即为根节点
            });
            this.rows = OrderChilds(root, this.rows, id, parentId).ToList();

            Func<T, int> GetLevel = null;
            GetLevel = g =>
            {
                if (g == null)
                {
                    return 0;
                }
                var parentid = (parentId.GetValue(g) ?? "").ToString();
                if (parentid.IsEmpty())
                {
                    return 0;
                }
                return GetLevel(this.rows.Where(x => id.GetValue(x).ToString() == parentId.GetValue(g).ToString()).FirstOrDefault()) + 1;
            };

            foreach (var item in this.rows)
            {
                var row = new JqgridTreeModel<T>();
                row._data = item;
                row.loaded = true;
                row.expanded = true;

                // 任意的parentID不等于自己的ID，即为叶子节点
                row.isLeaf = !this.rows.Exists(x => parentId.GetValue(x).ToString() == id.GetValue(item).ToString());
                row.level = GetLevel(item);

                treegrid.rows.Add(row);
            }
            var jsonObject = JObject.FromObject(treegrid);
            var dataRows = jsonObject.GetValue("rows");
            if (dataRows != null)
            {
                foreach (var item in dataRows.Children())
                {
                    var data = item["_data"];
                    if (data != null)
                    {
                        foreach (JProperty pro in data)
                        {
                            var jp = new JProperty(pro.Name, pro.Value);
                            item.Last.AddAfterSelf(jp);
                        }
                        item["_data"] = null;
                        item["_data"].Parent.Remove();
                    }
                }
            }
            return jsonObject;
        }

        public JObject ToTreeGrid(Expression<Func<T, object>> IdFeild, Expression<Func<T, object>> parentIdFeild)
        {
            var idproperties = ReferencedPropertyFinder.GetExpProperties(IdFeild);
            var parentIdproperties = ReferencedPropertyFinder.GetExpProperties(parentIdFeild);
            if (idproperties.Count < 1 || parentIdproperties.Count < 1)
            {
                return null;
            }
            var idname = idproperties[0].Name;
            var parentIdname = parentIdproperties[0].Name;
            return ToTreeGrid(idname, parentIdname);
        }
    }

    public class JqgridDatatablePageResponse : JqgridPageResponseBase
    {
        public DataTable rows { get; set; }
    }

    public class JqgridPageResponse : JqgridPageResponseBase
    {
        public object rows { get; set; }
    }
}
