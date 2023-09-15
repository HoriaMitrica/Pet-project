using System.Collections.Generic;
using System.Linq;
using Enums;
using Items;
using Structures;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Inventory
{
    public class Storage : MonoBehaviour
    {
        public int AmountOfSlots { get; private set; }
        private int _maxStackSize = 64;
        [SerializeField] private Inventory inventory;

        public Inventory PlayerInventory
        {
            get { return inventory; }
        }
    
        [SerializeField] private List<Chest> chests;
        public InventorySlot[] Slots { get; private set; }
        private int _activeChest = -1;
        private StorageGrid _grid;

        public void SetActiveChest(int id)
        {
            _activeChest = id;
        }
        public void CreateSlots(InventorySlot[] slots)
        {
            Slots = new InventorySlot[AmountOfSlots];
        }
        
         public void SetGrid(StorageGrid grid)
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

        public (bool Success, int Remainder) AddItem(MasterItem itemClass, int amount)
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
                        var addItem=AddItem(itemClass, amount - 1);
                        return (Success:true,Remainder:addItem.Remainder);
                    }
                    return (Success:true,Remainder:0);
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
                        var addItem=AddItem(itemClass, amount - _maxStackSize);
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
                        var addItem=AddItem(itemClass, sum - _maxStackSize);
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
            var chest=chests.Find(chest => chest.Id == _activeChest);
            if (!IsSlotEmpty(index) && amount > 0)
            {
                if (amount >= GetAmountAtIndex(index))
                {
                    chest.RemoveItem(index,Slots[index],GetAmountAtIndex(index));
                    Slots[index] = null;
                    UpdateSlotAtIndex(index);
                    return true;
                }
                chest.RemoveItem(index,Slots[index],Slots[index].Amount - amount);
                Slots[index] = new InventorySlot(Slots[index].ItemClass, Slots[index].Amount - amount);
                UpdateSlotAtIndex(index);
                return true;
            }
            return false;
        }

        public bool SwapSlots(int index1, int index2)
        {
            if (index1 < Slots.Length && index2 < Slots.Length)
            {
                var chest=chests.Find(chest => chest.Id == _activeChest);
                (Slots[index2], Slots[index1]) = (Slots[index1], Slots[index2]);
                UpdateSlotAtIndex(index1);
                UpdateSlotAtIndex(index2);
                chest.AddItem(index1,Slots[index1]);
                chest.AddItem(index2,Slots[index2]);
                return true;
            }
            return false;
        }
        
        public bool SameClassSlots(int index1, int index2)
        {
            if(!IsSlotEmpty(index1) && !IsSlotEmpty(index2))
                return Slots[index1].ItemClass == Slots[index2].ItemClass;
            return false;
        }
        public bool AddToIndex(int fromIndex, int toIndex)
        {
            var chest=chests.Find(chest => chest.Id == _activeChest);
            if (SameClassSlots(fromIndex, toIndex) &&
                Slots[toIndex].Amount<_maxStackSize && 
                Slots[fromIndex].ItemClass.info.CanStack)
            {
                int rest = _maxStackSize - GetAmountAtIndex(toIndex);
                if (rest >= GetAmountAtIndex(fromIndex))
                {
                    Slots[toIndex] = new InventorySlot(Slots[fromIndex].ItemClass, GetAmountAtIndex(toIndex) + GetAmountAtIndex(fromIndex));
                    Slots[fromIndex] = null;
                    chest.AddItem(toIndex,Slots[toIndex]);
                    chest.AddItem(fromIndex,Slots[fromIndex]);
                    UpdateSlotAtIndex(fromIndex);
                    UpdateSlotAtIndex(toIndex);
                    return true;
                }
                Slots[toIndex] = new InventorySlot(Slots[fromIndex].ItemClass, _maxStackSize);
                Slots[fromIndex] = new InventorySlot(Slots[fromIndex].ItemClass, GetAmountAtIndex(fromIndex)-rest);
                chest.AddItem(toIndex,Slots[toIndex]);
                chest.AddItem(fromIndex,Slots[fromIndex]);
                UpdateSlotAtIndex(fromIndex);
                UpdateSlotAtIndex(toIndex);
                return true;
            }
            return false;
        }
        public bool AddItemAtIndexInternal(int index, MasterItem itemClass, int amount)
        {
            if (IsSlotEmpty(index) && amount <= _maxStackSize)
            { 
                var chest=chests.Find(chest => chest.Id == _activeChest);
                Slots[index]=new InventorySlot(itemClass, amount);
                UpdateSlotAtIndex(index);
                return true;
            }
            return false;
        }
        public bool AddItemAtIndex(int index, MasterItem itemClass, int amount)
        {
            if (IsSlotEmpty(index) && amount <= _maxStackSize)
            { 
                var chest=chests.Find(chest => chest.Id == _activeChest);
                Slots[index]=new InventorySlot(itemClass, amount);
                chest.AddItem(index,Slots[index]);
                UpdateSlotAtIndex(index);
                return true;
            }
            return false;
        }

        public bool IncreaseAmountAtIndex(int index, int amount)
        {
            if (!IsSlotEmpty(index) && GetAmountAtIndex(index) + amount <= _maxStackSize)
            {
                Slots[index] = new InventorySlot(Slots[index].ItemClass, Slots[index].Amount + amount);
                UpdateSlotAtIndex(index);
                return true;
            }
            return false;
        }
        
        public bool MoveFromStorageToInventoryIndex(int storageIndex, int inventoryIndex)
        {
            if (inventory.IsSlotEmpty(inventoryIndex))
            {
                int amountToAdd = GetAmountAtIndex(storageIndex);
                if (inventory.AddItemAtIndex(inventoryIndex, Slots[storageIndex].ItemClass, amountToAdd))
                {
                    RemoveItemAtIndex(storageIndex, amountToAdd);
                    return true;
                }

                return false;
            }
            if (Slots[storageIndex].ItemClass == inventory.Slots[inventoryIndex].ItemClass &&
                GetItemAtIndex(storageIndex).ItemInfo.CanStack)
            {
                int amountToAdd =
                    _maxStackSize - GetAmountAtIndex(storageIndex) < GetAmountAtIndex(inventoryIndex)
                        ? _maxStackSize - GetAmountAtIndex(storageIndex)
                        : GetAmountAtIndex(inventoryIndex);
                if (inventory.IncreaseAmountAtIndex(inventoryIndex, amountToAdd))
                {
                    RemoveItemAtIndex(storageIndex, amountToAdd);
                    return true;
                }
            }
            return false;
        }
    }
}
