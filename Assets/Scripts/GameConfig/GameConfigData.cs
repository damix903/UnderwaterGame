using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Data/GameConfig", order = 0)]
public class GameConfigData : ScriptableObject, ICollisionConfig, ILayerConfig
{
    [Header("Collision Config")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private CameraShakeData cameraShake;

    [Header("Layer Config")] 
    [SerializeField] private LayerMask allDamageableLayer;
    [SerializeField] private LayerMask invincibleLayer;
    [SerializeField] private LayerMask groundLayer;

    public float Damage => damage;
    public EffectData EffectData { get; private set; }

    private void OnValidate()
    {
        EffectData = new EffectData(null, cameraShake);
    }

    public LayerMask AllDamageableLayer => allDamageableLayer;
    public LayerMask InvincibleLayer => invincibleLayer;
    public LayerMask GroundLayer => groundLayer;
}