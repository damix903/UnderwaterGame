using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utility
{
    public static class TilemapExtension
    {
        /// <summary>
        /// Tilemapに幅と高さを指定して、指定した位置にタイルを配置する
        /// </summary>
        public static void SetTileArea(this Tilemap tilemap, TileBase tile, Vector3Int startPosition, int width, int height)
        {
            // boundsはタイルを配置する範囲を定義するための構造体で、開始位置とサイズを指定して作成する
            var bounds = new BoundsInt(startPosition, new Vector3Int(width, height, 1));
            var tileArray = new TileBase[width * height];
            
            for (int i = 0; i < tileArray.Length; i++)
                tileArray[i] = tile;

            // 個別でタイルを配置するのではなく、まとめて配置することでパフォーマンスが向上する
            tilemap.SetTilesBlock(bounds, tileArray);
        }

        /// <summary>
        /// Tilemap上の全てのタイルを探して、指定したタイルと一致するタイルの座標をリストで返す
        /// 座標はVector3とVector3Intを指定できる
        /// </summary>
        public static List<T> FindTiles<T>(this Tilemap tilemap, TileBase targetTile) where T : struct
        {
            if (!IsVector<T>()) return null;
            
            var positions = new List<T>();

            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos) || tilemap.GetTile(pos) != targetTile) continue;
                
                if (typeof(T) == typeof(Vector3))
                    positions.Add((T)(object)tilemap.CellToWorld(pos));
                else if (typeof(T) == typeof(Vector3Int))
                    positions.Add((T)(object)pos);
            }

            return positions;
        }

        /// <summary>
        /// TがVector3かVector3Intのどちらかであることを確認する
        /// </summary>
        private static bool IsVector<T>() where T : struct
        {
            // IsAssignableFromは、ある型が別の型に割り当て可能かどうかを判断するためのメソッドで、
            // ここではTがVector3またはVector3Intのどちらかであることを確認している
            if (!typeof(T).IsAssignableFrom(typeof(Vector3)) && !typeof(T).IsAssignableFrom(typeof(Vector3Int)))
            {
                Debug.LogError("Type T must be either Vector3 or Vector3Int.");
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Tilemap上の全てのタイルをTileBaseをキー、座標のリストを値とする辞書で返す
        /// 座標はVector3とVector3Intのどちらかで指定できる
        /// </summary>
        public static Dictionary<TileBase, List<T>> GetTileDictionary<T>(this Tilemap tilemap) where T : struct
        {
            if (!IsVector<T>()) return null;
            
            var tileDict = new Dictionary<TileBase, List<T>>();

            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(pos)) continue;

                var tile = tilemap.GetTile(pos);

                if (typeof(T) == typeof(Vector3))
                {
                    var worldPos = tilemap.CellToWorld(pos);

                    if (!tileDict.ContainsKey(tile))
                        tileDict[tile] = new List<T>();

                    tileDict[tile].Add((T)(object)worldPos);
                }
                else if (typeof(T) == typeof(Vector3Int))
                {
                    if (!tileDict.ContainsKey(tile))
                        tileDict[tile] = new List<T>();

                    tileDict[tile].Add((T)(object)pos);
                }
            }

            return tileDict;
        }
    }
}