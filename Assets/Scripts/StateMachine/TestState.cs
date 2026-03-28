using UnityEngine;

public abstract class TestState : IState
{
    public virtual void OnEnter()
    {
        Debug.Log($"OnEnter : {GetType()}");
    }

    public virtual void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {
        
    }

    public virtual void OnExit()
    {
        Debug.Log($"OnExit : {GetType()}");
    }
}

public class TestIdle : TestState
{
}

public class TestMove : TestState
{
}

public class TestJump : TestState
{
}