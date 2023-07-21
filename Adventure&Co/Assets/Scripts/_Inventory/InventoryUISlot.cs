using System;
using Structures;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace _Inventory
{
    public class InventoryUISlot : MonoBehaviour
    {
        public int SlotIndex { get; private set; }
        public Inventory Inventory { get; private set; }
        public ItemInfo ItemInfo { get; private set; }
        private int _amount;
        private CanvasGroup _imageVisibility;
        private InventoryGrid _grid;
        [SerializeField] public Button button;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;
        private DraggedItem _draggedItem;
        private float _lastClickTime;
        private float _doubleClickThreshold = 0.2f;

        public void Awake()
        {
            _imageVisibility = image.GetComponent<CanvasGroup>();
        }
        
        public void FillVariables(int slotIndex, Inventory inventory, DraggedItem draggedItem,InventoryGrid grid)
        {
            _grid = grid;
            _draggedItem = draggedItem;
            SlotIndex = slotIndex;
            Inventory = inventory;
            AddClickFunctionality(this, SlotIndex);
            AddDragDropFunctionality(this, SlotIndex);

        }

        public void UpdateSlot()
        {
            if (Inventory.IsSlotEmpty(SlotIndex))
            {
                button.enabled = false;
                _imageVisibility.alpha = 0;
            }
            else
            {
                button.enabled = true;
                ItemInfo = Inventory.GetItemAtIndex(SlotIndex).Item2;
                _amount = Inventory.GetItemAtIndex(SlotIndex).Item3;
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

        private void AddClickFunctionality(InventoryUISlot slot, int index)
        {
            EventTrigger eventTrigger = slot.button.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { Inventory.OnSlotClicked(slot, (PointerEventData)data); });
            eventTrigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { DoubleClicked(index); });
            eventTrigger.triggers.Add(entry);
        }

        private void DoubleClicked(int index)
        {
            float timeSinceLastClick = Time.time - _lastClickTime;
            if (timeSinceLastClick < _doubleClickThreshold)
            {
                Inventory.UseItemAtIndex(index);
            }

            _lastClickTime = Time.time;
        }

        private void AddDragDropFunctionality(InventoryUISlot slot, int index)
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

        private void DragSetup(InventoryUISlot newSlot, int index)
        {
            _draggedItem.UpdateSlot(newSlot.ItemInfo, _amount);
            _draggedItem.gameObject.SetActive(true);
        }

        private void Dragging()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane; // Set the Z position to the camera's near clip plane distance
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            _draggedItem.transform.position = worldPosition;

        }

        private void DragCleanUp()
        {
            _draggedItem.gameObject.SetActive(false);
            DropSetup(this);
        }
        private void DropSetup(InventoryUISlot slot)
        {
            InventoryUISlot targetSlot = GetDropTargetSlot();
            if (targetSlot != null && targetSlot != slot)
            {
                
                ItemInfo tempItemInfo = slot.ItemInfo;
                var fromIndex = slot.SlotIndex;
                var toIndex = targetSlot.SlotIndex;
                if (Inventory.SameClassSlots(fromIndex, toIndex))
                {
                    Inventory.AddToIndex(fromIndex, toIndex);
                }
                else
                {
                    if (Inventory.SwapSlots(fromIndex, toIndex))
                    {
                        Debug.Log("SUCCESS");
                    }
                }
            }
            _draggedItem.gameObject.SetActive(false);
        }
        private InventoryUISlot GetDropTargetSlot()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Collider2D[] hitColliders = Physics2D.OverlapPointAll(worldPosition);
            foreach (Collider2D collider in hitColliders)
            {

                InventoryUISlot targetSlot = collider.GetComponent<InventoryUISlot>();
                if (targetSlot != null)
                {
                    return targetSlot;
                }
            }
            return null;
        }
    }
}
