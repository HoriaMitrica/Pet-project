using System;
using System.Collections.Generic;
using Items;
using Structures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Inventory
{
    public class CraftingMenu : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Button craftButton;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private CraftableEntry craftableEntry;
        [SerializeField] private RecipeEntry recipeEntry;
        [SerializeField] private GameObject craftableEntryContainer;
        [SerializeField] private GameObject recipeEntryContainer;
        [SerializeField] private GameObject rightSide;
        private List<CraftableEntry> _craftableEntries = new List<CraftableEntry>();
        public Inventory Inventory { get; private set; }
        private MasterItem _currentItemClass;
        private ItemInfo _currentItemInfo;
        private List<RecipeEntry> _recipeEntries=new List<RecipeEntry>();
        private CraftableEntry _currentCraftable;

        public MasterItem GetItemClass()
        {
            return _currentItemClass;
        }
        public void OnCraftableClicked(CraftableEntry clickedCraftableEntry)
        {
            if (_currentCraftable != null)
            {
                _currentCraftable.ToggleInteractable();
            }
            rightSide.GetComponent<CanvasGroup>().alpha = 1;
            _currentCraftable = clickedCraftableEntry;
            UpdateDetailWindow(_currentCraftable.ItemClass);
        }

        public void InitializeCraftingMenu(Inventory inventory)
        {
            Inventory = inventory;
            GenerateCraftableList();
        }

        private void GenerateCraftableList()
        {
            foreach (var entry in _craftableEntries)
            {
                Destroy(entry.gameObject);
            }
            _craftableEntries.Clear();
            foreach (var craftableItem in Inventory.UnlockedCraftableItems)
            {
                CraftableEntry newEntry = Instantiate(craftableEntry, craftableEntryContainer.transform, false);
                newEntry.InitializeEntry(this,craftableItem);
                _craftableEntries.Add(newEntry);
            }
        }

        public void GenerateRecipeEntries()
        {
            foreach (var entry in _recipeEntries)
            {
                Destroy(entry.gameObject);
            }
            _recipeEntries.Clear();
            foreach (var entry in _currentItemInfo.Recipe)
            {
                RecipeEntry newEntry = Instantiate(recipeEntry, recipeEntryContainer.transform, false);
                newEntry.InitializeEntry(this,entry.ItemClass,entry.Amount);
                newEntry.UpdateEntry();
                _recipeEntries.Add(newEntry);
            }
        }

        public bool CanBeCrafted()
        {
            foreach (var entry in _recipeEntries)
            {
                if (entry.CurrentAmount < entry.RequiredAmount)
                {
                    return false;
                }
            }
            return true;
        }

        public void UpdateDetailWindow(MasterItem newItemClass)
        {
            if (newItemClass == null)
            {
                rightSide.GetComponent<CanvasGroup>().alpha = 0;
            }
            if (!(newItemClass == _currentItemClass))
            {
                _currentItemClass = newItemClass;
                _currentItemInfo = _currentItemClass.info;
                icon.sprite = _currentItemInfo.Icon;
                nameText.text = _currentItemInfo.Name;
                descriptionText.text = _currentItemInfo.Description;
                GenerateRecipeEntries();
            }
            else
            {
                foreach (var entry in _recipeEntries)
                {
                    entry.UpdateEntry();
                }
            }
            craftButton.interactable = CanBeCrafted();
        }

        public void OnCraftButtonClicked()
        {
            craftButton.interactable = false;
            var result = Inventory.AddItem(_currentItemClass, 1);
            if (!result.Success)
            {
                craftButton.interactable = CanBeCrafted();
            }
            else
            {
                foreach (var itemToRemove in _currentItemInfo.Recipe)
                {
                    Debug.Log("REMOVING "+itemToRemove.ItemClass.name +itemToRemove.Amount);
                    Inventory.RemoveItem(itemToRemove.ItemClass, itemToRemove.Amount);
                }
                craftButton.interactable = CanBeCrafted();
            }
        }

        public void OnCloseButtonPress()
        {
            GetComponent<Canvas>().enabled = false;
        }
    }
}
