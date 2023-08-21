using System;
using System.Collections.Generic;
using Items;
using Structures;
using UnityEngine;

namespace _Inventory
{
    public class MainWidget : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private ObtainedItem obtainedItem;
        private List<InventorySlot> _obtainedItemsQueue=new List<InventorySlot>();
        [SerializeField] private InventoryGrid inventoryGrid;
        [SerializeField] private CraftingMenu craftingMenu;
        private Canvas _obtainedItemCanvas;
        void Start()
        {
            
            inventoryGrid.SetInventory(inventory);
            inventory.SetGrid(inventoryGrid);
            inventoryGrid.GenerateSlots();
            craftingMenu.InitializeCraftingMenu(inventory);
            _obtainedItemCanvas = obtainedItem.GetComponent<Canvas>();
        }

         public void AddItemsToQueue(MasterItem itemClass,int amount)
         {

             if (_obtainedItemsQueue.Count > 0)
             {
                 _obtainedItemsQueue.Add(new InventorySlot(itemClass, amount));
             }
             else
             {
                 _obtainedItemsQueue.Add(new InventorySlot(itemClass, amount));
                 obtainedItem.UpdateWidget(itemClass,amount);
                 _obtainedItemCanvas.enabled=true;
                 obtainedItem.ShowObtainedItem();
             }
         }
         public void OnObtainMessageEnd()
         {
             _obtainedItemsQueue.RemoveAt(0);
             if (_obtainedItemsQueue.Count > 0)
             {
                 obtainedItem.UpdateWidget(_obtainedItemsQueue[0].ItemClass,_obtainedItemsQueue[0].Amount);
                 _obtainedItemCanvas.enabled=true;
                 obtainedItem.ShowObtainedItem();
             }
         }
    }
}
