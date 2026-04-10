using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BAD_", menuName = "Data/Animation/Base", order = 0)]
public class BaseAnimData : ScriptableObject
{
    //[SerializeField] private BaseAnimation[] baseAnimations;
    [SerializeField] private AnimationClip idleClip;
    [SerializeField] private AnimationClip moveClip;
    [SerializeField] private AnimationClip fallClip;
    [SerializeField] private AnimationClip attackClip;
    
    public AnimationClip IdleClip => idleClip;
    public AnimationClip MoveClip => moveClip;
    public AnimationClip FallClip => fallClip;
    public AnimationClip AttackClip => attackClip;
    
    // private Dictionary<BaseAnimationType, AnimationClip> _baseAnimsCache;
    //
    // public Dictionary<BaseAnimationType, AnimationClip> GetBaseAnims()
    // {
    //     if (_baseAnimsCache == null)
    //     {
    //         _baseAnimsCache = new Dictionary<BaseAnimationType, AnimationClip>();
    //         foreach (var b in baseAnimations)
    //         {
    //             if (b != null) _baseAnimsCache.Add(b.animType, b.animClip);
    //         }
    //     }
    //     
    //     return _baseAnimsCache;
    // }
}

// public enum BaseAnimationType
// {
//     Idle,
//     Walk,
//     Run,
//     Jump,
//     Fall,
//     AirJump
// }
//
// [System.Serializable]
// public class BaseAnimation
// {
//     public BaseAnimationType animType;
//     public AnimationClip animClip;
// }