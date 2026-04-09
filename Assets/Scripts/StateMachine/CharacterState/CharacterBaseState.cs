using UnityEngine;

public abstract class CharacterBaseState : IState
{
    protected readonly IAnimPlayable Anim;
    private readonly AnimationClip Clip;

    public CharacterBaseState(IAnimPlayable anim, AnimationClip clip = null)
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

public abstract class CharacterBaseState<T> : CharacterBaseState where T : class, ICharacterController
{
    protected readonly T owner;


    protected CharacterBaseState(T owner, IAnimPlayable anim, AnimationClip clip = null) : base(anim, clip)
    {
        this.owner = owner;
    }
}