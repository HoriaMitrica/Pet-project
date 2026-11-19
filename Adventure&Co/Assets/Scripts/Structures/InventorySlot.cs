using Items;

namespace Structures
{
    [System.Serializable]
    public class InventorySlot
    {
        public MasterItem ItemClass;
        public int Amount;

        public InventorySlot(MasterItem itemClass, int amount)
        {
            ItemClass = itemClass;
            Amount = amount;
        }
    }
}
