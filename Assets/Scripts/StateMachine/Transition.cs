/// <summary>
/// 遷移情報の定義
/// </summary> 
public class Transition
{
    public IState To { get; }
    public IPredicate Condition { get; }

    public Transition(IState to, IPredicate condition)
    {
        To = to;
        Condition = condition;
    }
}