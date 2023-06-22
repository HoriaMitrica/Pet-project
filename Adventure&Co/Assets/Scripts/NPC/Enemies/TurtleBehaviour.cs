using UnityEngine;

namespace NPC.Enemies
{
    public class TurtleBehaviour : MonoBehaviour
    {
        private readonly int _maxHealth = 100;
        private Animator _animator;
        private int _currentHealth;
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

        private void Hurt()
        {
            _animator.SetBool("isHurt",true);
        }
    }
}
