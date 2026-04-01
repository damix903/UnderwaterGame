using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected EntityData data;

    public void Initialize(EntityData entityData)
    {
        data = entityData;
        OnInitialize();
    }

    protected virtual void OnInitialize() { }
}