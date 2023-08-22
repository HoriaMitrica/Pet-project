using System;
using System.Collections.Generic;
using Items;
using Player;
using Static;
using TMPro;
using UnityEngine;

namespace NPC.Shopkeeper
{
    public class Shopkeeper : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private string npcName;
        [SerializeField] private GameObject nameUI;
        [SerializeField] private ShopWidget shopReference;
        [SerializeField] private List<MasterItem> items;
        private int _id;
        private Canvas _nameCanvas;
        private SpriteRenderer _sprite;
        private PlayerController _playerController;
        private void Start()
        {
            _sprite = GetComponent<SpriteRenderer>();
            nameText.text = npcName;
            _nameCanvas=nameUI.GetComponent<Canvas>();
            _id = UniqueIDGenerator.GetUniqueID();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_playerController != null)
                {
                    OnInteract();
                }
            }
        }

        private void OnInteract()
        {
            shopReference.GenerateEntries(items,_id);
            shopReference.GetComponent<Canvas>().enabled = true;
            _nameCanvas.enabled = false;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Hello");
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
                shopReference.GetComponent<Canvas>().enabled = false;
                shopReference.ToggleShopOpen();
            }
        }
    }
}
