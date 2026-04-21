using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;
using Utility;

namespace ProjectileSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : PoolableEntity
    {
        private Rigidbody2D _rb;
        private IProjectileBehaviour _behaviour;

        private float _lifeTimeTimer;
        private readonly HashSet<GameObject> _hitObjects = new HashSet<GameObject>();
        private IReadOnlyList<IProjectileModifier> _modifiers = new List<IProjectileModifier>();

        public BaseProjectileData ProjData { get; private set; }
        public ProjectileSpawnParams SpawnParams { get; private set; }
        public ProjectileRunTimeStats RunTimeStats { get; set; }

        protected override void Awake()
        {
            base.Awake();
            _rb = gameObject.GetOrAddComponent<Rigidbody2D>();
            RunTimeStats = new ProjectileRunTimeStats();
        }

        private void FixedUpdate()
        {
            _behaviour?.OnUpdate(_rb, this);

            _lifeTimeTimer += Time.fixedDeltaTime;
            if (_lifeTimeTimer >= RunTimeStats.LifeTime)
                OnLifeTimeReached();
        }

        public void Initialize(BaseProjectileData projData, ProjectileSpawnParams param, IProjectileBehaviour behaviour, IReadOnlyList<IProjectileModifier> modifiers)
        {
            ProjData = projData;
            _behaviour = behaviour;
            SpawnParams = param;
            _modifiers = modifiers;

            _hitObjects.Clear();
            _lifeTimeTimer = 0f;
            RunTimeStats.Initialize(ProjData, SpawnParams);

            if (_modifiers is { Count: > 0 })
                foreach (var m in _modifiers)
                    m?.OnSpawn(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!CanHit(other)) return;
        
            _hitObjects.Add(other.gameObject);
            
            if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                if (damageable.TeamID != SpawnParams.OwnerTeamID)
                {
                    OnHitToDamageable(damageable);
                    return;
                }
            }
        
            OnHitToObstacle();
        }

        private bool CanHit(Collider2D other)
        {
            var obj = other.gameObject;
            return obj != SpawnParams.Owner 
                   && obj.IsInLayerMask(SpawnParams.DetectionLayer)
                   && !_hitObjects.Contains(obj);
        }

        private void OnHitToDamageable(IDamageable damageable)
        {
            if (_modifiers is { Count: > 0 })
                foreach (var m in _modifiers)
                    m?.OnHitToDamageable(this, damageable);
            
            damageable.TakeDamage(RunTimeStats.DamageInfo);
            HandleRelease();
        }

        private void OnHitToObstacle()
        {
            if (_modifiers is { Count: > 0 })
                foreach (var m in _modifiers)
                    m?.OnHitToObstacle(this);
            
            HandleRelease();
        }

        private void HandleRelease()
        {
            if (RunTimeStats.Durability <= 0f) Release();
        } 

        private void OnLifeTimeReached()
        {
            Release();
        }

        protected override ReleaseType ReleaseType => ReleaseType.Projectile;
    }
    
    public struct ProjectileSpawnParams
    {
        public readonly GameObject Owner;
        public readonly LayerMask DetectionLayer;
        public readonly TeamID OwnerTeamID;

        public ProjectileSpawnParams(GameObject owner, LayerMask detectionLayer, TeamID ownerTeamID)
        {
            Owner = owner;
            DetectionLayer = detectionLayer;
            OwnerTeamID = ownerTeamID;
        }
    }
}