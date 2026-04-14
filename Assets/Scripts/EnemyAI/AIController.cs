using System;
using System.Collections;
using MessagePipe;
using Underwater.StateMachine;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour, ICharacterController
{
    private StateMachine _stateMachine;
    public GameObject GameObject => gameObject;

    public Transform Target { get; private set; }

    private IDetectable detectable;
    //private EnemyData _enemyData;
    private EnemyContext _ctx;
    
    public void Initialize(EnemyContext ctx)
    {
        _ctx = ctx;
        StartCoroutine(Wait(Random.Range(0f, 3f)));
    }

    private void Start()
    {
        //StartCoroutine(Wait(Random.Range(0f, 3f)));
    }

    private IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        
        detectable = GetComponent<SightSensor2D>();
        detectable.OnTargetDetected += HandleTargetDetect;
        detectable.OnTargetLost += HandleTargetLost;

        _stateMachine = _ctx.Data.StateBuilder.Build(this, _ctx);
    }

    private void HandleTargetDetect(GameObject obj) => Target = obj.transform;
    private void HandleTargetLost(GameObject obj) => Target = null;

    private void Update()
    {
        _stateMachine?.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine?.FixedUpdate();
    }

    private void OnDestroy()
    {
        if (detectable == null) return;
        detectable.OnTargetDetected -= HandleTargetDetect;
        detectable.OnTargetLost -= HandleTargetLost;
    }
}
