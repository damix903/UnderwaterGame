namespace StateMachine
{
    /// <summary>
    /// 外部からイベントを受け取ることが条件
    /// </summary>
    public class ActionPredicate : IPredicate
    {
        private bool _isTriggered;
    
        public void Trigger() => _isTriggered = true;
    
        public bool Evaluate() => _isTriggered;

        public void Reset() => _isTriggered = false;
    }
}