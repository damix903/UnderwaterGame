using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Data/GameConfig", order = 0)]
public class GameConfigData : ScriptableObject, ICollisionConfig
{
    [Header("Collision Config")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private CameraShakeData cameraShake;

    public float Damage => damage;
    public EffectData EffectData { get; private set; }

    private void OnValidate()
    {
        EffectData = new EffectData(new HitStopEvent(), cameraShake);
    }
}