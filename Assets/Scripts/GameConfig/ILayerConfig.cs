using UnityEngine;

public interface ILayerConfig
{
    public LayerMask AllDamageableLayer { get; }
    public LayerMask InvincibleLayer { get; }
    public LayerMask GroundLayer { get; }
}