using UnityEngine;

namespace NPC.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        
        private Animator _animator;
        [SerializeField] private Transform[] patrolPoints;
        private int _currentPatrolIndex; 
        private static readonly int HasToWalk = Animator.StringToHash("hasToWalk");
        private float _maxRight;
        private float _maxLeft;
        public bool isPatrolling;
        private bool _isMovingForward = true;
        private float _walkSpeed=0.5f;
        
        private float _enemyX;
        
        void Start()
        {

            _animator = GetComponent<Animator>();
            if (isPatrolling)
            {
                _animator.SetBool(HasToWalk, true);
            }
        }
        
        void Update()
        {
            MoveTowardsPatrolPoint();
        }

        private void MoveTowardsPatrolPoint()
        {
            if (patrolPoints.Length == 0)
                return;
            Vector3 targetPosition = patrolPoints[_currentPatrolIndex].position;
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            if (moveDirection.x < 0)
            {
                Flip("right");
            }
            if (moveDirection.x > 0)
            {
                Flip("left");
            }
                
            transform.Translate(moveDirection * (_walkSpeed * Time.deltaTime));
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (distanceToTarget < 0.1f)
            {
                ChangePatrolPoint();
            }
        }

        private void ChangePatrolPoint()
        {
            if (_isMovingForward)
            {
                _currentPatrolIndex++;

                if (_currentPatrolIndex >= patrolPoints.Length)
                {
                    _currentPatrolIndex = patrolPoints.Length - 1;
                    _isMovingForward = false;
                }
            }
            else
            {
                _currentPatrolIndex--;
                if (_currentPatrolIndex < 0)
                {
                    _currentPatrolIndex = 0;
                    _isMovingForward = true;
                }
            }
        }
        
        private void Flip(string direction)
        {
            if (direction == "right")
            {
                var localScale = transform.localScale;
                localScale.x = 1f;
                transform.localScale = localScale;   
            }
            if (direction == "left")
            {
                var localScale = transform.localScale;
                localScale.x = -1f;
                transform.localScale = localScale;  
            }

        }
    }
}
