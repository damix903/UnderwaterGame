using System;
using UnityEngine;
using Unity.Cinemachine;
using MessagePipe;
using VContainer;


[RequireComponent(
        typeof(CinemachineCamera),
        typeof(CinemachineImpulseSource),
        typeof(CinemachineImpulseListener)
    )
]
public class CameraManager : MonoBehaviour
{
    [Inject] private ISubscriber<DamageResult> _sub;
    
    private CinemachineCamera _vCam;
    private CinemachinePositionComposer _vCamComposer;
    private CinemachineImpulseSource _impulseSource;
    private CinemachineImpulseListener _impulseListener;

    private IDisposable _subscription;
    
    private void Awake()
    {
        _vCam = GetComponent<CinemachineCamera>();
        _vCamComposer = _vCam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachinePositionComposer;
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

    public void SetLookaheadEnabled(bool isEnabled)
    {
        if (_vCamComposer == null) return;

        var settings = _vCamComposer.Lookahead;
        settings.Enabled = isEnabled;
        _vCamComposer.Lookahead = settings;
        _vCam.PreviousStateIsValid = isEnabled;
    }

    private void OnDestroy()
    {
        _subscription?.Dispose();
    }
}
