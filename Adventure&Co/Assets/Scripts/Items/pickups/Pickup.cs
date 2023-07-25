using System;
using _Inventory;
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
            _sprite.sprite = itemClass.info.Icon;
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
                if (addItem.Success)
                {
                    if (addItem.Remainder > 0)
                    {
                        amount = addItem.Remainder; 
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
