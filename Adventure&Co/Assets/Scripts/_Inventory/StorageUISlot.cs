using System;
using Enums;
using Structures;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace _Inventory
{
    public class StorageUISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public int SlotIndex { get; private set; }
        private Color _slotColor;
        public Storage Storage { get; private set; }
        public ItemInfo ItemInfo { get; private set; }
        private int _amount;
        private CanvasGroup _imageVisibility;
        [SerializeField] public Button button;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;
        private DraggedItem _draggedItem;
        private DetailUI _detailWidget;
        private float _lastClickTime;
        private float _doubleClickThreshold = 0.2f;

        public void Awake()
        {
            _imageVisibility = image.GetComponent<CanvasGroup>();
        }
        
        public void FillVariables(int slotIndex, Storage storage,DraggedItem draggedItem,DetailUI detailWidget)
        {
            _draggedItem = draggedItem;
            _detailWidget = detailWidget;
            SlotIndex = slotIndex;
            Storage = storage;
            AddClickFunctionality(this, SlotIndex);
            AddDragDropFunctionality(this, SlotIndex);
        }

        public void UpdateSlot()
        {
            if (Storage.IsSlotEmpty(SlotIndex))
            {
                button.enabled = false;
                ItemInfo = null;
                _imageVisibility.alpha = 0;
            }
            else
            {
                button.enabled = true;
                var ItemAtIndex = Storage.GetItemAtIndex(SlotIndex);
                ItemInfo = ItemAtIndex.ItemInfo;
                _amount = ItemAtIndex.Amount;
                _slotColor = ItemInfo.RarityColor;
                image.sprite = ItemInfo.Icon;
                _imageVisibility.alpha = 1;
                if (ItemInfo.CanStack)
                {
                    text.text = "x" + _amount;
                }
                else
                {
                    text.text = String.Empty;
                }
            }
        }
        private void AddClickFunctionality(StorageUISlot slot, int index)
        {
            EventTrigger eventTrigger = slot.button.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry(); 
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { DoubleClicked(index); });
            eventTrigger.triggers.Add(entry);
        }
        private void DoubleClicked(int index)
        {
            float timeSinceLastClick = Time.time - _lastClickTime;
            if (timeSinceLastClick < _doubleClickThreshold)
            {
                Debug.Log("Storage Item Double Clicked");
                var inventorySlot=Storage.PlayerInventory.SearchEmptySlot();
                if (inventorySlot.Success)
                {
                    Storage.MoveFromStorageToInventoryIndex(index, inventorySlot.Index);
                }
            }
            _lastClickTime = Time.time;
        }

        private void AddDragDropFunctionality(StorageUISlot slot, int index)
        {
            EventTrigger eventTrigger = slot.button.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.BeginDrag;
            entry.callback.AddListener((data) => { DragSetup(slot, index); });
            eventTrigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { Dragging(); });
            eventTrigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.EndDrag;
            entry.callback.AddListener((data) => { DragCleanUp(); });
            eventTrigger.triggers.Add(entry);
        }

        private void DragSetup(StorageUISlot newSlot, int index)
        {
            if (ItemInfo != null)
            {
                _draggedItem.UpdateSlot(newSlot.ItemInfo, _amount);
                _draggedItem.gameObject.SetActive(true);
            }
        }

        private void Dragging()
        {
            _draggedItem.transform.position = GetWorldPosition();
        }

        private static Vector3 GetWorldPosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            return worldPosition;
        }

        private void DragCleanUp()
        {
            if (ItemInfo != null)
            {
                _draggedItem.gameObject.SetActive(false);
                DropSetup(this);
            }
        }
        private void DropSetup(StorageUISlot slot)
        {
            var slots = GetDropTargetSlot();
            var targetStorageSlot = slots.storageSlot;
            var targetInventorySlot = slots.inventorySlot;
            if (targetStorageSlot != null && targetStorageSlot != slot)
            {
                var fromIndex = slot.SlotIndex;
                var toIndex = targetStorageSlot.SlotIndex;
                if (Storage.SameClassSlots(fromIndex, toIndex))
                {
                    Storage.AddToIndex(fromIndex, toIndex);
                }
                else
                {
                    if (Storage.SwapSlots(fromIndex, toIndex))
                    {
                        Debug.Log("SUCCESS");
                    }
                }
            }

            if (targetInventorySlot != null)
            {
                Storage.MoveFromStorageToInventoryIndex(slot.SlotIndex, targetInventorySlot.SlotIndex);
            }

            _draggedItem.gameObject.SetActive(false);
        }
        private (StorageUISlot storageSlot,InventoryUISlot inventorySlot) GetDropTargetSlot()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Collider2D[] hitColliders = Physics2D.OverlapPointAll(worldPosition);
            StorageUISlot targetStorageSlot=null;
            InventoryUISlot targetInventorySlot=null;
            foreach (Collider2D collider in hitColliders)
            {
                targetInventorySlot=collider.GetComponent<InventoryUISlot>();
                targetStorageSlot= collider.GetComponent<StorageUISlot>();
            }
            return (storageSlot:targetStorageSlot,inventorySlot:targetInventorySlot);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!Input.GetMouseButton(0))
            {
                if (button.isActiveAndEnabled )
                {
                    var worldPosition=GetWorldPosition();   
                    _detailWidget.transform.position = worldPosition;
                    _detailWidget.gameObject.SetActive(true);
                    _detailWidget.UpdateInfo(ItemInfo,_slotColor,_amount);
                }
            }
            else
            {
                button.image.color=new Color(0.7f,0.3f,0f,0.7f);
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            button.image.color = new Color(0.45f, 0.45f, 0.45f, 0.7f);
            _detailWidget.gameObject.SetActive(false);
        }
    }
}
