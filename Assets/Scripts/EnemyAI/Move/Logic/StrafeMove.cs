using Movement;
using UnityEngine;

namespace EnemyAI.Move
{
    public class StrafeMove : BaseMove
    {
        private readonly Transform _owner;
        private readonly StrafeMoveData _data;
        
        private Vector2 _strafeDir = Vector2.zero;
        private float _directionChangeTimer = 0f;
    
        public StrafeMove(CharacterMovement movement, Transform owner, StrafeMoveData data) : base(movement)
        {
            _owner = owner;
            _data = data;
        }

        public override void Start(Vector2 position = default)
        {
            base.Start(position);
            _directionChangeTimer = 0f;
        }
        
        /// <summary>
        /// ターゲットの周りをストレイフ移動するロジック。一定の距離を保ちながら、定期的に左右どちらかの方向に移動方向を変更する。
        /// </summary>
        /// <param name="position"></param>
        public override void MoveTick(Vector2 position = default)
        {
            if (_owner == null) return;
        
            var dirToTarget = (position - (Vector2)_owner.position).normalized;
            HandleStrafeDirectionChange(dirToTarget);
            
            var distance = Vector2.Distance(position, _owner.position);
            
            // ターゲットとの距離を保つための移動入力を計算
            var maintainDistanceInput = Vector2.zero;
            if (distance < _data.DistanceToMaintain - _data.DistanceThreshold) maintainDistanceInput = -dirToTarget;
            else if (distance > _data.DistanceToMaintain + _data.DistanceThreshold) maintainDistanceInput = dirToTarget;
            
            var finalInput = _strafeDir + maintainDistanceInput;
            movement?.SetMovementInput(finalInput.normalized);
        }
        
        private void HandleStrafeDirectionChange(Vector2 dirToTarget)
        {
            _directionChangeTimer -= Time.deltaTime;
            if (!(_directionChangeTimer <= 0f)) return;
            
            _directionChangeTimer = _data.DirectionChangeInterval;
            _strafeDir = new Vector2(-dirToTarget.y, dirToTarget.x) * (Random.value < 0.5f ? 1 : -1); // ランダムに左右どちらかの方向に変更
        }
    }
}