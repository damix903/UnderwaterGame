namespace StateMachine
{
    /// <summary>
    /// 遷移条件の定義
    /// @function Evaluate 遷移条件を評価する
    /// @function Reset 遷移した際に呼び出される。遷移条件の状態をリセットするために使用する。
    /// </summary>
    public interface IPredicate
    {
        bool Evaluate();
        void Reset();
    }
}