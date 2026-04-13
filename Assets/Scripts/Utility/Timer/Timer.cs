using System;

namespace Underwater.Utility.Timer
{
    public static class Timer
    {
        private static ITimerService _service;
        
        public static void Initialize(ITimerService service)
        {
            if (_service != null) return;
            _service = service;
        }
        
        public static ITimerHandle Set(float duration, Action callback, bool isLooping = false, bool useRealTime = false)
        {
            if (_service == null)
            {
                throw new InvalidOperationException("Timer service is not initialized.");
            }
            
            return _service.SetTimer(duration, callback, isLooping, useRealTime);
        }
    }
}