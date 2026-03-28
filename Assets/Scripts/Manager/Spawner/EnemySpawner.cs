using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnSpan = 2f;
    [SerializeField] private float range = 5f;
    [SerializeField] private GameObject prefab;
    
    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        var obj = Instantiate(prefab);
        obj.transform.position = transform.position + new Vector3(Random.Range(-range, range), 0f, 0f);
        yield return new WaitForSeconds(spawnSpan);
        StartCoroutine(Spawn());
    }
}
