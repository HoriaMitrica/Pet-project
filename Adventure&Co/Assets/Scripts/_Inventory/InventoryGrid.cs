using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Inventory
{
    public class InventoryGrid : MonoBehaviour
    {
         
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private InventoryUISlot slot;
        [SerializeField] private DraggedItem draggedItem;
        [SerializeField] private DetailUI detailWidget;
        [SerializeField] private RemoveFromInventory throwWidget;
        [SerializeField] private ActionMenu actionMenu;
        [SerializeField] private Inventory inventory;
        public InventoryUISlot[] Slots { get; private set; }
        
        public void GenerateSlots()
        {
            Slots = new InventoryUISlot[inventory.AmountOfSlots];
            for(int i=0;i<inventory.Slots.Length;i++)
            {
                InventoryUISlot newSlot = Instantiate(slot, transform, false);
                newSlot.FillVariables(i,inventory,draggedItem,detailWidget,throwWidget,actionMenu);
                Slots[i]=newSlot;
            }   
        }

        
    }
}
