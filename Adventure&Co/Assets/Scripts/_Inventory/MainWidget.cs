using System;
using System.Collections.Generic;
using Items;
using Structures;
using UnityEngine;

namespace _Inventory
{
    public class MainWidget : MonoBehaviour
    {
        private List<InventorySlot> _obtainedItemsQueue=new List<InventorySlot>();
        [SerializeField] private Inventory inventory;
        [SerializeField] private Storage storage;
        [SerializeField] private ObtainedItem obtainedItem;
        [SerializeField] private InventoryGrid inventoryGrid;
        [SerializeField] private StorageGrid storageGrid;
        [SerializeField] private CraftingMenu craftingMenu;
        private Canvas _obtainedItemCanvas;
        private Canvas _storageUiCanvas;
        void Start()
        {
            inventory.SetGrid(inventoryGrid);
            storage.SetGrid(storageGrid);
            inventoryGrid.GenerateSlots();
            //storageGrid.GenerateSlots();
            craftingMenu.InitializeCraftingMenu(inventory);
            _obtainedItemCanvas = obtainedItem.GetComponent<Canvas>();
        }
        public void HideStorage()
        {
            _storageUiCanvas.enabled = false;
            inventory.isStorageOpen = false;
        }

        public void ShowStorage()
        {
            _storageUiCanvas.enabled = true;
            inventory.isStorageOpen = true;
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
