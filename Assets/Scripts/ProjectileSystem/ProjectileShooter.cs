using Manager;
using MessagePipe;
using Movement;
using UnityEngine;
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
        [Inject] private IModifierProvider _modifierProvider;
        [Inject] private IPublisher<EffectData> _effectPub;

        private float _cooldownTimer;
        private Vector2 _aimDir;

        private Vector2 ShootPos =>
            transform.position + (Vector3)data.ShootCenter + (Vector3)_aimDir * data.ShootRadius;

        private LayerMask detectionLayer;

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

        public void Fire(Vector2 dir)
        {
            if (!CanAttack) return;

            ShootProjectile(dir);
            _effectPub.Publish(data.EffectData);

            var overwrite = _movement.Velocity.x * dir.x > 0f;
            _movement.AddImpulseForce(-_aimDir * data.Recoil, overwrite);
            _movement.BlockMovement(.5f, 1f);
           
            _costable.Consume(data.Cost);
            _cooldownTimer = data.Cooldown;
        }

        private void ShootProjectile(Vector2 dir)
        {
            if (dir != default) _aimDir = dir;
            var obj = _manager.Spawn(data.ProjectileData, transform);

            obj.transform.position = ShootPos;
            obj.transform.right = _aimDir;
            var param = new ProjectileSpawnParams(gameObject, detectionLayer, TeamID.Player);

            obj.Initialize(data.ProjectileData, param, data.ProjectileData, _modifierProvider.Modifiers);
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
