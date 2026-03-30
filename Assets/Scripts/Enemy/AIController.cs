using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    private StateMachine _stateMachine;
    
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private float damage;
    [SerializeField] private BaseAnimData animData;
    [SerializeField] private BaseEnemyStateBuilder stateBuilder;
    public GameObject Target { get; private set; }
    private IDetectable detectable;

    private void Start()
    {
        StartCoroutine(Wait(Random.Range(0f, 3f)));
    }

    private IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        
        detectable = GetComponent<SightSensor2D>();
        detectable.OnTargetDetected += HandleTargetDetect;
        detectable.OnTargetLost += HandleTargetLost;

        _stateMachine = stateBuilder.Build(this);
    }

    private void HandleTargetDetect(GameObject obj) => Target = obj;
    private void HandleTargetLost() => Target = null;

    private void Update()
    {
        _stateMachine?.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine?.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((detectionLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                if (damageable.TeamID == TeamID.Player)
                    damageable.TakeDamage(new DamageInfo(gameObject, damage, new EffectData()));
            }
        }
    }

    private void OnDestroy()
    {
        detectable.OnTargetDetected -= HandleTargetDetect;
        detectable.OnTargetLost -= HandleTargetLost;
    }
}
