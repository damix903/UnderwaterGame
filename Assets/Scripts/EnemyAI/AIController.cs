using System;
using System.Collections;
using Sensor;
using StateMachine;
using UnityEngine;

public class AIController : MonoBehaviour, ICharacterController
{
    private FiniteStateMachine _stateMachine;
    public GameObject GameObject => gameObject;

    public Transform Target { get; private set; }

    private IDetectable detectable;
    //private EnemyData _enemyData;
    private EnemyContext _ctx;

    private void Awake()
    {
        detectable = GetComponent<SightSensor2D>();
    }

    public void Initialize(EnemyContext ctx)
    {
        _ctx = ctx;
        //_stateMachine = _ctx.Data.StateBuilder.Build(this, _ctx);
        _stateMachine = _ctx.Data.BuildStateMachine(this, _ctx);
    }

    private void HandleTargetDetect(GameObject obj) => Target = obj.transform;
    private void HandleTargetLost(GameObject obj) => Target = null;

    private void Update() => _stateMachine?.Update();
    private void FixedUpdate() => _stateMachine?.FixedUpdate();

    private void OnEnable()
    {
        if (detectable == null) return;
        detectable.OnTargetDetected += HandleTargetDetect;
        detectable.OnTargetLost += HandleTargetLost;
    }

    private void OnDisable()
    {
        if (detectable == null) return;
        detectable.OnTargetDetected -= HandleTargetDetect;
        detectable.OnTargetLost -= HandleTargetLost;
        _stateMachine = null;
    }
}
