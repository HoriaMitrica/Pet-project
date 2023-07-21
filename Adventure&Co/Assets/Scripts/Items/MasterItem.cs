using _Inventory;
using Enums;
using Structures;
using UnityEngine;

namespace Items
{
    public class MasterItem : MonoBehaviour
    {
        // Start is called before the first frame update
        public ItemInfo info;

        public void UseItem(Inventory inventory,int index)
        {
            if (inventory.RemoveItemAtIndex(index, 1))
            {
                Debug.Log($"Item Used {info.Name}");
            }
        }
    }
}
