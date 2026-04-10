using System;
using ProjectileSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "PD_", menuName = "Data/Projectile/Normal")]
public class NormalProjectileData : BaseProjectileData
{
    public override void OnUpdate(Rigidbody2D rb, Projectile proj)
    {
        rb.linearVelocity = proj.transform.right * MaxSpeed;
    }
}
