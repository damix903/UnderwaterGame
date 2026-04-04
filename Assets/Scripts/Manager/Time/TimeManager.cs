using System;
using System.Collections;
using UnityEngine;
// using Cysharp.Threading.Tasks;
using MessagePipe;
using VContainer;

public class TimeManager : MonoBehaviour
{
    [Inject] private ISubscriber<DamageResult> _subscriber;
    private Coroutine _hitStopCoroutine;

    private void Awake()
    {
        _subscriber?.Subscribe(HandleDamageEvent);
    }

    private void HandleDamageEvent(DamageResult e)
    {
        var data = e.DamageInfo.EffectData.HitStopData;
        if (data == null)　return;
        
        StartHitStop(data);

        //Stop(info.Duration).Forget();
        //Debug.Log($"{info.Duration}, {info.TimeScale}");
    }

    public void StartHitStop(HitStopData data)
    {
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

    // private async UniTaskVoid Stop(float duration)
    // {
    //     await UniTask.Delay(TimeSpan.FromSeconds(duration), DelayType.UnscaledDeltaTime, cancellationToken: this.GetCancellationTokenOnDestroy());
    //     Time.timeScale = 1f;
    // }
}
