using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class UnityEngineExtension
    {
        /// <summary>
        /// レイヤーマスクが単一のレイヤーを表している場合、そのレイヤー番号を返す。
        /// </summary>
        public static int MaskToInt(this LayerMask mask)
        {
            int layer = 0;
            int value = mask.value;
            while (value > 0)
            {
                if ((value & 1) == 1) return layer;
                value >>= 1;
                layer++;
            }
            
            // ここまで来たら、マスクが単一のレイヤーを表していないことになる
            Debug.LogError($"LayerMask {mask} does not correspond to a single layer.");
            return -1;
        }
        
        /// <summary>
        /// ベクトルを受け取って8方向のうち最も近い方向を返す。
        /// </summary>
        public static Direction8 ToDirection8(this Vector2 dir)
        {
            if (dir == Vector2.zero) return Direction8.Right; // デフォルトは右
            
            // atan2は-180°から180°の範囲で角度を返すため、0°から360°の範囲に変換する
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;

            switch (angle)
            {
                case >= 337.5f:
                case < 22.5f:
                    return Direction8.Right;
                case >= 22.5f and < 67.5f:
                    return Direction8.UpRight;
                case >= 67.5f and < 112.5f:
                    return Direction8.Up;
                case >= 112.5f and < 157.5f:
                    return Direction8.UpLeft;
                case >= 157.5f and < 202.5f:
                    return Direction8.Left;
                case >= 202.5f and < 247.5f:
                    return Direction8.DownLeft;
                case >= 247.5f and < 292.5f:
                    return Direction8.Down;
                case >= 292.5f and < 337.5f:
                    return Direction8.DownRight;
                default:
                    // 理論上ここには来ないはず
                    Debug.LogError($"Unexpected angle {angle} for direction {dir}");
                    return Direction8.Right; // デフォルトは右
            }
        }
        
        /// <summary>
        /// startからtargetに向かって、duration秒かけて線形補間するコルーチン。
        /// </summary>
        public static IEnumerator LerpCoroutine(float start, float target, float duration, System.Action<float> onUpdate)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float value = Mathf.Lerp(start, target, elapsed / duration);
                onUpdate?.Invoke(value);
                yield return null;
            }
            onUpdate?.Invoke(target); // 最終的にターゲット値を確実に設定する
        }
    }
    
    public enum Direction8
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    }
}