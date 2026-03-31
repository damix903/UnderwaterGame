using UnityEngine;

public class SingleSpawnStrategy : ISpawnStrategy
{
    private readonly Transform _transform;

    public SingleSpawnStrategy(Transform transform)
    {
        _transform = transform;
    }    
    
    public Transform GetSpawnPoint() => _transform;
}