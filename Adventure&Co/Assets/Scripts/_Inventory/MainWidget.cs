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
         public InventoryGrid InventoryGrid { get; private set; }

         private void Awake()
         {
             InventoryGrid = GetComponentInChildren<InventoryGrid>();
         }

         void Start()
        {
            InventoryGrid.SetInventory(inventory);
            InventoryGrid.GenerateSlots();
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
                 obtainedItem.gameObject.SetActive(true);
                 obtainedItem.ShowObtainedItem();
             }
         }
         public void OnObtainMessageEnd()
         {
             _obtainedItemsQueue.RemoveAt(0);
             if (_obtainedItemsQueue.Count > 0)
             {
                 obtainedItem.UpdateWidget(_obtainedItemsQueue[0].ItemClass,_obtainedItemsQueue[0].Amount);
                 obtainedItem.gameObject.SetActive(true);
                 obtainedItem.ShowObtainedItem();
             }
         }
    }
}
