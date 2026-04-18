using System.Collections;
using Manager;
using MessagePipe;
using Movement;
using SpawnSystem;
using UnityEngine;
using Utility;
using VContainer;

namespace ProjectileSystem
{
    public class ProjectileShooter : MonoBehaviour, IAimable, IAttackable
    {
        [SerializeField] private ProjectileShooterData data;

        private CharacterMovement _movement;
        [Inject] private IProjectileService _manager;
        [Inject] private ILayerConfig _layerConfig;
        [Inject] private ICostable _costable;
        [Inject] private IProjModifierProvider projModifierProvider;
        [Inject] private IPublisher<EffectData> _effectPub;

        private float _cooldownTimer;
        private Vector2 _aimDir;

        private Vector2 ShootPos =>
            transform.position + (Vector3)data.ShootCenter + (Vector3)_aimDir * data.ShootRadius;

        private LayerMask detectionLayer;
        
        private ShooterContext _context;
        private Coroutine _fireCo;

        private void Start()
        {
            detectionLayer = _layerConfig.AllDamageableLayer | _layerConfig.GroundLayer;
            _movement = GetComponent<CharacterMovement>();
            Debug.Log(_costable.GetType());
        }

        private void Update()
        {
            _cooldownTimer -= Time.deltaTime;
        }

        [SerializeField] private float angle;
        [SerializeField] private int count;
        [SerializeField] private int count2;
        [SerializeField] private float interval;
        
        public void Fire(Vector2 dir)
        {
            if (!CanAttack) return;
            
            _context = new ShooterContext
            {
                recoil = data.Recoil,
                cost = data.Cost,
                cooldown = data.Cooldown
            };
            if (projModifierProvider.ShooterModifiers is { Count: > 0 })
            {
                foreach (var mod in projModifierProvider.ShooterModifiers)
                    mod.Apply(_context);
            }

            _context.spreadCount = count;
            _context.spreadAngle = angle;
            _context.burstCount = count2;
            _context.burstInterval = interval;

            if (_fireCo != null) StopCoroutine(_fireCo);
            _fireCo = StartCoroutine(FireRoutine());

            _effectPub.Publish(data.EffectData);

            var overwrite = _movement.Velocity.x * dir.x > 0f;
            _movement.AddImpulseForce(-_aimDir * _context.recoil, overwrite);
            _movement.BlockMovement(.5f, 1f);
           
            _costable.Consume(_context.cost);
            _cooldownTimer = _context.cooldown;
        }

        private IEnumerator FireRoutine()
        {
            for (int i = 0; i < _context.burstCount; i++)
            {
                if (_context.spreadCount > 1) FireSpread(_aimDir);
                else ShootProjectile(default);
                
                if (_context.burstInterval > 0f)
                    yield return new WaitForSeconds(_context.burstInterval);
            }
        }

        private void ShootProjectile(Vector2 dir)
        {
            if (dir != default) _aimDir = dir;
   
            var proj = _manager.Spawn(data.ProjectileData, new SpawnPoint(ShootPos, _aimDir.ToQuaternion()));

            var param = new ProjectileSpawnParams(gameObject, detectionLayer, TeamID.Player);
            proj.Initialize(data.ProjectileData, param, data.ProjectileData, projModifierProvider.Modifiers);
        }

        private void FireSpread(Vector2 direction)
        {
            int count = _context.spreadCount;

            for (var i = 0; i < count; i++)
            {
                // 例えばspreadAngleが30でspreadCountが3なら、-15, 0, +15の3発になる
                var angle = -_context.spreadAngle / 2f + _context.spreadAngle / (count - 1) * i;
                var dir = Quaternion.Euler(0f, 0f, angle) * direction;
                ShootProjectile(dir);
            }
        }

        public void SetAimDirection(Vector2 direction) => _aimDir = direction;
        public void Attack(Vector2 direction = default) => Fire(direction);
        public bool CanAttack => _cooldownTimer <= 0f;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //if (Camera.main == null) return;

            if (!Application.isPlaying)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position + (Vector3)data.ShootCenter, data.ShootRadius);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(ShootPos, .2f);
            Gizmos.DrawRay(ShootPos, _aimDir * 1f);
        }
#endif
    }
}
