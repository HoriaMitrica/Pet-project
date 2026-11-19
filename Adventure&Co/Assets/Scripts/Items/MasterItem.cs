using _Inventory;
using Structures;
using UnityEngine;

namespace Items
{
    [System.Serializable]
    public class MasterItem : MonoBehaviour
    {
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
