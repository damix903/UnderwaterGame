using UnityEngine;

namespace ProjectileSystem
{
    [CreateAssetMenu(fileName = "PSD", menuName = "Data/Projectile/Shooter", order = 0)]
    public class ProjectileShooterData : ScriptableObject
    {
        [Header("Shooter Data")]
        [SerializeField] private float recoil = 5f;
        [SerializeField] private float cost = 5f;
        [SerializeField] private float cooldown = .5f;
        [SerializeField] private BaseProjectileData projectileData;

        [Header("Shoot Position")]
        [SerializeField] private Vector2 shootCenter;
        [SerializeField] private float shootRadius;
    
        public float Recoil => recoil;
        public float Cost => cost;
        public float Cooldown => cooldown;
        public BaseProjectileData ProjectileData => projectileData;
        public Vector2 ShootCenter => shootCenter;
        public float ShootRadius => shootRadius;
    }
}