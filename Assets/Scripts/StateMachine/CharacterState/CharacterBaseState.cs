using StateMachine;
using UnityEngine;

public abstract class CharacterBaseState : IState
{
    protected readonly IAnimPlayable Anim;
    protected abstract AnimType AnimType { get; }

    public CharacterBaseState(IAnimPlayable anim)
    {
        Anim = anim;
    }

    public virtual void OnEnter()
    {
        Debug.Log($"Entering {GetType().Name}");
        Anim?.Play(AnimType);
    }
    public virtual void Update(){}
    public virtual void FixedUpdate(){}
    public virtual void OnExit(){}
}

public abstract class CharacterBaseState<T> : CharacterBaseState where T : class, ICharacterController
{
    protected readonly T owner;


    protected CharacterBaseState(T owner, IAnimPlayable anim) : base(anim)
    {
        this.owner = owner;
    }
}