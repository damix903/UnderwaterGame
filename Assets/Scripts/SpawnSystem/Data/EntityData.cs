using UnityEngine;

public class EntityData : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    
    public GameObject Prefab => prefab;
}
