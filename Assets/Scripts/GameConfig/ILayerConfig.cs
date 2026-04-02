using UnityEngine;

public interface ILayerConfig
{
    public LayerMask DamageableLayer { get; }
    public LayerMask InvincibleLayer { get; }
    public LayerMask IgnoreWallLayer { get; }
}