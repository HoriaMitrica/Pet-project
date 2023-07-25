using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Inventory
{
    public class RecipeEntry : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text amountText;
        private MasterItem _itemClass;
        public int RequiredAmount { get; private set; }
        public int CurrentAmount{ get; private set; }
        private CraftingMenu _craftingMenu;

        public void InitializeEntry(CraftingMenu craftingMenu,MasterItem itemClass,int amount)
        {
            RequiredAmount = amount;
            _craftingMenu = craftingMenu;
            _itemClass = itemClass;
        }
        public void UpdateEntry()
        {
            icon.sprite = _itemClass.info.Icon;
            var amountOfItems = _craftingMenu.Inventory.GetTotalAmountOfItems(_itemClass);
            CurrentAmount = amountOfItems.TotalAmount;
            if (RequiredAmount > CurrentAmount)
            {
                nameText.color = Color.red;
                amountText.color = Color.red;
            }
            else
            {
                nameText.color = Color.green;
                amountText.color = Color.green;
            }
            nameText.text = _itemClass.info.Name;
            amountText.text = $"{CurrentAmount} / {RequiredAmount}";
        }
        
    }
}
