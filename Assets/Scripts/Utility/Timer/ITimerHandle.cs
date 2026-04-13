using System;
using UnityEngine;

namespace Underwater.Utility.Timer
{
    public interface ITimerHandle : IDisposable
    {
        public float Progress { get; }
        public TimerState State { get; }
        public void Pause();
        public void Resume();
        public void Cancel();
        ITimerHandle BindTo(GameObject go);
    }

    public enum TimerState {Running, Paused, Completed, Cancelled}
}