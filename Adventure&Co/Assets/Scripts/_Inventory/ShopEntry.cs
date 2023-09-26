using System;
using System.Collections;
using System.Collections.Generic;
using _Inventory;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Slider slider;
    [SerializeField] private Image icon;
    [SerializeField] private Button buyButton;
    private Inventory _inventory;
    private ShopWidget _shopWidget;
    private MasterItem _itemClass;
    public void InitializeEntry(ShopWidget shopWidget,MasterItem itemClass,Inventory inventory)
    {
        _inventory = inventory;
        _itemClass = itemClass;
        _shopWidget = shopWidget;
        nameText.text = itemClass.info.Name;
        icon.sprite = itemClass.info.Icon;
        priceText.text = itemClass.info.Price.ToString();
    }
    private void Update()
    {
        if (_inventory.Coins < _itemClass.info.Price * slider.value)
        {
            priceText.color = Color.red;
            buyButton.interactable = false;
        }
        else
        {
            priceText.color = new Color(1f, 0.82f, 0f, 1.0f);
            buyButton.interactable = true;
        }
        amountText.text = slider.value.ToString();
        priceText.text = (_itemClass.info.Price * slider.value).ToString();
    }

    public void OnBuyClicked()
    {
        
        var addItem = _inventory.AddItem(_itemClass,Convert.ToInt32(slider.value));
        if (addItem.Success)
        {
            _inventory.DecreaseMoney(_itemClass.info.Price *(Convert.ToInt32(slider.value)-addItem.Remainder));
        }
    }
}
