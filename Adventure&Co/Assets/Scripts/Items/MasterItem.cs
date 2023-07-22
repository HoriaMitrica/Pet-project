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
        private Color GetRarityColor(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Common:
                    return Color.white;
                case ItemRarity.Uncommon:
                    return new Color(0.62f, 0.86f, 0.5f, 1.0f);
                case ItemRarity.Rare:
                    return new Color(102/255f, 204/255f, 255/255f, 1.0f);
                case ItemRarity.Epic:
                    return new Color(0.75f, 0.0f, 1.0f, 1.0f); 
                case ItemRarity.Legendary:
                    return new Color(1.0f, 0.65f, 0.0f, 1.0f);
                case ItemRarity.Mythic:
                    return new Color(199 / 255.0f, 0, 0, 1.0f);; 
                case ItemRarity.Ascended:
                    return new Color(1.0f, 0.08f, 0.57f, 1.0f); 
                default:
                    return Color.white; 
            }
        }
        public void Start()
        {
            Debug.Log("THIS IS MASTER ITEM");
        }

        public void UseItem(Inventory inventory,int index)
        {
            if (inventory.RemoveItemAtIndex(index, 1))
            {
                Debug.Log($"Item Used {info.Name}");
            }
        }
    }
}
