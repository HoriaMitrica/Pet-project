using UnityEngine;

namespace _Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private Inventory inventory;
        [SerializeField] private RemoveFromInventory throwWidget;
        [SerializeField] private ActionMenu actionMenu;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                CloseInventoryButton();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                inventory.AddToIndex(0, 1);
            }
        }

        public void CloseInventoryButton()
        {
            if (inventoryUI.gameObject.activeInHierarchy)
            {
                inventoryUI.gameObject.SetActive(false);
                throwWidget.gameObject.SetActive(false);
                actionMenu.gameObject.SetActive(false);
            }
            else
            {   
                inventoryUI.gameObject.SetActive(true);
            }
        }
    }
}
