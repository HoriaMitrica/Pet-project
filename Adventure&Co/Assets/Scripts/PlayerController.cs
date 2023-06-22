using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private float _horizontal;
    private float _vertical;
    private bool _isfacingRight = true;
    private float _jumpForce = 4f;
    private float _walkSpeed = 1f;
    private float _runSpeed = 2f;
    
    // Start is called before the first frame update
    void Start()    
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        Flip();

    }

     private void FixedUpdate()
     {
         if (_horizontal == 0)
         {
             _animator.SetBool("isWalking",false);
             _animator.SetBool("isRunning",false);
         }
         else
             if (Input.GetKey(KeyCode.LeftShift))
             {
                 _animator.SetBool("isAttackingStrong",false);
                 _animator.SetBool("isRunning",true);
                 _animator.SetBool("isWalking",true);
                 _rigidbody.velocity = new Vector2(_horizontal*_runSpeed, _rigidbody.velocity.y);
             }
             else
             {
                 _animator.SetBool("isAttackingStrong",false);
                 _animator.SetBool("isRunning",false);
                 _animator.SetBool("isWalking",true);
                 _rigidbody.velocity = new Vector2(_horizontal*_walkSpeed, _rigidbody.velocity.y);
             }


         if (_animator.GetBool("isJumping") == false)
         {
             _rigidbody.AddForce(new Vector2(0,_vertical*_jumpForce),ForceMode2D.Impulse);
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
         Debug.Log(collision.gameObject);
         if (collision.gameObject.CompareTag("Ground"))
         {
             _animator.SetBool("isJumping",false);
         }
     }

     private void OnTriggerExit2D(Collider2D collision)
     {
         if (collision.gameObject.CompareTag("Ground"))
         {
             _animator.SetBool("isJumping", true);
         }
     }
}
