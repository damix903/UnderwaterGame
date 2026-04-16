using System;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class AnimEventReceiver : MonoBehaviour, IAnimEventListenable
    {
        private readonly Dictionary<AnimationEventType, Action<bool>> _registerDict = new();

        public void Register(AnimationEventType type, Action<bool> action)
        {
            if (!_registerDict.TryAdd(type, action))
                _registerDict[type] += action;
        }

        public void Remove(AnimationEventType type, Action<bool> action)
        {
            if (!_registerDict.ContainsKey(type)) return;
            
            _registerDict[type] -= action;
            if (_registerDict[type] == null) _registerDict.Remove(type);
        }
    
        public void OnAnimEvent(AnimationEventType type, bool isActive)
        {
            if (_registerDict.TryGetValue(type, out var actions))
                actions?.Invoke(isActive);
        }
        
        public void SendStartEvent(AnimationEventType type) => OnAnimEvent(type, true);
        
        public void SendEndEvent(AnimationEventType type) => OnAnimEvent(type, false);
    }
}