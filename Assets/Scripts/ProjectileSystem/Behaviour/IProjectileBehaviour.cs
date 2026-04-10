using ProjectileSystem;
using UnityEngine;

public interface IProjectileBehaviour
{
    public void OnUpdate(Rigidbody2D rb, Projectile proj);
}