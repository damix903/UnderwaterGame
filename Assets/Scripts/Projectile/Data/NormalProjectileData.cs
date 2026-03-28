using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PD_", menuName = "Data/Projectile/Normal")]
public class NormalProjectileData : ProjectileDataBase
{
    protected override Type ProjectileClass => typeof(NormalProjectileData);
    public override void OnUpdate(Rigidbody2D rb, ProjectileBase proj)
    {
        rb.linearVelocity = proj.transform.right * MaxSpeed;
    }
}
