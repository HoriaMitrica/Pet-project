using System;
using Items;
using Structures;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int AmountOfSlots { get; private set; }
    private int _maxStackSize = 64;

    [SerializeField] private GameObject player;
    [SerializeField] private MasterItem startingItem;
    private InventorySlot[] _slots;

    public void Awake()
    {
        ChangeAmountOfSlots(5);
        _slots = new InventorySlot[AmountOfSlots];
        Debug.Log("The slots are created");
        _slots[0] = new InventorySlot(startingItem, 10);
    }

    public void ChangeAmountOfSlots(int slots)
    {
        AmountOfSlots = slots;
    }
    public bool IsSlotEmpty(int index)
    {
        return _slots[index]==null;
    }

    public Tuple<bool,ItemInfo,int> GetItemAtIndex(int index)
    {
        if (IsSlotEmpty(index))
        {
            return new Tuple<bool, ItemInfo, int>(true, null,-1);
        }
        return new Tuple<bool, ItemInfo, int>(false, _slots[index].ItemClass.Info, _slots[index].Amount);
    }

    public Tuple<bool,int> SearchEmptySlot()
    {
        for (int i = 0; i < _slots.Length;i++)
        {
            if (IsSlotEmpty(i))
                return new Tuple<bool, int>(true, i);
        }
        return new Tuple<bool, int>(false, -1);
    }

    public Tuple<bool, int> SearchFreeStack(MasterItem itemClass)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (!IsSlotEmpty(i))
            {
                if (_slots[i].ItemClass == itemClass && _slots[i].Amount<_maxStackSize)
                    return new Tuple<bool, int>(true, i);
            }
        }

        return new Tuple<bool, int>(false, -1);
    }

    public Tuple<bool,int> AddItem(MasterItem itemClass, int amount)
    {
        
        Tuple<bool, int> emptySlot;
        if (!itemClass.Info.CanStack)
        {
            emptySlot=SearchEmptySlot();
            if (emptySlot.Item1)
            {
                _slots[emptySlot.Item2] = new InventorySlot(itemClass,1);
                if (amount > 1)
                {
                    var addItem=AddItem(itemClass, amount - 1);
                    return new Tuple<bool, int>(true,addItem.Item2);
                }
            }
            else
            {
                return new Tuple<bool, int>(false,amount);
            }
        }
        else
        {
            var freeStack = SearchFreeStack(itemClass);
            Tuple<bool, int> addItem;
            if (!freeStack.Item1)
            {
                emptySlot = SearchEmptySlot();
                if (!emptySlot.Item1)
                    return new Tuple<bool, int>(false,amount);
                if (amount > _maxStackSize)
                {
                    _slots[emptySlot.Item2] = new InventorySlot(itemClass, _maxStackSize);
                    addItem=AddItem(itemClass, amount - _maxStackSize);
                    return new Tuple<bool, int>(true,addItem.Item2);
                }
                else
                {
                    _slots[emptySlot.Item2] = new InventorySlot(itemClass, amount);
                    return new Tuple<bool, int>(true,0) ;
                }
            }
            else
            {
                int sum = amount + _slots[freeStack.Item2].Amount + amount;
                if (sum > _maxStackSize)
                {
                    _slots[freeStack.Item2] = new InventorySlot(itemClass, 64);
                    addItem=AddItem(itemClass, sum - _maxStackSize);
                    return new Tuple<bool, int>(true,addItem.Item2);
                }
                else
                {
                    _slots[freeStack.Item2] = new InventorySlot(itemClass, sum);
                    return new Tuple<bool, int>(true,0);
                }
            }
        }
        return new Tuple<bool, int>(false,0);
    }

    public int GetAmountAtIndex(int index)
    {
        return _slots[index].Amount;
    }
    
}
