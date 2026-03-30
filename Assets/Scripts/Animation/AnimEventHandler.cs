using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventHandler : IAnimEventListenable
{
    private Dictionary<AnimationEventType, List<Action<bool>>> _register = new();

    public void Register(AnimationEventType type, Action<bool> action)
    {
        if (_register.TryGetValue(type, out var actions))
        {
            actions.Add(action);
            return;
        }
        
        _register.Add(type, new List<Action<bool>> {action});
    }

    public void Remove(AnimationEventType type, Action<bool> action)
    {
        if (_register.TryGetValue(type, out var actions))
        {
            actions.Remove(action);
        }
    }
    
    public void OnAnimEvent(AnimationEventType type, bool isActive)
    {
        if (_register.TryGetValue(type, out var actions))
        {
            foreach (var a in actions)
            {
                a.Invoke(isActive);
            }
        }
    }
}