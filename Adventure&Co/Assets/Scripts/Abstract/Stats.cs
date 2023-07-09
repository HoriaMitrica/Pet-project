
using UnityEngine;

namespace Abstract
{
public abstract class Stats : MonoBehaviour
{
    [SerializeField] private int maxHealth=100;
    [SerializeField] private int damage=10;
    [SerializeField] private float walkSpeed=1f;

    private int _currentHealth;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => _currentHealth;
    public int Damage => damage;
    public float WalkSpeed => walkSpeed;

    void Start()
    {
        _currentHealth = maxHealth;
    }
    public void DecreaseHealth(int amount)
    {
    _currentHealth -= amount;
    }
}
    
}
