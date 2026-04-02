using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    private StateMachine _stateMachine;

    public GameObject Target { get; private set; }
    private IDetectable detectable;
    private EnemyData _enemyData;

    public void Initialize(EnemyData data)
    {
        _enemyData = data;
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

        _stateMachine = _enemyData.StateBuilder.Build(this, _enemyData.AnimData);
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

    private void OnDestroy()
    {
        if (detectable == null) return;
        detectable.OnTargetDetected -= HandleTargetDetect;
        detectable.OnTargetLost -= HandleTargetLost;
    }
}
