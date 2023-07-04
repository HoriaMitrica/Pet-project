
using UnityEngine;

namespace NPC.Enemies
{

public class EnemyCombat : MonoBehaviour
{
    
    private static readonly int IsDead = Animator.StringToHash("isDead");
    private static readonly int IsHurt = Animator.StringToHash("isHurt");
    private readonly int _maxHealth = 100;
    private float _hurtTime = 0.5f;
    private int _currentHealth;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _currentHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        _animator.SetBool(IsHurt,true);
        Invoke(nameof(NotHurt), _hurtTime);
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        _animator.SetBool(IsDead,true);
        _animator.SetBool(IsHurt,false);
    }
    
    private void NotHurt()
    {
        _animator.SetBool(IsHurt,false);
    }
}
}
