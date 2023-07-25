
using _Inventory;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
{ 
    private PlayerStats _stats;
    private Rigidbody2D _rigidbody;
    [SerializeField] public Inventory inventory;
    private Animator _animator;
    private float _horizontal;
    private float _attackspeed=1;
    private bool _isfacingRight = true;
    private bool _isJumpPressed;
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int AttackingStrong = Animator.StringToHash("AttackingStrong");
    private static readonly int AttackingBasic = Animator.StringToHash("AttackingBasic");
    private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _stats = GetComponent<PlayerStats>();
    }

    void Update()
    {


        _horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _animator.SetTrigger(AttackingBasic);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            _animator.SetTrigger(AttackingStrong);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _attackspeed+=0.1f;
            _animator.SetFloat(AttackSpeed,_attackspeed);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            _attackspeed-=0.1f;
            _animator.SetFloat(AttackSpeed,_attackspeed);
        }
        
        Flip();
    }

     private void FixedUpdate()
     {
         if (_horizontal == 0)
         {
             _animator.SetBool(IsWalking,false);
             _animator.SetBool(IsRunning,false);
         }
         else
             if (Input.GetKey(KeyCode.LeftShift))
             {
                 _animator.SetBool(IsRunning,true);
                 _animator.SetBool(IsWalking,true);
                 _rigidbody.velocity = new Vector2(_horizontal*_stats.RunSpeed, _rigidbody.velocity.y);
             }
             else
             {
                 _animator.SetBool(IsRunning,false);
                 _animator.SetBool(IsWalking,true);
                 _rigidbody.velocity = new Vector2(_horizontal*_stats.WalkSpeed, _rigidbody.velocity.y);
             }
         if (_animator.GetBool(IsJumping) == false &&_isJumpPressed)
         {
             _rigidbody.AddForce(new Vector2(0,_stats.JumpForce),ForceMode2D.Impulse);
         }
     }
     private void Flip()
     {
         if (_isfacingRight && _horizontal < 0 || !_isfacingRight && _horizontal > 0)
         {
             _isfacingRight = !_isfacingRight;
             var localScale = transform.localScale;
             localScale.x *= -1f;
             transform.localScale = localScale;
         }
     }
    
     private void OnTriggerEnter2D(Collider2D collision)
     {
         if (collision.gameObject.CompareTag("Ground"))
         {
             _isJumpPressed = false;
             _animator.SetBool(IsJumping,false);
         }
     }

     private void OnTriggerExit2D(Collider2D collision)
     {         if (collision.gameObject.CompareTag("Ground"))
         {
             _animator.SetBool(IsJumping, true);
         }
     }
}
}
