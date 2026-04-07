using System;
using Stat;
using UnityEngine;
using VContainer;

public class ProjectileShooter : MonoBehaviour, IAimable, IAttackable
{
    [SerializeField] private ProjectileShooterData data;
    //[SerializeField] private float 

    private CharacterMovement _movement;
    [Inject] private ProjectileSpawnManager _manager;
    [Inject] private ILayerConfig _layerConfig;
    [Inject] private PlayerHealthManager _health;

    private float _cooldownTimer;
    private Vector2 _aimDir;
    private Vector2 ShootPos => transform.position + (Vector3)data.ShootCenter + (Vector3)_aimDir * data.ShootRadius;
    private LayerMask detectionLayer;

    private void Start()
    {
        detectionLayer = _layerConfig.AllDamageableLayer | _layerConfig.GroundLayer;
        _movement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        _cooldownTimer -= Time.deltaTime;
    }

    public void Fire(Vector2 dir)
    {
        if (!CanAttack) return;
        
        if (dir != default) _aimDir = dir;
        var obj = _manager.Spawn(data.ProjectileData, transform);
        
        obj.transform.position = ShootPos;
        obj.transform.right = _aimDir;
        obj.Initialize(data.ProjectileData, new ProjectileSpawnParams(gameObject, dir, detectionLayer, TeamID.Player), data.ProjectileData);

        bool overwrite = _movement.Velocity.x * dir.x > 0f;
        _movement.AddImpulseForce(-_aimDir * data.Recoil, overwrite);
        _movement.BlockMovement(.5f, 1f);
        // GetComponent<CharacterMovement2>().AddImpulseForce(-_aimDir * data.Recoil, true);
        // GetComponent<IHealth>().ChangeHealth(-data.Cost);
        _health.Consume(data.Cost);
        _cooldownTimer = data.Cooldown;
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
