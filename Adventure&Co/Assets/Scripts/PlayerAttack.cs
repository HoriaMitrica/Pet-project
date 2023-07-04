using System;
using System.Collections;
using System.Collections.Generic;
using NPC.Enemies;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float _horizontal;

    [SerializeField] private Transform attackPoint;
     private float _attackRange=0.1f;
     private int _damage=10;
     private LayerMask _enemyLayers;
    // Start is called before the first frame update
    void Start()
    {
        _enemyLayers=LayerMask.GetMask("Enemies");
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void DealDamage()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, _attackRange, _enemyLayers);
        foreach (var enemy in enemiesHit)
        {
            enemy.GetComponent<EnemyCombat>().TakeDamage(_damage);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position,_attackRange);
    }
}
