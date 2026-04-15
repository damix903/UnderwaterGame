using UnityEngine;

namespace Utility
{
    public static class UnityEngineExtension
    {
        /// <summary>
        /// レイヤーマスクが単一のレイヤーを表している場合、そのレイヤー番号を返す。
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
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
    }
}