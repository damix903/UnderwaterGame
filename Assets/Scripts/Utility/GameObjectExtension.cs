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

        /// <summary>
        ///     GameObjectがカメラに映っているかどうかを判定する。
        /// </summary>
        /// <param name="margin">
        ///     許容範囲。カメラの外側にどれだけオブジェクトがあっても映っているとみなすかを指定する。パーセンテージで指定。
        /// </param>
        public static bool IsInCameraView(this GameObject obj, Camera camera, float margin = 0f)
        {
            if (obj == null || camera == null) return false;
            
            Vector3 viewportPos = camera.WorldToViewportPoint(obj.transform.position);
            return viewportPos.x >= -margin && viewportPos.x <= 1 + margin &&
                   viewportPos.y >= -margin && viewportPos.y <= 1 + margin &&
                   viewportPos.z > 0;
        }
    }
}