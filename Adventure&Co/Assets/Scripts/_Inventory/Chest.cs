using System.Collections;
using System.Collections.Generic;
using _Inventory;
using Items;
using Player;
using Static;
using Structures;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int amountOfSlots;
    [SerializeField] private GameObject nameUI;
    [SerializeField] private string objectName;
    [SerializeField] private GameObject storageUiReference;
    [SerializeField] private Storage storage;
    [SerializeField] private StorageGrid storageGrid;
    [SerializeField] private List<InventorySlot> chestItems=new List<InventorySlot>();
    private InventorySlot[] _slots;
    public int Id { get; private set; }
    private PlayerController _playerController;
    private Canvas _nameCanvas;
    private Canvas _storageCanvas;
    void Start()
    {
        Id = UniqueIDGenerator.GetUniqueID();
        _nameCanvas=nameUI.GetComponent<Canvas>();
        _storageCanvas = storageUiReference.GetComponent<Canvas>();
        _slots=new InventorySlot[amountOfSlots];
        for (int i = 0; i < chestItems.Count; i++)
        {
            _slots[i] = chestItems[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            _storageCanvas.enabled = !_storageCanvas.enabled;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_playerController != null)
            {
                OnInteract();
            }
        }
    }
    public void RemoveItem(int index,InventorySlot itemToRemove, int amount)
    {
        if (itemToRemove.Amount - amount < 1)
        {
            _slots[index] = null;
        }
        else
        {
            _slots[index]=new InventorySlot(_slots[index].ItemClass, _slots[index].Amount - amount);
        }
    }
    public void AddItem(int index,InventorySlot itemToAdd)
    {
        if (itemToAdd!=null)
        {
            _slots[index] = new InventorySlot(itemToAdd.ItemClass, itemToAdd.Amount);
        }
        else
        {
            _slots[index] = null;
        }
        
    }
    private void OnInteract()
    {
        storage.SetActiveChest(Id);
        storage.ChangeAmountOfSlots(amountOfSlots);
        storage.CreateSlots(_slots);
        storageGrid.GenerateSlots(Id);
        
        for (int i = 0; i < amountOfSlots; i++)
        {
            if(_slots[i]!=null)
                storage.AddItemAtIndexInternal(i,_slots[i].ItemClass, _slots[i].Amount);
        }
        _storageCanvas.enabled = !_storageCanvas.enabled;
        _nameCanvas.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hello");
            GameObject player = other.gameObject;
            _playerController = player.GetComponent<PlayerController>();
            _nameCanvas.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerController = null;
            _nameCanvas.enabled = false;
            _storageCanvas.enabled = false;
        }
    }
}
