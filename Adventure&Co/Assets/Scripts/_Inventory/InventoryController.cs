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
                inventory.SplitStack(0, 32);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                inventory.SwapSlots(1, 2);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                inventory.UseItemAtIndex(0);
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
                inventoryUI.GetComponentInChildren<RemoveFromInventory>().gameObject.SetActive(false);
            }
        }
    }
}
