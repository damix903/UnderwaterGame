using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility.Lottery
{
    public static class RandomSelector
    {
        public static T SelectWithWeight<T>(IEnumerable<T> items) where T : class, IWeightable
        {
            if (items == null || !items.Any()) return null;

            var itemList = items.ToList();
            int total = itemList.Sum(x => x.Weight);
            int randomValue = Random.Range(0, total);

            int current = 0;

            foreach (var i in itemList)
            {
                current += i.Weight;
                if (current > randomValue) return i;
            }

            return itemList.Last();
        }
    }
}