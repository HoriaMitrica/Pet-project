using System;
using _Inventory;
using Player;
using TMPro;
using UnityEngine;

namespace Items.pickups
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private MasterItem itemClass;
        [SerializeField] private int amount;
        [SerializeField] private UiHandle uiHandle;
        private bool _hasUi;
        private SpriteRenderer _sprite;
        private PlayerController _playerController;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _sprite.sprite = itemClass.info.Icon;
            if (uiHandle != null)
            {
                _hasUi = true;
            }
        }

        void Update()
        {
            if (_playerController != null && Input.GetKeyDown(KeyCode.E))
            {
                var inventoryRef = _playerController.inventory;
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

        private void OnDrawGizmos()
        {
             Gizmos.DrawWireSphere(transform.position,0.1f);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameObject player = other.gameObject;
                _playerController = player.GetComponent<PlayerController>();
                if (_hasUi)
                {
                    uiHandle.CanvasEnable();
                }
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerController = null;
                if (_hasUi)
                {
                    uiHandle.CanvasDisable();
                }
            }
        }
        
        
    }
}
