using System;
using System.Collections;
using Manager;
using UnityEngine;
using MessagePipe;
using VContainer;

public class TimeManager : MonoBehaviour
{
    private Coroutine _hitStopCoroutine;
    private IDisposable _subscription;

    [Inject]
    public void Construct(ISubscriber<DamageResult> damageSub, ISubscriber<EffectData> effectSub)
    {
        var bag = DisposableBag.CreateBuilder();
        damageSub?.Subscribe(HandleDamageEvent).AddTo(bag);
        effectSub?.Subscribe((x) =>
        {
            if (x == null) return;
            StartHitStop(x.HitStopData);
        }).AddTo(bag);
        
        _subscription = bag.Build();
    }

    private void HandleDamageEvent(DamageResult e)
    {
        if (e.DamageInfo.EffectData == null) return;
        StartHitStop(e.DamageInfo.EffectData.HitStopData);
    }

    public void StartHitStop(HitStopData data)
    {
        if (data == null) return;
        StopHitStopCoroutine();
        _hitStopCoroutine = StartCoroutine(HitStopCoroutine(data));
    }

    private IEnumerator HitStopCoroutine(HitStopData data)
    {
        Time.timeScale = data.TimeScale;
        yield return new WaitForSecondsRealtime(data.Duration);
        Time.timeScale = 1f;
    }

    private void StopHitStopCoroutine()
    {
        if (_hitStopCoroutine != null) StopCoroutine(_hitStopCoroutine);
        _hitStopCoroutine = null;
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        StopHitStopCoroutine();
        _subscription?.Dispose();
    }
}
