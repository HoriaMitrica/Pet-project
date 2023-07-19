using System;
using Player;
using UnityEngine;

namespace Items.pickups
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private MasterItem itemClass;
        [SerializeField] private int amount;
        private SpriteRenderer _sprite;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _sprite.sprite = itemClass.Info.Icon;
        }

        private void OnDrawGizmos()
        {
             Gizmos.DrawWireSphere(transform.position,0.1f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject player = other.gameObject;
                PlayerController playerController = player.GetComponent<PlayerController>();
                var inventoryRef = playerController.inventory;
                var addItem = inventoryRef.AddItem(itemClass, amount);
                if (addItem.Item1)
                {
                    if (addItem.Item2 > 0)
                    {
                        amount = addItem.Item2; 
                    }
                    else
                    {
                        Destroy(gameObject);   
                    }
                }

            }
        }
    }
}