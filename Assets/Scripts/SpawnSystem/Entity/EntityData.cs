using UnityEngine;

namespace SpawnSystem
{
    public class EntityData : ScriptableObject
    {
        [SerializeField] private GameObject prefab;
    
        public GameObject Prefab => prefab;
    }
}
