using UnityEditor.VersionControl;
using UnityEngine;

[CreateAssetMenu(menuName = "Data / Movement / ModeModifier", order = 0)]
public class ModeMovementModifier : ScriptableObject, IMovementModifier
{
    [SerializeField] private float moveSpeedMultiplier = 1f;
    [SerializeField] private float jumpHeightMultiplier = 1f;
    [SerializeField] private int overrideJumpCount = 1;
    [SerializeField] private float gravityScaleMultiplier = 1f;
    
    public MovementRuntimeStats Apply(MovementRuntimeStats stats)
    {
        // stats.movementMaxSpeed *= moveSpeedMultiplier;
        // stats.jumpHeight *= jumpHeightMultiplier;
        // stats.maxJumpCount = overrideJumpCount;
        // stats.defaultGravityScale *= gravityScaleMultiplier;
        // stats.fallGravity *= gravityScaleMultiplier;
        // stats.upwardGravity *= gravityScaleMultiplier;

        return stats;
    }
}