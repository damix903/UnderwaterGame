using System;
using UnityEngine;
using VContainer;

namespace Sensor
{
    public class DamageSensor : MonoBehaviour, IDetectable
    {
        public event Action<GameObject> OnTargetDetected;
        public event Action<GameObject> OnTargetLost;

        private IDamageable _damageable;

        private void Awake()
        {
            _damageable = GetComponent<IDamageable>();
        }

        private void HandleDamage(DamageResult result)
        {
            var attacker = result.DamageInfo.Attacker;
            if (attacker != null) OnTargetDetected?.Invoke(attacker);
        }

        private void OnEnable()
        {
            if (_damageable != null) _damageable.OnDamaged += HandleDamage;
        }

        private void OnDisable()
        {
            if (_damageable != null) _damageable.OnDamaged -= HandleDamage;
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;
            if (_damageable == null) _damageable = GetComponent<IDamageable>();
            if (_damageable == null) Debug.LogWarning("DamageSensor requires a component that implements IDamageable.", this);
        }
    }
}