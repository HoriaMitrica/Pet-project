using Items;
using UnityEngine;

namespace Structures
{
    public class InventorySlot 
    {
        public MasterItem ItemClass { get; private set; }
        public int Amount { get; private set; }

        public InventorySlot(MasterItem itemClas, int amount)
        {
            ItemClass = itemClas;
            Amount = amount; 
        }
    }
}
