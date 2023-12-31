
using System;
using Interfaces;
using Player;
using UnityEngine;

namespace NPC.Enemies
{

public class EnemyCombat : MonoBehaviour
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Hurt = Animator.StringToHash("isHurt");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    [SerializeField] private CircleCollider2D circleCollider;
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
        if (PlayerInBiteRange())
        {
            
            if (_cooldownTimer >= attackCooldown)
            {
                _cooldownTimer = 0;
                _animator.SetTrigger(Attack);
            }
        }
    }
    private bool PlayerInBiteRange()
    {
        RaycastHit2D hit = Physics2D.CircleCast(circleCollider.bounds.center,circleCollider.radius,Vector2.left,0,_playerLayer);
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
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // Set the color you want for the visualization

        float radius = circleCollider.radius;
        Vector2 origin = circleCollider.bounds.center + transform.right * (range * transform.localScale.x);

        Gizmos.DrawWireSphere(origin, radius);
    }
    
    public void TakeDamage(int damageTaken)
    {
        _cooldownTimer -= _hurtTimer;
        _stats.DecreaseHealth(damageTaken);
        _animator.SetTrigger(Hurt);
        _healthbar.SetHealth(_stats.CurrentHealth,_stats.MaxHealth);
        if (_stats.CurrentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        _movement.StopMovement();
        _animator.SetBool(IsDead,true);
    }
    public void DealDamage()
    {
        Debug.Log("Attack");
        if (_playerCombat == null)
            return;
        if (PlayerInBiteRange())
        {
            _playerCombat.TakeDamage(_stats.Damage);
        }
    }
    
}   
}
