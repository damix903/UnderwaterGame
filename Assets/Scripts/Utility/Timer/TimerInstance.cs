using System;
using UnityEngine;

namespace Underwater.Utility.Timer
{
    public class TimerInstance : ITimerHandle
    {
        private readonly float _duration;
        private readonly Action _callback;
        private readonly bool _isLooping;
        
        private float _elapsedTime;
        private bool _hasBinding;
        private GameObject _bindingGo;
    
        public bool UseRealTime { get; private set; }
        public float Progress => Mathf.Clamp01(_elapsedTime / _duration);
        public TimerState State { get; private set; } = TimerState.Running;
        
        public void Pause() => State = TimerState.Paused;
        public void Resume() => State = TimerState.Running;
        public void Cancel() => State = TimerState.Cancelled;
    
        public ITimerHandle BindTo(GameObject go)
        {
            _hasBinding = go != null;
            _bindingGo = go;
            return this;
        }
    
        public TimerInstance(float duration, Action callback, bool isLooping, bool useRealTime = false)
        {
            _duration = duration;
            _callback = callback;
            _isLooping = isLooping;
            UseRealTime = useRealTime;
        }

        public void Tick(float deltaTime)
        {
            if (State != TimerState.Running) return;

            if (CheckBindingGo()) return;
        
            _elapsedTime += deltaTime;

            if (_elapsedTime >= _duration)
            {
                _callback?.Invoke();

                if (_isLooping) _elapsedTime -= _duration;
                else State = TimerState.Completed;
            }
        }

        private bool CheckBindingGo()
        {
            if (_hasBinding && _bindingGo == null)
            {
                Cancel();
                return true;
            }

            return false;
        }

        private bool disposed;
    
        public void Dispose()
        {
            if (disposed) return;

            State = TimerState.Cancelled;
            GC.SuppressFinalize(this);
        
            disposed = true;
        }
    }
}