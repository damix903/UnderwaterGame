using System;
using UnityEngine;

namespace Sensor
{
    [RequireComponent(typeof(Collider2D))]
    public class ColliderDetector : MonoBehaviour, IDetectable
    {
        [SerializeField] private LayerMask targetLayer;
        
        public event Action<GameObject> OnTargetDetected;
        public event Action<GameObject> OnTargetLost;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & targetLayer) != 0)
            {
                OnTargetDetected?.Invoke(other.gameObject);
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & targetLayer) != 0)
            {
                OnTargetLost?.Invoke(other.gameObject);
            }
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;
            var col = GetComponent<Collider2D>();
            if (col != null) col.isTrigger = true;
        }
    }
}