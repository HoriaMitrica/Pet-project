using Structures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Inventory
{
    public class DetailUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text categoryText;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text descriptionText;
        public void UpdateInfo(ItemInfo info,Color textColor, int amount)
        {
            nameText.color = textColor;
            categoryText.color = textColor;
            descriptionText.color = textColor;
            if (amount > 1)
            {
                nameText.text = $"{info.Name} x{amount}";
            }
            else
            {
                nameText.text = info.Name;
            }
            categoryText.text = info.Category.ToString();
            icon.sprite = info.Icon;
            descriptionText.text = info.Description;
        }
    }
}
