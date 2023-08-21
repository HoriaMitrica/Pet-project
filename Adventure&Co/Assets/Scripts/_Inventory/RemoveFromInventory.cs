
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
        [SerializeField] private Inventory inventory;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text countText;
        [SerializeField] private Image icon;
        public void UpdateWidget(int index)
        {
            _foundIndex = index;
            _throwCount = 1;
            var itemAtIndex = inventory.GetItemAtIndex(index);
            _maxAmount = itemAtIndex.Amount;
            nameText.text = $"{itemAtIndex.ItemInfo.Name} x{_maxAmount}";
            icon.sprite = itemAtIndex.ItemInfo.Icon;
            countText.text = $"{_throwCount}";
        }

        public void OnCloseButtonPress()
        {
            transform.gameObject.SetActive(false);
        }
        public void OnConfirmButtonPress()
        {
            inventory.RemoveItemAtIndex(_foundIndex, _throwCount);
            transform.gameObject.SetActive(false);
        }
        public void OnPlusButtonPress()
        {
            float timeSinceLastClick = Time.time - _lastClickTime;
            if (timeSinceLastClick < _doubleClickThreshold)
            {
                _throwCount = _maxAmount;
                countText.text = $"{_throwCount}";
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
        }

        // Function to decrement the number
        private void DecrementNumber()
        {
            if (_throwCount >1)
            {
                _throwCount--;
                countText.text = $"{_throwCount}";
            }
            
        }
    }
}
