using MessagePipe;
using UnityEngine;
using VContainer;

public class Enemy : PoolableEntity
{
    [Inject] private ISubscriber<ReleaseType> _subscriber;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _subscriber?.Subscribe((type) =>
        {
            if (type == ReleaseType.Enemy) Release();
        });
    }
}