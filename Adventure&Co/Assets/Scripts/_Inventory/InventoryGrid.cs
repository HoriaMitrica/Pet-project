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
                Debug.Log("This is where i generate slots");
                InventoryUISlot newSlot = Instantiate(slot, transform, false);
                newSlot.FillVariables(i,Inventory);
                EventTrigger eventTrigger = newSlot.button.GetComponent<EventTrigger>();
                if (eventTrigger == null)
                {
                    eventTrigger = newSlot.button.gameObject.AddComponent<EventTrigger>();
                }
                
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((data) => { Inventory.OnSlotClicked(newSlot,(PointerEventData)data); });
                eventTrigger.triggers.Add(entry);
                Slots[i]=newSlot;   
            }
            
        }
    }
}
