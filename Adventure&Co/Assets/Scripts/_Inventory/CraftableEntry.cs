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
        
        public void OnButtonPressed()
        {
            ToggleInteractable();
            _craftingMenu.OnCraftableClicked(this);
        }

        public void ToggleInteractable()
        {
            selectButton.interactable = !selectButton.interactable;
        }
    }
}
