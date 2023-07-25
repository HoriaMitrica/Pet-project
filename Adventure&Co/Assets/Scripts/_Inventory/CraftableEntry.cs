using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Inventory
{
    public class CraftableEntry : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Button selectButton;
        public MasterItem ItemClass { get; private set; }
        private CraftingMenu _craftingMenu;

        public void InitializeEntry(CraftingMenu craftingMenu,MasterItem itemClass)
        {
            ItemClass = itemClass;
            _craftingMenu = craftingMenu;
            nameText.text = itemClass.info.Name;
        }

        public void DisableButton()
        {
            selectButton.enabled = false;
        }
        public void EnableButton()
        {
            selectButton.enabled = true;
        }
        public void OnButtonPressed()
        {
            _craftingMenu.OnCraftableClicked(this);
        }
    }
}
