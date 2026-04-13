using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace Underwater.Utility.Timer
{
    public class TimerManager : ITimerService, ITickable
    {
        private readonly HashSet<TimerInstance> _timers = new();
        private readonly List<TimerInstance> _timersToAdd = new();
    
        public ITimerHandle SetTimer(float duration, Action callback, bool isLoop = false,
            bool useRealTime = false)
        {
            var handle = new TimerInstance(duration, callback, isLoop, useRealTime);
            _timersToAdd.Add(handle);
        
            return handle;
        }

        public void Tick()
        {
            // タイマーのアップデート中に追加されないようにここのタイミングで追加する
            if (_timersToAdd.Count != 0)
            {
                foreach (var t in _timersToAdd) _timers.Add(t);
                _timersToAdd.Clear();
            }

            // 更新して条件に合うものを削除
            _timers.RemoveWhere(t =>
            {
                t.Tick(t.UseRealTime ? Time.unscaledDeltaTime : Time.deltaTime);
                return t.State is TimerState.Completed or TimerState.Cancelled;
            });
        }
    }

    public interface ITimerService
    {
        public ITimerHandle SetTimer(float duration, Action callback, bool isLoop = false,
            bool useRealTime = false);
    }
}