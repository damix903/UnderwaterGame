using UnityEngine;

namespace Utility
{
    public static class GameObjectExtension
    {
        // コンポーネントを探し、見つからなければ追加する
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if (go == null) return null;
            
            if (!go.TryGetComponent(out T component))
                component = go.gameObject.AddComponent<T>();
            
            return component;
        }
    }
}