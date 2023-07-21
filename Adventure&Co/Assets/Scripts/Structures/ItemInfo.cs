using Enums;
using UnityEngine;

namespace Structures
{
    [CreateAssetMenu(fileName = "New ItemInfo", menuName = "Inventory/ItemInfo")]
    public class ItemInfo : ScriptableObject
    {
        // Start is called before the first frame update
        [Header("Item Information")]
        public string Name;
        public string Description;
        public Sprite Icon;
        public bool CanBeUsed;
        public string UseText;
        public bool CanStack;
        public ItemCategories Category;

    }
}
