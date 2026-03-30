using System;
using UnityEngine;
using Unity.Cinemachine;
using MessagePipe;
using VContainer;


[RequireComponent(typeof(CinemachineImpulseSource), typeof(CinemachineImpulseListener))]
public class CameraManager : MonoBehaviour
{
    [Inject] private ISubscriber<DamageResult> _sub;
    [SerializeField] private CameraShakeData lightData;
    [SerializeField] private CameraShakeData heavyData;

    private CinemachineImpulseSource _impulseSource;
    private CinemachineImpulseListener _impulseListener;

    private IDisposable _subscription;
    
    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _impulseListener = GetComponent<CinemachineImpulseListener>();
        _subscription = _sub?.Subscribe(HandleDamageEvent);
    }

    public void ShakeCamera(CameraShakeData data, Vector3 position = default)
    {
        if (data == null) return;
        
        _impulseListener.Gain = data.Gain;
        _impulseListener.ReactionSettings.AmplitudeGain = data.AmplitudeGain;
        _impulseListener.ReactionSettings.FrequencyGain = data.FrequencyGain;
        _impulseListener.ReactionSettings.Duration = data.DurationGain;
        
        _impulseSource.ImpulseDefinition = data.CreateDefinition();
        _impulseSource.GenerateImpulseAt(position, data.Velocity);
    }

    private void HandleDamageEvent(DamageResult result)
    {
        ShakeCamera(result.DamageInfo.EffectData.CameraShakeData, result.Defender.transform.position);
    }

    // private CameraShakeData ResolveData(CameraShakeEvent e)
    // {
    //     return e.Type switch
    //     {
    //         CameraShakeType.Light => lightData,
    //         CameraShakeType.Heavy => heavyData,
    //         CameraShakeType.Custom => e.Data,
    //         _ => null
    //     };
    // }

    private void OnDestroy()
    {
        _subscription?.Dispose();
    }
}

// public enum CameraShakeType {None, Light, Heavy, Custom }
//
// public struct CameraShakeEvent
// {
//     public readonly CameraShakeType Type;
//     public readonly CameraShakeData Data;
//
//     public CameraShakeEvent(CameraShakeType type, CameraShakeData data)
//     {
//         Type = type;
//         Data = data;
//     }
// }
