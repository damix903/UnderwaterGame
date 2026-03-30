using System;

public interface IAnimEventListenable
{
    void OnAnimEvent(AnimationEventType type, bool isActive);
    public void Register(AnimationEventType type, Action<bool> action);
    public void Remove(AnimationEventType type, Action<bool> action);
}