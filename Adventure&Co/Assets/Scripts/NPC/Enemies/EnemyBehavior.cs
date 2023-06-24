using UnityEngine;

namespace NPC.Enemies
{
    public class EnemyBehavior : MonoBehaviour
    {
        private readonly int _maxHealth = 100;
        private Animator _animator;
        private int _currentHealth;
        [SerializeField] private Transform _rightLimit;
        [SerializeField] private Transform _leftLimit;
        private float _hurtTime = 0.5f;
        private static readonly int IsDead = Animator.StringToHash("isDead");
        private static readonly int IsHurt = Animator.StringToHash("isHurt");
        private bool _isfacingRight;

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

        private void Attack()
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
        private void Flip()
        {
            if (_isfacingRight)
            {
                _isfacingRight = !_isfacingRight;
                var localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
        private void Patrol()
        {
            
        }
    }
}
