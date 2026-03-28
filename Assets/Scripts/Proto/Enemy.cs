using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class Enemy : MonoBehaviour
{
    private CharacterMovement _movement;
    
    [SerializeField]  private float speed;
    private Vector2 _dir;
    private bool chasePlayer;
    
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private float damage;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
        chasePlayer = Random.value > .5f;
        if (chasePlayer) player = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None)[0].gameObject;
        
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        _dir = chasePlayer ? Vector2.zero : new Vector2(x, y);
    }

    private void Update()
    {
        if (chasePlayer) _dir = (player.transform.position - transform.position).normalized;
        var pos = transform.position;
        transform.position = (Vector2)pos + _dir * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((detectionLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            _dir *= -1f;

            if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                if (damageable.TeamID != TeamID)
                    damageable.TakeDamage(new DamageInfo(gameObject, damage, new EffectData()));
            }
        }
    }

    public bool TakeDamage(DamageInfo info)
    {
        Destroy(gameObject);
        return true;
    }

    public TeamID TeamID => TeamID.Enemy;
    public bool IsAlive  => true;
    public DefenseState DefenseState { get; set; }
}
