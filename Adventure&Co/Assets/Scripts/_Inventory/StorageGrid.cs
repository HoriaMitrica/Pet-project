using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Inventory
{
    public class StorageGrid : MonoBehaviour
    {
        [SerializeField] private StorageUISlot slot;
        [SerializeField] private DraggedItem draggedItem;
        [SerializeField] private DetailUI detailWidget;
        [SerializeField] private Storage storage; 
        public StorageUISlot[] Slots { get; private set; }
        private int _lastId;

        public void GenerateSlots(int id)
        {
            if (id != _lastId)
            {
                if(Slots!=null)
                {
                    foreach (var entry in Slots)
                    {
                        Destroy(entry.gameObject);
                    }
                }
                Slots = new StorageUISlot[storage.AmountOfSlots];
                _lastId = id;

                for (int i = 0; i < storage.AmountOfSlots; i++)
                {
                    StorageUISlot newSlot = Instantiate(slot, transform, false);
                    newSlot.FillVariables(i, storage, draggedItem, detailWidget);
                    Slots[i] = newSlot;
                }
            }

        }
    }
}
