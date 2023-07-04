using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerAttack _combat;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private float _horizontal;
    private float _vertical;
    private bool _isfacingRight = true;
    private float _attacDelay = 1f;
    private float _jumpForce = 4f;
    private bool _isJumpPressed;
    private bool _isAttackingPressed;
    private float _walkSpeed = 1f;
    private float _runSpeed = 2f;
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsAttackingStrong = Animator.StringToHash("isAttackingStrong");
    private static readonly int IsAttackingBasic = Animator.StringToHash("isAttackingBasic");

    // Start is called before the first frame update
    void Start()
    {
        _combat = GetComponent<PlayerAttack>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJumpPressed = true;
        }
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _isAttackingPressed = true;
        }
        Flip();

    }

     private void FixedUpdate()
     {
         if (_isAttackingPressed)
         {
             _isAttackingPressed = false;
             if (!_animator.GetBool(IsAttackingBasic))
             {
                 _animator.SetBool(IsAttackingBasic, true);

                 Invoke(nameof(StopAttacking), _attacDelay);
             }
         }
         /*if (_attackStrong!=0 && _horizontal==0)
         {
             _animator.SetBool(IsAttackingStrong,true);
         }
         else
         {
             _animator.SetBool(IsAttackingStrong,false);
         }*/
         ////////
         if (_horizontal == 0)
         {
             _animator.SetBool(IsWalking,false);
             _animator.SetBool(IsRunning,false);
         }
         else
             if (Input.GetKey(KeyCode.LeftShift))
             {
                 _animator.SetBool(IsAttackingStrong,false);
                 _animator.SetBool(IsAttackingBasic, false);
                 _animator.SetBool(IsRunning,true);
                 _animator.SetBool(IsWalking,true);
                 _rigidbody.velocity = new Vector2(_horizontal*_runSpeed, _rigidbody.velocity.y);
             }
             else
             {
                 _animator.SetBool(IsAttackingStrong,false);
                 _animator.SetBool(IsRunning,false);
                 _animator.SetBool(IsWalking,true);
                 _rigidbody.velocity = new Vector2(_horizontal*_walkSpeed, _rigidbody.velocity.y);
             }


         if (_animator.GetBool(IsJumping) == false &&_isJumpPressed)
         {
             _rigidbody.AddForce(new Vector2(0,_jumpForce),ForceMode2D.Impulse);
         }
     }

     private void StopAttacking()
     {
         //_combat.DealDamage();
         _animator.SetBool(IsAttackingBasic, false);
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
     {
         if (collision.gameObject.CompareTag("Ground"))
         {
             _animator.SetBool(IsJumping, true);
         }
     }
}
