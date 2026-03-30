using UnityEngine;

[CreateAssetMenu(menuName = "Data/Movement/Base Stats")]
public class MovementStats : ScriptableObject
{
    [Header("Move")]
    [SerializeField] private float movementMaxSpeed = 3f;
    [SerializeField] private float upwardMaxSpeed = 5f;
    [SerializeField] private float groundAccel = 60f;
    [SerializeField] private float airAccel = 30f;
    [SerializeField] private float groundDecel = 50f;
    [SerializeField] private float airDecel = 20f;
    [SerializeField] private float maxSpeed = 8f;

    [Header("Gravity")]
    [SerializeField] private float defaultGravityScale = 2f;
    [SerializeField] private float upwardGravityScale = 2f;
    
    [Header("Collision Check")]
    public Vector2 groundCheckOffset;
    public float groundCheckDist;
    public Vector2 wallCheckOffset;
    public float wallCheckDist;

    public MovementRuntimeStats Stats => new MovementRuntimeStats
    {
        movementMaxSpeed = movementMaxSpeed,
        groundAccel = groundAccel,
        airAccel = airAccel,
        groundDecel = groundDecel,
        airDecel = airDecel,
        maxSpeed = maxSpeed,
        defaultGravityScale = defaultGravityScale,
        upwardGravityScale = upwardGravityScale,
    };

}

public struct MovementRuntimeStats
{
    public float movementMaxSpeed;
    public float groundAccel;
    public float airAccel;
    public float groundDecel;
    public float airDecel;
    public float maxSpeed;
    public float defaultGravityScale;
    public float upwardGravityScale;
}
