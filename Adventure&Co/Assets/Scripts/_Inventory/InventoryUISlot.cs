using System;
using Structures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace _Inventory
{
    public class InventoryUISlot : MonoBehaviour
    {
        public int SlotIndex { get; private set;}
        public Inventory Inventory { get; private set;}
        public ItemInfo ItemInfo { get; private set;}
        private int _amount;
        private CanvasGroup _imageVisibility;
        [SerializeField] public Button button;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;
        public void Awake()
        {
            _imageVisibility=image.GetComponent<CanvasGroup>();
        }
        
        
        public void FillVariables(int slotIndex,Inventory inventory)
        {
            SlotIndex = slotIndex;
            Inventory = inventory;
            
        }
        public void UpdateSlot()
        {
            if(Inventory.IsSlotEmpty(SlotIndex))
            {
                button.enabled = false;
                _imageVisibility.alpha = 0; 
            }
            else
            {
                button.enabled = true;
                ItemInfo = Inventory.GetItemAtIndex(SlotIndex).Item2;
                _amount = Inventory.GetItemAtIndex(SlotIndex).Item3;
                image.sprite = ItemInfo.Icon;
                _imageVisibility.alpha = 1;
                if (ItemInfo.CanStack)
                {
                    text.text = "x" + _amount;
                }
                else
                {
                    text.text = String.Empty;
                }
            }
        }
    }
}
