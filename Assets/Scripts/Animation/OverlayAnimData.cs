using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "OAD_", menuName = "Data/Animation/Overlay", order = 0)]
public class OverlayAnimData : ScriptableObject
{
    [SerializeField] private AnimationClip clip;
    [SerializeField] private OverlayType overlayType;
    [SerializeField] private List<AnimationEventData> events;
    
    public AnimationClip Clip => clip;
    public OverlayType OverlayType => overlayType;
    public List<AnimationEventData> Events => events;
}
    
#region Data

    public enum OverlayType { Once, Loop, Hold }

    public enum AnimationEventType
    {
        HitScan,
        CanTransition,
        MoveTrigger,
        FinishAnim
    }

    [System.Serializable]
    public struct AnimationEventData
    {
        public AnimationEventType eventType;
        [Range(0f, 1f)] public float startTime;
        [Range(0f, 1f)] public float endTime;
    }
#endregion



