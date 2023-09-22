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
        [SerializeField] private ShopWidget shopReference;
        [SerializeField] private UiHandle uiHandle;
        [SerializeField] private List<MasterItem> items;
        private bool _hasUi;
        private int _id;
        private PlayerController _playerController;
        private void Start()
        {
            _id = UniqueIDGenerator.GetUniqueID();
            if (uiHandle != null)
            {
                _hasUi = true;
            }
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
            if (_hasUi) 
            {
                uiHandle.CanvasDisable();
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Hello");
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
                shopReference.GetComponent<Canvas>().enabled = false;
                shopReference.CloseShop();
            }
        }
    }
}
