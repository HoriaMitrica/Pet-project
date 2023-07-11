
using System;
using Interfaces;
using Player;
using UnityEngine;

namespace NPC.Enemies
{

public class EnemyCombat : MonoBehaviour, ICombat
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private  float attackCooldown = 1f;
    [SerializeField] private float range;
    private float _cooldownTimer = Mathf.Infinity;
    private readonly float _hurtTimer = 0.5f;
    private LayerMask _playerLayer;
    private Animator _animator;
    private PlayerCombat _playerCombat;
    private EnemyMovement _movement;
    private EnemyStats _stats;
    private HealthbarBehaviour _healthbar;
    // Start is called before the first frame update
    void Start()
    {
        _movement = GetComponent<EnemyMovement>();
        _stats = GetComponent<EnemyStats>();
        _animator = GetComponent<Animator>();
        _playerLayer=LayerMask.GetMask("Player");
        _healthbar = GetComponentInChildren<HealthbarBehaviour>();
        _healthbar.SetHealth(_stats.CurrentHealth,_stats.MaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        _cooldownTimer += Time.deltaTime;
        if (PlayerInSight())
        {
            
            if (_cooldownTimer >= attackCooldown)
            {
                _cooldownTimer = 0;
                _animator.SetTrigger(Attack);
            }
        }
    }
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_boxCollider.bounds.center+transform.right * (range * transform.localScale.x),
            _boxCollider.bounds.size,
            0, Vector2.left, 0, _playerLayer);
        if (hit.collider != null)
        { 
            _playerCombat=hit.transform.GetComponent<PlayerCombat>();
        }
        else
        {
            _playerCombat = null;
        }
        return hit.collider != null;
    }
    public void TakeDamage(int damageTaken)
    {
        _cooldownTimer -= _hurtTimer;
        _stats.DecreaseHealth(damageTaken);
        _healthbar.SetHealth(_stats.CurrentHealth,_stats.MaxHealth);
        if (_stats.CurrentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        _movement.isPatrolling = false;
        _animator.SetBool(IsDead,true);
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.color=Color.blue;
        Gizmos.DrawWireCube(_boxCollider.bounds.center+transform.right*range*transform.localScale.x,
            _boxCollider.bounds.size);
    }*/

    public void DealDamage()
    {
        if (_playerCombat == null)
            return;
        if (PlayerInSight())
        {
            _playerCombat.TakeDamage(_stats.Damage);
        }
    }
    
}   
}
