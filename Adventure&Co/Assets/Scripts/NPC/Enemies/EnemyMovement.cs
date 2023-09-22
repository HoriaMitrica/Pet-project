using UnityEngine;

namespace NPC.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        private static readonly int HasToWalk = Animator.StringToHash("hasToWalk");
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] public bool isPatrolling;
        [SerializeField] private bool isAggresive;
        private bool _isMovingForward = true;
        private int _currentPatrolIndex;
        private Animator _animator;
        private EnemyStats _stats;
        private float _enemyX;
        
        void Start()
        {
            _stats = GetComponent<EnemyStats>();
            _animator = GetComponent<Animator>();
            _animator.SetBool(HasToWalk, isPatrolling);
        }
        
        void Update()
        {
            if (isAggresive)
            {
                MoveTowardsPlayer();
            }
            else
            {
                if (isPatrolling)
                {
                    MoveTowardsPatrolPoint();
                }
            }
        }

        private void MoveTowardsPlayer()
        {
            if (patrolPoints.Length == 0)
                return;
            Vector3 targetPosition = patrolPoints[_currentPatrolIndex].position;
            var moveDirection = GetMovingDirection(targetPosition);
            transform.Translate(moveDirection * (_stats.WalkSpeed * Time.deltaTime));
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (distanceToTarget < 0.1f)
            {
                ChangePatrolPoint();
            }
        }
        

        private Vector3 GetMovingDirection(Vector3 targetPosition)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            if (moveDirection.x < 0)
            {
                Flip("right");
            }
            if (moveDirection.x > 0)
            {
                Flip("left");
            }

            return moveDirection;
        }
        private void MoveTowardsPatrolPoint()
        {
            if (patrolPoints.Length == 0)
                return;
            Vector3 targetPosition = patrolPoints[_currentPatrolIndex].position;
            var moveDirection = GetMovingDirection(targetPosition);
            transform.Translate(moveDirection * (_stats.WalkSpeed * Time.deltaTime));
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
