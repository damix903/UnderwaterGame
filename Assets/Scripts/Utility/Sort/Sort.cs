using System.Collections.Generic;

namespace Utility
{
    public static class Sort
    {
        /// <summary>
        /// SortPriorityの値が小さい順にソートするSort
        /// </summary>
        public static void SortByPriority<T>(this List<T> list) where T : ISortable
        {
            list.Sort((a, b) => a.SortPriority.CompareTo(b.SortPriority));
        }
        
    }
}