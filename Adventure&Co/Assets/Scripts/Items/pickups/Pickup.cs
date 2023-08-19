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
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private GameObject nameUI;
        private Canvas _nameCanvas;
        private SpriteRenderer _sprite;
        private PlayerController _playerController;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _sprite.sprite = itemClass.info.Icon;
            nameText.text = $"{itemClass.info.Name} x{amount}";
            _nameCanvas=nameUI.GetComponent<Canvas>();
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
                _nameCanvas.enabled = true;
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerController = null;
                _nameCanvas.enabled = false;
            }
        }
        
        
    }
}
