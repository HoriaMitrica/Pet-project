using UnityEngine;

namespace Abstract
{
    public abstract class Stats : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int damage = 10;
        [SerializeField] private float walkSpeed = 1f;

        public int MaxHealth => maxHealth;
        public int CurrentHealth { get; private set; }
        public int Damage => damage;
        public float WalkSpeed => walkSpeed;

        protected virtual void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public void DecreaseHealth(int amount)
        {
            CurrentHealth -= amount;
        }
    }
}
