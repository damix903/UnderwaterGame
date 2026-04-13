namespace Underwater.StateMachine
{
    /// <summary>
    /// 遷移情報の定義
    /// @param to 遷移先の状態
    /// @param condition 遷移条件
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
}