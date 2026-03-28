using System;
using UnityEngine;
// using Cysharp.Threading.Tasks;
// using MessagePipe;
// using VContainer;

public class TimeManager : MonoBehaviour
{
    //[Inject] private ISubscriber<DamageResult> _subscriber;
    [SerializeField] private HitStopDefinition definition;

    private void Awake()
    {
        //_subscriber?.Subscribe(HandleDamageEvent);
    }

    private void HandleDamageEvent(DamageResult e)
    {
        if (definition == null) return;
        var hit = e.DamageInfo.EffectData.HitStop;
        var info = hit.Type == HitStopType.Custom ? hit.Info : definition.GetHitStopInfo(hit.Type);

        Time.timeScale = info.TimeScale;
        //Stop(info.Duration).Forget();
        //Debug.Log($"{info.Duration}, {info.TimeScale}");
    }

    // private async UniTaskVoid Stop(float duration)
    // {
    //     await UniTask.Delay(TimeSpan.FromSeconds(duration), DelayType.UnscaledDeltaTime, cancellationToken: this.GetCancellationTokenOnDestroy());
    //     Time.timeScale = 1f;
    // }
}

public struct HitStopEvent
{
    public readonly HitStopType Type;
    public readonly HitStopInfo Info;

    public HitStopEvent(HitStopType type, HitStopInfo info)
    {
        Type = type;
        Info = info;
    }
}
