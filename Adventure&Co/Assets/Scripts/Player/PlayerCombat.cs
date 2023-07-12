
using Interfaces;
using NPC.Enemies;
using UnityEngine;

namespace Player
{

    public class PlayerCombat : MonoBehaviour, ICombat
    {
        [SerializeField] private Transform attackPoint;
        private float _attackRange = 0.1f;
        private PlayerStats _stats;
        private LayerMask _enemyLayers;
        private static readonly int IsDead = Animator.StringToHash("isDead");
        
        private Animator _animator;

        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();

            _enemyLayers = LayerMask.GetMask("Enemies");
            _stats = GetComponent<PlayerStats>();
        }

        // Update is called once per frame
        public void DealDamage()
        {
            Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, _attackRange, _enemyLayers);
            foreach (var enemy in enemiesHit)
            {
                enemy.GetComponent<EnemyCombat>().TakeDamage(_stats.Damage);
            }
        }

        public void TakeDamage(int damageTaken)
        {
            _stats.DecreaseHealth(damageTaken);
            if (_stats.CurrentHealth <= 0)
            {
                _animator.SetBool(IsDead, true);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
                return;
            Gizmos.DrawWireSphere(attackPoint.position, _attackRange);
        }
    }
}
