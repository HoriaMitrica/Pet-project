using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Inventory
{
    public class StorageGrid : MonoBehaviour
    {
         
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private StorageUISlot slot;
        [SerializeField] private DraggedItem draggedItem;
        [SerializeField] private DetailUI detailWidget;
        public Inventory PlayerInventory { get; private set; }
        public Storage Storage { get; private set; }
        public StorageUISlot[] Slots { get; private set; }
        public void SetStorage(Storage storage)
        {
            Storage = storage;
        }

        public void GenerateSlots(Storage storage, int id)
        {
            Slots = new StorageUISlot[Storage.AmountOfSlots];
                for (int i = 0; i < Storage.Slots.Length; i++)
                {
                    StorageUISlot newSlot = Instantiate(slot, transform, false);
                    newSlot.FillVariables(i, Storage, draggedItem, detailWidget);
                    Slots[i] = newSlot;
                }
        }
    }
}
