using System;
using Manager;
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
    [Inject] private ISubscriber<DamageResult> _damageSub;
    [Inject] private ISubscriber<EffectData> _effectSub;
    
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
        
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        var bag = DisposableBag.CreateBuilder();
        _damageSub?.Subscribe(HandleDamageEvent).AddTo(bag);
        _effectSub?.Subscribe((x) =>
        {
            if (x == null) return;
            ShakeCamera(x.CameraShakeData);
        }).AddTo(bag);
        _subscription = bag.Build();
    }

    private void HandleDamageEvent(DamageResult e)
    {
        if (e.DamageInfo.EffectData == null) return;
        ShakeCamera(e.DamageInfo.EffectData.CameraShakeData);
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
