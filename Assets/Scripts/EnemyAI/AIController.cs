using System;
using System.Collections;
using System.Collections.Generic;
using Sensor;
using StateMachine;
using UnityEngine;

public class AIController : MonoBehaviour, ICharacterController
{
    private FiniteStateMachine _stateMachine;

    public GameObject GameObject => gameObject;
    public Transform Target { get; private set; }
    public Transform Transform => transform;

    private IDetectable[] detectables;
    //private EnemyData _enemyData;
    private EnemyContext _ctx;

    private void Awake()
    {
        detectables = GetComponents<IDetectable>();
    }

    public void Initialize(EnemyContext ctx)
    {
        _ctx = ctx;
        _stateMachine = _ctx.Data.BuildStateMachine(this, _ctx);
    }

    private void Update() => _stateMachine?.Update();
    private void FixedUpdate() => _stateMachine?.FixedUpdate();

    private void HandleTargetDetect(GameObject obj) => Target = obj.transform;
    private void HandleTargetLost(GameObject obj) => Target = null;

    private void OnEnable()
    {
        foreach (var d in detectables)
        {
            if (d == null) continue;

            d.OnTargetDetected += HandleTargetDetect;
            d.OnTargetLost += HandleTargetLost;
        } 
    }

    private void OnDisable()
    {
        foreach (var d in detectables)
        {
            if (d == null) continue;

            d.OnTargetDetected -= HandleTargetDetect;
            d.OnTargetLost -= HandleTargetLost;
        }

        _stateMachine = null;
        Target = null;
    }
}
