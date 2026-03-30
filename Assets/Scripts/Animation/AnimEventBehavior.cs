using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimEventBehavior : PlayableBehaviour
{
    private OverlayAnimData _data;
    private IAnimEventListenable _listener;

    private List<AnimationEventData> _activeEvents = new();
    private List<AnimationEventData> _endedEvents = new();

    private List<AnimEventHandler> _eventHandlers = new();
    private double _previousTime;

    public void Initialize(OverlayAnimData data, IAnimEventListenable listener)
    {
        ClearEventData();
        _data = data;
        _listener = listener;
        _previousTime = 0;
        _eventHandlers.Clear();
        foreach (var ev in _data.Events)
        {
            _eventHandlers.Add(new AnimEventHandler(ev));
        }
    }

    public override void OnGraphStop(Playable playable)
    {
        ClearEventData();
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if (_data == null || _listener == null) return;

        double currentTime = playable.GetTime();
        double duration = playable.GetDuration();
        
        if (duration <= 0) return;
        float normalizedTime = (float)currentTime / (float)duration;
        if (currentTime < _previousTime) _activeEvents.Clear();

        for (int i = _eventHandlers.Count - 1; i >= 0; i--)
        {
            var ev = _eventHandlers[i];
            if (normalizedTime >= ev.Data.startTime && !ev.IsActive)
            {
                ev.IsActive = true;
                _listener?.OnAnimEvent(ev.Data.eventType, true);
            }
            else if (normalizedTime >= ev.Data.endTime && ev.IsActive)
            {
                _listener?.OnAnimEvent(ev.Data.eventType, false);
                _eventHandlers.RemoveAt(i);
            }
        }
        
        _previousTime = currentTime;
        if (normalizedTime > 1f) ClearEventData();
    }
    
    private void ClearEventData()
    {
        _data = null;
        _listener = null;
        _activeEvents.Clear();
        _endedEvents.Clear();
        _eventHandlers.Clear();
    }

    private class AnimEventHandler
    {
        public AnimationEventData Data;
        public bool IsActive;

        public AnimEventHandler(AnimationEventData data)
        {
            Data = data;
            IsActive = false;
        }
    }
}