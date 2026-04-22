using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;

namespace Stage
{
    public class TileMapManager : MonoBehaviour
    {
        [SerializeField] private Tilemap platformTileMap;
        [SerializeField] private Tilemap markerTileMap;
        
        [SerializeField] private TileBaseSetting tileBaseSetting;
        
        [Header("Debug")]
        [SerializeField] private bool showMarkerTileMap = false;
        
        private Dictionary<TileBase, List<Vector3Int>> tileBaseDict = new();

        public List<Vector3Int> PlatformPos => tileBaseDict.GetValueOrDefault(tileBaseSetting.PlatformMarkerTileBase, new List<Vector3Int>());

        private void Awake()
        {
            BakeTileMap();
        }

        private void OnEnable()
        {
            markerTileMap.gameObject.SetActive(false);
            #if UNITY_EDITOR
            markerTileMap.gameObject.SetActive(showMarkerTileMap);
            #endif
        }

        public void ClearTileMap()
        {
            platformTileMap.ClearAllTiles();
        }

        public void SetPlatform(Vector3Int center, int width)
        {
            var halfWidth = (int)(width / 2f);
            var startPos = new Vector3Int(center.x - halfWidth, center.y, 0);
            
            platformTileMap.SetTileArea(tileBaseSetting.PlatformTileBase, startPos, width, 1);
        }
        
        [ContextMenu("Bake TileMap")]
        public void BakeTileMap()
        {
            tileBaseDict = new Dictionary<TileBase, List<Vector3Int>>();
            tileBaseDict = markerTileMap.GetTileDictionary<Vector3Int>();
            foreach (var pair in tileBaseDict)
            {
                Debug.Log($"{pair.Key.name} has {pair.Value.Count} positions.");
            }
        }
    }
}