using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [Serializable]
        internal class KeyValuePair
        {
            public TKey Key;
            public TValue Value;

            public KeyValuePair(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }
        
        [SerializeField] private List<KeyValuePair> keyValuePairs = new List<KeyValuePair>();
        
        public void OnBeforeSerialize()
        {
            // keyValuePairs.Clear();
            // // 辞書の内容をリストに変換して保存。インスペクターで表示されるようにする。
            // foreach (var pair in this)
            //     keyValuePairs.Add(new KeyValuePair(pair.Key, pair.Value));
        }

        public void OnAfterDeserialize()
        {
            Clear();
            // リストから辞書に内容を復元。重複キーがあれば警告を出す。
            foreach (var pair in keyValuePairs)
            {
                if (pair == null || pair.Key == null) continue;

                if (ContainsKey(pair.Key))
                {
                    Debug.LogWarning($"Duplicate key: {pair.Key} at {GetType().Name}.");
                    continue;
                }
                
                Add(pair.Key, pair.Value);
            }
        }
    }
}