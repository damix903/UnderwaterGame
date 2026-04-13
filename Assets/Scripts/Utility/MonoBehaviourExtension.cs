using UnityEngine;

namespace Utility
{
    public static class MonoBehaviourExtension
    {
        // コンポーネントを探し、見つからなければ追加する
        public static T GetOrAddComponent<T>(this MonoBehaviour mono) where T : Component
        {
            if (mono == null) return null;
            
            if (!mono.TryGetComponent(out T component))
                component = mono.gameObject.AddComponent<T>();
            
            return component;
        }
    }
}