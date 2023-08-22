
using Structures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Inventory
{
    public class RemoveFromInventory : MonoBehaviour
    {
        private int _clickCount;
        private int _maxAmount;
        private int _throwCount=1;
        private int _foundIndex;
        private float _lastClickTime; 
        private float _doubleClickThreshold = 0.12f;
        private float _repeatRate = 0.1f;
        private ItemInfo _itemInfo;
        [SerializeField] private Inventory inventory;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private TMP_Text countText;
        [SerializeField] private TMP_Text valueText;
        [SerializeField] private Image icon;
        [SerializeField] private ShopWidget shopReference;

        public void UpdateWidget(int index)
        {
            _foundIndex = index;
            _throwCount = 1;
            var itemAtIndex = inventory.GetItemAtIndex(index);
            _itemInfo = itemAtIndex.ItemInfo;
            _maxAmount = itemAtIndex.Amount;
            nameText.text = $"{_itemInfo.Name} x{_maxAmount}";
            icon.sprite = _itemInfo.Icon;
            countText.text = $"{_throwCount}";
            messageText.text = "How many do you want to throw away?";
            valueText.text = "";
            if (shopReference.IsShopOpen)
            {
                valueText.text = _itemInfo.RetailPrice.ToString();
                messageText.text = "How many do you want to sell?";
            }
        }

        public void OnCloseButtonPress()
        {
            transform.gameObject.SetActive(false);
        }
        public void OnConfirmButtonPress()
        {
            inventory.RemoveItemAtIndex(_foundIndex, _throwCount);
            transform.gameObject.SetActive(false);
            if (shopReference.IsShopOpen)
            {
                inventory.IncreaseMoney(_itemInfo.RetailPrice*_throwCount);
            }
        }
        public void OnPlusButtonPress()
        {
            float timeSinceLastClick = Time.time - _lastClickTime;
            if (timeSinceLastClick < _doubleClickThreshold)
            {
                _throwCount = _maxAmount;
                countText.text = $"{_throwCount}";
                if (shopReference.IsShopOpen)
                {
                    valueText.text = (_itemInfo.RetailPrice*_throwCount).ToString();
                }
            }
            else
            {
                IncrementNumber();
            }
            _lastClickTime = Time.time;
            InvokeRepeating(nameof(IncrementNumber), _repeatRate, _repeatRate);
        }
        public void OnPlusButtonRelease()
        {
            CancelInvoke(nameof(IncrementNumber));
        }
        public void OnMinusButtonRelease()
        {
            CancelInvoke(nameof(DecrementNumber));
        }
        public void OnMinusButtonPress()
        {
            float timeSinceLastClick = Time.time - _lastClickTime;
            if (timeSinceLastClick < _doubleClickThreshold)
            {
                _throwCount = 1;
                countText.text = $"{_throwCount}";
                if (shopReference.IsShopOpen)
                {
                    valueText.text = (_itemInfo.RetailPrice*_throwCount).ToString();
                }
            }
            else
            {
                DecrementNumber();
            }
            _lastClickTime = Time.time;
            InvokeRepeating(nameof(DecrementNumber), _repeatRate, _repeatRate);
        }
        private void IncrementNumber()
        {
            if (_throwCount < _maxAmount)
            {
                _throwCount++;
                countText.text = $"{_throwCount}";
            }
            if (shopReference.IsShopOpen)
            {
                valueText.text = (_itemInfo.RetailPrice*_throwCount).ToString();
            }
        }

        // Function to decrement the number
        private void DecrementNumber()
        {
            if (_throwCount >1)
            {
                _throwCount--;
                
                countText.text = $"{_throwCount}";
            }
            if (shopReference.IsShopOpen)
            {
                valueText.text = (_itemInfo.RetailPrice*_throwCount).ToString();
            }
        }
    }
}
