using Structures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Inventory
{
    public class DraggedItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text amountText;
        private ItemInfo _info;
        private int _amount;

        public void UpdateSlot(ItemInfo info, int amount)
        {
            _info = info;
            _amount = amount;
            icon.sprite = _info.Icon;
            if (_info.CanStack)
            {
                amountText.text = $"x{_amount}";
            }
            else
            {
                amountText.text = "";
            }
        }
    }
}
