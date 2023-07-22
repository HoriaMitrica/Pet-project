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
        public Inventory Inventory { get; private set; }
        public InventoryUISlot[] Slots { get; private set; }

        public void SetInventory(Inventory inventory)
        {
            Inventory = inventory;
        }
        public void GenerateSlots()
        {
            Slots = new InventoryUISlot[Inventory.AmountOfSlots];
            for(int i=0;i<Inventory.Slots.Length;i++)
            {
                InventoryUISlot newSlot = Instantiate(slot, transform, false);
                newSlot.FillVariables(i,Inventory,draggedItem,detailWidget,throwWidget);
                Slots[i]=newSlot;
            }   
        }

        
    }
}
