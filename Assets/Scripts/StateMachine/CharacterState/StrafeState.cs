using EnemyAI.Move;

namespace StateMachine.CharacterState
{
    public class StrafeState : CharacterBaseState<ICharacterController>
    {
        protected override AnimType AnimType => AnimType.Move;
        private readonly IMoveable _moveable;

        public StrafeState(ICharacterController owner, IAnimPlayable anim, IMoveable moveable) : base(owner, anim)
        {
            _moveable = moveable;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _moveable?.Start();
        }

        public override void FixedUpdate()
        {
            if (owner == null || owner.Target == null) return;
        
            _moveable?.MoveTick(owner.Target.position);
        }

        public override void OnExit()
        {
            base.OnExit();
            _moveable?.Stop();
        }
    }
}