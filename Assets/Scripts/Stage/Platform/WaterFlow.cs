using Movement;
using Sensor;
using UnityEngine;

namespace Stage
{
    public class WaterFlow : MonoBehaviour
    {
        [SerializeField] private Vector2 direction;
        [SerializeField] private float strength = 2f;
        
        private Vector2 ConstantForce => direction.normalized * strength;
        private IDetectable _detectable;

        private void Awake()
        {
            _detectable = GetComponent<IDetectable>();
        }

        private void OnEnable()
        {
            if (_detectable == null) return;
            
            _detectable.OnTargetDetected += HandleTargetDetected;
            _detectable.OnTargetLost += HandleTargetLost;
        }

        private void OnDisable()
        {
            if (_detectable == null) return;
            
            _detectable.OnTargetDetected -= HandleTargetDetected;
            _detectable.OnTargetLost -= HandleTargetLost;
        }

        private void HandleTargetDetected(GameObject obj)
        {
            if (obj.TryGetComponent(out IForceApplicable applicable))
            {
                applicable.AddConstantForce(ConstantForce);
            }
        }
        
        private void HandleTargetLost(GameObject obj)
        {
            if (obj.TryGetComponent(out IForceApplicable applicable))
            {
                applicable.RemoveConstantForce(ConstantForce);
            }
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;
            if (!TryGetComponent<IDetectable>(out var detectable))
            {
                Debug.LogWarning("WaterFlow requires a component that implements IDetectable to function properly.", this);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 start = transform.position;
            Vector3 end = start + (Vector3)(ConstantForce);
            Gizmos.DrawLine(start, end);
            Gizmos.DrawSphere(end, 0.1f);
        }
    }
}