using Enums;
using Structures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Inventory
{
    public class ActionMenu : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private RemoveFromInventory throwUI;
        [SerializeField] private ShopWidget shopReference;
        [SerializeField] private Button useButton;
        [SerializeField] private Button splitButton;
        [SerializeField] private Button dropButton;
        [SerializeField] private TMP_Text dropText;
        [SerializeField] private Button cancelButton;
        [SerializeField] private TMP_Text useText;
        private Canvas _canvas;
        private int _index;
        private int _amount;
        private ItemInfo _info;

        void Start()
        {
            _canvas = GetComponent<Canvas>();
        }
        public void UpdateMenu(int newIndex)
        {
            _index = newIndex;
            var itemAtIndex = inventory.GetItemAtIndex(_index);
            _info = itemAtIndex.ItemInfo;
            _amount = itemAtIndex.Amount;
            if (_info.CanBeUsed)
            {
                useText.text = _info.UseText;
                useButton.transform.gameObject.SetActive(true);
            }
            else
            {
                useButton.transform.gameObject.SetActive(false);
            }

            if (_info.Category == ItemCategory.QuestItem)
            {
                dropButton.transform.gameObject.SetActive(false);
            }
            else
            {
                if (shopReference.IsShopOpen)
                {
                    dropText.text = "Sell";
                }
                else
                {
                    dropText.text = "Throw";
                }
                dropButton.transform.gameObject.SetActive(true);
            }

            if (_info.CanStack && _amount > 1)
            {
                splitButton.transform.gameObject.SetActive(true);
            }
            else
            {
                splitButton.transform.gameObject.SetActive(false);
            }
        }

        public void OnUseButtonPress()
        {
            inventory.UseItemAtIndex(_index);
            _canvas.enabled = false;
        }
        public void OnSplitButtonPress()
        {
            inventory.SplitStack(_index, _amount / 2);
            _canvas.enabled = false;
        }
        public void OnThrowButtonPress()
        {
            if (inventory.GetItemAtIndex(_index).Amount > 1)
            {
                throwUI.gameObject.SetActive(true);
                throwUI.UpdateWidget(_index);
            }
            else
            {
                inventory.RemoveItemAtIndex(_index, 1);
            }
            _canvas.enabled = false;
        }
        public void OnCancelButtonPress()
        {
            _canvas.enabled = false;
        }
    }
    }

