using Utility;

namespace StateMachine
{
    /// <summary>
    /// 遷移情報の定義
    /// @param to 遷移先の状態
    /// @param condition 遷移条件
    /// </summary> 
    public class Transition : ISortable
    {
        public IState To { get; }
        public IPredicate Condition { get; }
        public int SortPriority { get; }

        public Transition(IState to, IPredicate condition, int sortPriority = 0)
        {
            To = to;
            Condition = condition;
            SortPriority = sortPriority;
        }

    }
}