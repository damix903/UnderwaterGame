using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AD_", menuName = "Data/Animation/Data", order = 0)]
public class AnimData : ScriptableObject
{
    [SerializeField] private AnimationClip idleClip;
    [SerializeField] private AnimationClip moveClip;
    [SerializeField] private AnimationClip fallClip;
    [SerializeField] private AnimationClip attackClip;
    
    public AnimationClip GetAnim(AnimType animType)
    {
        return animType switch
        {
            AnimType.Idle => idleClip,
            AnimType.Move => moveClip,
            AnimType.Fall => fallClip,
            AnimType.Attack => attackClip,
            _ => null
        };
    }
}