using UnityEngine;

namespace _Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private Inventory inventory;

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
                
            }
            else
            {
                inventoryUI.gameObject.SetActive(true);
                //inventoryUI.GetComponentInChildren<RemoveFromInventory>().gameObject.SetActive(false);
            }
        }
    }
}
