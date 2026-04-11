using System;
using System.Collections.Generic;
using ProjectileSystem;
using Stat;
using UnityEngine;
using VContainer;

namespace ProjectileSystem
{
    public class ProjectileShooter : MonoBehaviour, IAimable, IAttackable
{
    [SerializeField] private ProjectileShooterData data;
    [SerializeField] private PierceModifier mod1; 

    private CharacterMovement _movement;
    [Inject] private ProjectileSpawnManager _manager;
    [Inject] private ILayerConfig _layerConfig;
    [Inject] private ICostable _health;

    private float _cooldownTimer;
    private Vector2 _aimDir;
    private Vector2 ShootPos => transform.position + (Vector3)data.ShootCenter + (Vector3)_aimDir * data.ShootRadius;
    private LayerMask detectionLayer;

    private void Start()
    {
        detectionLayer = _layerConfig.AllDamageableLayer | _layerConfig.GroundLayer;
        _movement = GetComponent<CharacterMovement>();
        Debug.Log(_health.GetType());
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
        var param = new ProjectileSpawnParams(gameObject, dir, detectionLayer, TeamID.Player);
        var mods = new List<IProjectileModifier>();
        mods.Add(mod1);
        obj.Initialize(data.ProjectileData, param , data.ProjectileData, mods);

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

}
