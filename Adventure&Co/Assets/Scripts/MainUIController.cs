using System.Collections;
using System.Collections.Generic;
using _Inventory;
using UnityEngine;

public class MainUIController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private Inventory inventory;
    [SerializeField] private RemoveFromInventory throwWidget;
    [SerializeField] private ActionMenu actionMenu;
    [SerializeField] private CraftingMenu craftingMenu;
    private Canvas _inventoryCanvas;
    private Canvas _throwCanvas;
    private Canvas _actionMenuCanvas;
    private Canvas _craftingMenuCanvas;
    

    void Start()
    {
        _inventoryCanvas = inventoryUI.GetComponent<Canvas>();
        _throwCanvas=throwWidget.GetComponent<Canvas>();
        _actionMenuCanvas=actionMenu.GetComponent<Canvas>();
        _craftingMenuCanvas=craftingMenu.GetComponent<Canvas>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleCraftingMenu();
        } 
    }

    public void ToggleInventory()
    {
        if (_inventoryCanvas.enabled)
        {
            
            _inventoryCanvas.enabled = false;
            _throwCanvas.enabled = false;
            _actionMenuCanvas.enabled = false;
        }
        else
        {
            _inventoryCanvas.enabled = true;
        }
    }
    public void ToggleCraftingMenu()
    {
        _craftingMenuCanvas.enabled = !_craftingMenuCanvas.enabled;
    }
}
