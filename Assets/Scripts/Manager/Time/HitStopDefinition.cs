using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HitStop/Definition", order = 0)]
public class HitStopDefinition : ScriptableObject
{
    [SerializeField] private List<HitStopInfoSetting> settings;
    
    private readonly Dictionary<HitStopType, HitStopInfo> hitStopInfos = new Dictionary<HitStopType, HitStopInfo>();

    private void Initialize()
    {
        hitStopInfos.Clear();
        foreach (var s in settings)
        {
            if (s.hitStopType == HitStopType.Custom) continue;
            hitStopInfos.Add(s.hitStopType, s.hitStopInfo);
        }
    }

    public HitStopInfo GetHitStopInfo(HitStopType hitStopType)
    {
        if (hitStopInfos.Count == 0) Initialize();

        return hitStopInfos.TryGetValue(hitStopType, out var info) ? info : new HitStopInfo();
    }
    
    [Serializable]
    private class HitStopInfoSetting
    {
        public HitStopType hitStopType;
        public HitStopInfo hitStopInfo;
    }
}

public enum HitStopType {Small, Normal, Large, Custom }

[Serializable]
public struct HitStopInfo
{
    public float Duration;
    [Range(0f, 1f)] public float TimeScale;
    
    public HitStopInfo(float duration, float timeScale) {
        Duration = duration;
        TimeScale = timeScale;
    }
}
