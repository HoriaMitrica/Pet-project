using System.Collections;
using System.Collections.Generic;
using _Inventory;
using Items;
using UnityEngine;

public class ShopWidget : MonoBehaviour
{
    [SerializeField] private ShopEntry shopEntry;
    [SerializeField] private GameObject shopEntryContainer;
    [SerializeField] private Inventory inventory;
    public bool IsShopOpen { get; private set; }
    private List<ShopEntry> _shopEntries=new List<ShopEntry>();
    private int _lastID = -1;
    public void GenerateEntries(List<MasterItem> itemClasses,int id)
    {
        if(_lastID!=id)
        {
            _lastID = id;
            foreach (var entry in _shopEntries)
            {
                Destroy(entry.gameObject);
            }
            _shopEntries.Clear();
            foreach (var item in itemClasses)
            {
                ShopEntry newEntry = Instantiate(shopEntry, shopEntryContainer.transform, false);
                newEntry.InitializeEntry(this,item,inventory);
                _shopEntries.Add(shopEntry);
            } 
        }
        OpenShop();
    }
    public void OnCloseButtonPress()
    {
        GetComponent<Canvas>().enabled = false;
        CloseShop();
    }
    public void OpenShop()
    {
        IsShopOpen = true;
    }
    public void CloseShop()
    {
        IsShopOpen = false;
    }
}
