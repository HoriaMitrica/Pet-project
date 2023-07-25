using System.Collections.Generic;
using Enums;
using Items;
using Structures;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Inventory
{
    public class Inventory : MonoBehaviour
    {
        public int AmountOfSlots { get; private set; }
        private int _maxStackSize = 64;
        public List<MasterItem> UnlockedCraftableItems { get; private set; } = new List<MasterItem>();
        [SerializeField] private int startingNumberOfSlots;
        [SerializeField] private GameObject player;
        [SerializeField] private MasterItem startingItem;
        [SerializeField] private MainWidget mainWidget;
        [SerializeField] private CraftingMenu craftingMenu;
        [SerializeField] private ActionMenu actionMenu;
        private InventoryGrid _grid;
        [SerializeField] private Vector3 actionMenuOffset;
        public InventorySlot[] Slots { get; private set; }
        [SerializeField] private List<MasterItem> startingCraftableItems=new List<MasterItem>();
         void Awake()
        {
            ChangeAmountOfSlots(startingNumberOfSlots);
            PopulateCraftableList();
            Slots = new InventorySlot[AmountOfSlots];
            Debug.Log("The slots are created");
        }

         public void PopulateCraftableList()
         {
             foreach (var craftableItem in startingCraftableItems)
             {
                 UnlockedCraftableItems.Add(craftableItem);
             }
         }
         public void SetGrid(InventoryGrid grid)
         {
             _grid = grid;
         }
         public void ChangeAmountOfSlots(int slots)
        {
            AmountOfSlots = slots;
        }
        public bool IsSlotEmpty(int index)
        {
            return Slots[index]==null;
        }

        public (bool Success,ItemInfo ItemInfo,int Amount) GetItemAtIndex(int index)
        {
            if (IsSlotEmpty(index))
            {
                return (Success:true, ItemInfo:null,Amount:-1);
            }
            return (Success:false, ItemInfo:Slots[index].ItemClass.info, Amount:Slots[index].Amount);
        }
        public (bool Success,int Index) SearchEmptySlot()   
        {
            for (int i = 0; i < Slots.Length;i++)
            {
                if (IsSlotEmpty(i))
                    return (Success:true, Index:i);
            }
            return (Success:false, Index:-1);
        }

        public (bool Success, int Index) SearchFreeStack(MasterItem itemClass)
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (!IsSlotEmpty(i))
                {
                    if (Slots[i].ItemClass == itemClass && Slots[i].Amount<_maxStackSize)
                        return (Success:true, Index:i);
                }
            }

            return (Success:false, Index:-1);
        }

        public (bool Success,int Remainder) AddItem(MasterItem itemClass, int amount)
        {

            var result=AddItemFunctionality(itemClass, amount);
            if (result.Success)
            {
                mainWidget.AddItemsToQueue(itemClass,amount-result.Remainder);
                UpdateCraftingMenu();
                return (Success: true, Remainder: result.Remainder);
            }
            return (Success: false, Remainder: result.Remainder);
        }

        public (bool Success, int Remainder) AddItemFunctionality(MasterItem itemClass, int amount)
        {
            if (!itemClass.info.CanStack)
            {
                var emptySlot =SearchEmptySlot();
                if (emptySlot.Success)
                {
                    Slots[emptySlot.Index] = new InventorySlot(itemClass,1);
                    UpdateSlotAtIndex(emptySlot.Index);
                    if (amount > 1)
                    {
                        var addItem=AddItemFunctionality(itemClass, amount - 1);
                        return (Success:true,Remainder:addItem.Remainder);
                    }
                }
                else
                {
                    return (Success:false,Remainder:amount);
                }
            }
            else
            {
                var freeStack = SearchFreeStack(itemClass);
                if (!freeStack.Success)
                {
                    var emptySlot = SearchEmptySlot();
                    if (!emptySlot.Success)
                        return (Success:false,Remainder:amount);
                    if (amount > _maxStackSize)
                    {
                        Slots[emptySlot.Index] = new InventorySlot(itemClass, _maxStackSize);
                        UpdateSlotAtIndex(emptySlot.Index);
                        var addItem=AddItemFunctionality(itemClass, amount - _maxStackSize);
                        return (Success:true,Remainder:addItem.Remainder);
                    }
                    else
                    {
                        Slots[emptySlot.Index] = new InventorySlot(itemClass, amount);
                        UpdateSlotAtIndex(emptySlot.Index);
                        return (Success:true,Remainder:0) ;
                    }
                }
                else
                {
                    int sum = amount + Slots[freeStack.Index].Amount;
                    if (sum > _maxStackSize)
                    {
                        Slots[freeStack.Index] = new InventorySlot(itemClass, _maxStackSize);
                        UpdateSlotAtIndex(freeStack.Index);
                        var addItem=AddItemFunctionality(itemClass, sum - _maxStackSize);
                        return (Success:true,Remainder:addItem.Remainder);
                    }
                    else
                    {
                        Slots[freeStack.Item2] = new InventorySlot(itemClass, sum);
                        UpdateSlotAtIndex(freeStack.Item2);
                        return (Success:true,Remainder:0);
                    }
                }
            }
            return (Success:false,Remainder:0);
        }
        public int GetAmountAtIndex(int index)
        {
            return Slots[index].Amount;
        }

        public void UpdateSlotAtIndex(int index)
        {
            _grid.Slots[index].UpdateSlot();
        }

        public bool RemoveItemAtIndex(int index, int amount)
        {
            if (!IsSlotEmpty(index) && amount > 0)
            {
                if (amount >= GetAmountAtIndex(index))
                {
                    Slots[index] = null;
                    UpdateSlotAtIndex(index);
                    UpdateCraftingMenu();
                    return true;
                }
                else
                {
                    Slots[index] = new InventorySlot(Slots[index].ItemClass, Slots[index].Amount - amount);
                    UpdateSlotAtIndex(index);
                    UpdateCraftingMenu();
                    return true;
                }
            }

            return false;
        }

        public bool SwapSlots(int index1, int index2)
        {
            if (index1 < Slots.Length && index2 < Slots.Length)
            {
                (Slots[index2], Slots[index1]) = (Slots[index1], Slots[index2]);
                UpdateSlotAtIndex(index1);
                UpdateSlotAtIndex(index2);
                return true;
            }

            return false;
        }

        public bool SplitStack(int index, int amount)
        {
            if (!IsSlotEmpty(index))
            {
                var itemInfoAtIndex = GetItemAtIndex(index);
                if (itemInfoAtIndex.ItemInfo.CanStack && itemInfoAtIndex.Amount >amount)
                {
                    var emptySlot = SearchEmptySlot();
                    if (emptySlot.Success)
                    {
                        Slots[index] =
                            new InventorySlot(Slots[index].ItemClass, Slots[index].Amount - amount);
                        Slots[emptySlot.Index] = new InventorySlot(Slots[index].ItemClass, amount);
                        UpdateSlotAtIndex(index);
                        UpdateSlotAtIndex(emptySlot.Index);
                        return true;
                    }
                }
            }
            return false;   
        }


        public bool UseItemAtIndex(int index)
        {
            if (!IsSlotEmpty(index))
            { 
                if (Slots[index].ItemClass.info.CanBeUsed)
                {
                    Slots[index].ItemClass.UseItem(this,index);
                    return true;
                }

                return false;
            }

            return false;
        }

        public void OnSlotClicked(InventoryUISlot inventorySlot, PointerEventData clickType)
        {
            if (clickType.button == PointerEventData.InputButton.Right)
            {
                actionMenu.UpdateMenu(inventorySlot.SlotIndex);
                actionMenu.gameObject.SetActive(true);
                actionMenu.transform.position = inventorySlot.transform.position+actionMenuOffset;
                inventorySlot.UpdateSlot();
            }
        }

        public bool SameClassSlots(int index1, int index2)
        {
            if(!IsSlotEmpty(index1) && !IsSlotEmpty(index2))
                return Slots[index1].ItemClass == Slots[index2].ItemClass;
            return false;
        }
        public bool AddToIndex(int fromIndex, int toIndex)
        {
            if (SameClassSlots(fromIndex, toIndex) &&
                Slots[toIndex].Amount<_maxStackSize && 
                Slots[fromIndex].ItemClass.info.CanStack)
            {
                int rest = _maxStackSize - GetAmountAtIndex(toIndex);
                if (rest >= GetAmountAtIndex(fromIndex))
                {
                    Slots[toIndex] = new InventorySlot(Slots[fromIndex].ItemClass, GetAmountAtIndex(toIndex) + GetAmountAtIndex(fromIndex));
                    Slots[fromIndex] = null;
                    UpdateSlotAtIndex(fromIndex);
                    UpdateSlotAtIndex(toIndex);
                    return true;
                }
                Slots[toIndex] = new InventorySlot(Slots[fromIndex].ItemClass, _maxStackSize);
                Slots[fromIndex] = new InventorySlot(Slots[fromIndex].ItemClass, GetAmountAtIndex(fromIndex)-rest);
                UpdateSlotAtIndex(fromIndex);
                UpdateSlotAtIndex(toIndex);
                return true;
            }
            return false;
        }

        public bool SplitStackToIndex(int fromIndex,int toIndex,int amount)
        {
            if (IsSlotEmpty(toIndex) && !IsSlotEmpty(fromIndex))
            {
                if (GetItemAtIndex(fromIndex).ItemInfo.CanStack &&
                    GetItemAtIndex(fromIndex).Amount > 1 &&
                    GetItemAtIndex(fromIndex).Amount > amount)
                {
                    var localClass = Slots[fromIndex].ItemClass;
                    Slots[fromIndex] = new InventorySlot(localClass, Slots[fromIndex].Amount - amount);
                    Slots[toIndex] = new InventorySlot(localClass, amount);
                    UpdateSlotAtIndex(fromIndex);
                    UpdateSlotAtIndex(toIndex);
                    return true;
                }
            }
            return false;
        }

        public (int TotalAmount,List<int> SlotIndeces)GetTotalAmountOfItems(MasterItem itemClass)
        {
            List<int> indecesFound = new List<int>();
            int amount = 0;
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i] != null && Slots[i].ItemClass == itemClass)
                {
                    indecesFound.Add(i);
                    amount += GetAmountAtIndex(i);
                }
            }

            return (amount, indecesFound);
        }

        public bool RemoveItem(MasterItem itemClass, int amount)
        {
            var result = GetTotalAmountOfItems(itemClass);
            if (result.TotalAmount >= amount)
            {
                var indeces = result.SlotIndeces;
                foreach (var index in indeces)
                {
                    if (GetAmountAtIndex(index) >= amount)
                    {
                        RemoveItemAtIndex(index, amount);
                        return true;
                    }
                    amount -= GetAmountAtIndex(index);
                    RemoveItemAtIndex(index,GetAmountAtIndex(index));
                    
                }
            }
            return false;
        }

        public void UpdateCraftingMenu()
        {
            if (craftingMenu.GetItemClass()!=null)
            {
                craftingMenu.UpdateDetailWindow(craftingMenu.GetItemClass());
            }
        }
    }
}
