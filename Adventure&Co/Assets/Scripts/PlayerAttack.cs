using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator _animator;

    private float _attackBasic;
    private float _attackStrong;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _attackBasic = Input.GetAxisRaw("AttackBasic");
        _attackStrong = Input.GetAxisRaw("AttackStrong");
    }

    private void FixedUpdate()
    {
        if (_attackBasic != 0)
        {
            _animator.SetBool("isAttackingBasic", true);
        }
        else
        {
            _animator.SetBool("isAttackingBasic", false);
        }
        if (_attackStrong != 0)
        {
            _animator.SetBool("isAttackingStrong",true);
        }
        else
        {
            _animator.SetBool("isAttackingStrong",false);
        }
    }
}
