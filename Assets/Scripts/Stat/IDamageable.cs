using System;
using UnityEngine;

public interface IDamageable
{
    public bool TakeDamage(DamageInfo info);
    public TeamID TeamID { get; }
    public bool IsAlive { get; }
    public DefenseState DefenseState { get; set; }
}

public struct DamageInfo
{
    public readonly Guid AttackID;
    public GameObject Attacker;
    public float Damage;
    public EffectData EffectData;

    public DamageInfo(GameObject attacker, float damage, EffectData effectData)
    {
        AttackID = Guid.NewGuid();
        Attacker = attacker;
        Damage = damage;
        EffectData = effectData;
    }
}

public struct DamageResult
{
    public DamageInfo DamageInfo;
    public GameObject Defender;
    public float AppliedDamage;
    public Vector2 HitPoint;
    public bool IsJust;
}

public struct DeathEvent
{
    public readonly GameObject Target;
    public readonly TeamID TeamID;
    public readonly DamageInfo Info;
    
    public DeathEvent(GameObject target, TeamID teamID, DamageInfo info)
    {
        Target = target;
        TeamID = teamID;
        Info = info;
    }
}

public enum TeamID { None, Player, Enemy, Others }

public enum DefenseState {None, Invincible, Just }

public struct EffectData
{
    public readonly HitStopEvent HitStop;
    public readonly CameraShakeEvent CameraShake;
    //public readonly DamageReactionData ReactionData;

    public EffectData(HitStopEvent hitStop, CameraShakeEvent cameraShake)//, DamageReactionData damageReactionData)
    {
        HitStop = hitStop;
        CameraShake = cameraShake;
        //ReactionData = damageReactionData;
    }
}