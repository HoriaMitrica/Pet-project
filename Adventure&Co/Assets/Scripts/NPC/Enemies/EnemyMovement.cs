using Player;
using UnityEngine;

namespace NPC.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        private static readonly int HasToWalk = Animator.StringToHash("hasToWalk");
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private bool isPatrolling;
        [SerializeField] private bool isAggresive;
        [SerializeField] private Vector3 offsetDistance;
        private bool _isMovingForward = true;
        private bool _isPlayerInSight;
        private int _currentPatrolIndex;
        private Vector3 _playerPosition;
        private GameObject _playerReference;
        private Animator _animator;
        private EnemyStats _stats;
        private float _enemyX;
        
        void Start()
        {
            _stats = GetComponent<EnemyStats>();
            _animator = GetComponent<Animator>();
            
        }
        
        void Update()
        {
            if (isAggresive && _playerReference)
            {
                GetPlayerPosition();
                MoveTowardsPlayer();
            }
            else
            {
                if (isPatrolling)
                {
                    _animator.SetBool(HasToWalk, true);
                    MoveTowardsPatrolPoint();
                }
                else
                {
                    _animator.SetBool(HasToWalk, false);
                }
            }
        }

        private void MoveTowardsPlayer()
        {
            Vector3 targetPosition =_playerPosition + offsetDistance;
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (distanceToTarget > 0.15f)
            {
                var moveDirection = GetMovingDirection(targetPosition);
                transform.Translate(moveDirection * (_stats.WalkSpeed * Time.deltaTime));
                _animator.SetBool(HasToWalk, true);
            }
            else
            {
                _animator.SetBool(HasToWalk, false);
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
            _animator.SetBool(HasToWalk, true);
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

        private void GetPlayerPosition()
        {
            if (_playerReference != null)
            {
                _playerPosition=_playerReference.transform.position; 
            }
            _animator.SetBool(HasToWalk, false);
        }

        public void StopMovement()
        {
            isPatrolling = false;
            isAggresive = false;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            
            if (other.CompareTag("Player"))
            {
                _playerReference = other.gameObject;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerReference = null;
            }
        }
    }
}
