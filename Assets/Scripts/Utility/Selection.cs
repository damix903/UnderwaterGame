using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utility
{
    public static class Selection
    {
        /// <summary>
        ///　条件に合うものを除外して指定した個数ランダムに選択する
        /// </summary>
        /// <param name="sourceList">選択する対象のリスト</param>
        /// <param name="count">選択する個数</param>
        /// <param name="excludedList">除外する条件リスト</param>
        public static List<T> RandomSelect<T>(IEnumerable<T> sourceList, int count, IEnumerable<T> excludedList = null)
        {
            if (sourceList == null) throw new ArgumentNullException(nameof(sourceList));

            var excludedSet = excludedList != null ? new HashSet<T>(excludedList) : new HashSet<T>();
            var filteredList = sourceList.Where(x => !excludedSet.Contains(x)).ToList();
            
            if (!filteredList.Any())
            {
                Debug.LogWarning("No items available for selection after exclusions.");
                return new List<T>();
            }
            
            int selectCount = Math.Min(count, filteredList.Count);
            var result = new List<T>(selectCount); // 先に容量を指定しておく

            for (int i = 0; i < selectCount; i++)
            {
                int randomIndex = Random.Range(0, filteredList.Count);
                result.Add(filteredList[randomIndex]);
                filteredList.RemoveAt(randomIndex); // 重複を避けるために選択したアイテムをリストから削除
            }
            
            if (result.Count < count)
            {
                Debug.LogWarning($"Requested {count} items, but only {result.Count} were available after exclusions.");
            }
            
            return result;
        }
    }
}