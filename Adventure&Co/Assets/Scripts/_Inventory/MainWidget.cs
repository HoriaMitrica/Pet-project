using System;
using UnityEngine;

namespace _Inventory
{
    public class MainWidget : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
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
    }
}
