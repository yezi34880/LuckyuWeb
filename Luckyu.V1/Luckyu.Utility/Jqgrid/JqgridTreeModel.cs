namespace Luckyu.Utility
{
    public class JqgridTreeModel<T> where T : class, new()
    {
        public T _data { get; set; }

        public int level { get; set; }

        public bool isLeaf { get; set; }

        public bool loaded { get; set; }

        public bool expanded { get; set; }

    }
}
