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
    public class InventoryUISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public int SlotIndex { get; private set; }
        private Color _slotColor;   
        public Inventory Inventory { get; private set; }
        public ItemInfo ItemInfo { get; private set; }
        private int _amount;
        private CanvasGroup _imageVisibility;
        private InventoryGrid _grid;
        [SerializeField] public Button button;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;
        private DraggedItem _draggedItem;
        private DetailUI _detailWidget;
        private RemoveFromInventory _throwWidget;
        private float _lastClickTime;
        private float _doubleClickThreshold = 0.2f;

        public void Awake()
        {
            _imageVisibility = image.GetComponent<CanvasGroup>();
        }
        
        public void FillVariables(int slotIndex, Inventory inventory, DraggedItem draggedItem,DetailUI detailWidget,RemoveFromInventory throwWidget)
        {
            _throwWidget = throwWidget;
            _draggedItem = draggedItem;
            _detailWidget = detailWidget;
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
            Debug.Log("DRAG START");
            _draggedItem.UpdateSlot(newSlot.ItemInfo, _amount);
            _draggedItem.gameObject.SetActive(true);
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
            Debug.Log("DRAG END");
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

            if (targetSlot == null)
            {
                if (ItemInfo.Category != ItemCategory.QuestItem)
                {
                    if (Inventory.GetItemAtIndex(SlotIndex).Amount > 1)
                    {
                        _throwWidget.gameObject.SetActive(true);
                        _throwWidget.UpdateWidget(SlotIndex);
                    }
                    else
                    {
                        Inventory.RemoveItemAtIndex(SlotIndex, 1);
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
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!Input.GetMouseButton(0))
            {
                if (button.isActiveAndEnabled )
                {
                    var worldPosition=GetWorldPosition();   
                    RectTransform canvasRectTransform = _detailWidget.GetComponent<RectTransform>();
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
