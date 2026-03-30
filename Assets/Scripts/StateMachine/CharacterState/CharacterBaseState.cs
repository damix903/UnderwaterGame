using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public abstract class CharacterBaseState : IState
{
    private readonly IAnimPlayable Anim;
    private readonly AnimationClip Clip;

    public CharacterBaseState(IAnimPlayable anim, AnimationClip clip)
    {
        Anim = anim;
        Clip = clip;
    }

    public virtual void OnEnter()
    {
        Debug.Log($"Entering {GetType().Name}");
        Anim?.PlayBaseClip(Clip);
    }
    public virtual void Update(){}
    public virtual void FixedUpdate(){}
    public virtual void OnExit(){}
}