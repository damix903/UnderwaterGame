using System;
using UnityEngine;

namespace Sensor
{
    public class SightSensor2D : MonoBehaviour, IDetectable
    {
        [SerializeField] private float viewRadius = 5f;
        [SerializeField, Range(0, 360)] private float viewAngle = 90f;
        [SerializeField] private float durationToLostTarget = 2f;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private float scanRate = 1f;

        private GameObject _currentTarget;

        public event Action<GameObject> OnTargetDetected;
        public event Action<GameObject> OnTargetLost;

        private bool _isTargetInSight;
        private float _lostTimer;
        private float _timer;

        private void Update()
        {
            HandleTargetLost();
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                ScanTarget();
                _timer = scanRate;
            }
        }

        private void ScanTarget()
        {
            Collider2D[] targetColliders = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetLayer);
            GameObject bestTarget = null;
            float bestDist = 0f;

            foreach (var col in targetColliders)
            {
                if (col.gameObject == gameObject) continue;
            
                Transform target = col.transform;
                Vector2 dirToTarget = (target.position - transform.position).normalized;
            
                // 視野角チェック
                if (Vector2.Angle(transform.right, dirToTarget) > viewAngle / 2) continue;
            
                float distToTarget = Vector2.Distance(transform.position, target.position);
                
                // 障害物チェック
                if (Physics2D.Raycast(transform.position, dirToTarget, distToTarget, obstacleLayer)) continue;

                // 距離が一番近いターゲットを選択する
                if (distToTarget < bestDist || bestDist == 0f)
                {
                    bestTarget = col.gameObject;
                    bestDist = distToTarget;
                }
            }

            HandleUpdateTarget(bestTarget);
        }

        private void HandleUpdateTarget(GameObject newTarget)
        {
            if (newTarget == _currentTarget)
            {
                _isTargetInSight = true;
                _lostTimer = durationToLostTarget;
                return;
            }

            if (newTarget != null)
            {
                _currentTarget = newTarget;
                _isTargetInSight = true;
                OnTargetDetected?.Invoke(newTarget);
            }
            else
            {
                // 一回だけ実行したい
                if (_isTargetInSight)
                {
                    _isTargetInSight = false;
                    _lostTimer = durationToLostTarget;
                }
            }
        }

        private void HandleTargetLost()
        {
            if (_currentTarget == null || _isTargetInSight) return;

            // ターゲットが視界から外れてもある程度は覚えておく
            _lostTimer -= Time.deltaTime;

            if (_lostTimer < 0f)
            {
                OnTargetLost?.Invoke(_currentTarget);
                _currentTarget = null;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            float halfAngle = viewAngle / 2f;
        
            var upRayRotation = Quaternion.AngleAxis(-halfAngle, Vector3.forward);
            var downRayRotation = Quaternion.AngleAxis(halfAngle, Vector3.forward);

            Vector3 upRayDir = upRayRotation * transform.right * viewRadius;
            Vector3 downRayDir = downRayRotation * transform.right * viewRadius;

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, upRayDir);
            Gizmos.DrawRay(transform.position, downRayDir);
            Gizmos.DrawLine(transform.position + upRayDir, transform.position + downRayDir);
        }
#endif
    }
}
