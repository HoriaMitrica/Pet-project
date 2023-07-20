using Items;
using Structures;
using UnityEngine;

namespace _Inventory
{
    public class Inventory : MonoBehaviour
    {
        public int AmountOfSlots { get; private set; }
        private int _maxStackSize = 64;
        
        [SerializeField] private int startingNumberOfSlots;
        [SerializeField] private GameObject player;
        [SerializeField] private MasterItem startingItem;
        [SerializeField] private MainWidget mainWidget;
        [SerializeField] private ActionMenu actionMenu;
        private InventoryGrid _inventoryGrid;
        public InventorySlot[] Slots { get; private set; }

         void Awake()
        {
            ChangeAmountOfSlots(startingNumberOfSlots);
            Slots = new InventorySlot[AmountOfSlots];
            Debug.Log("The slots are created");
        }

         void Start()
         {
             _inventoryGrid = mainWidget.InventoryGrid;
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
            return (Success:false, ItemInfo:Slots[index].ItemClass.Info, Amount:Slots[index].Amount);
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
            
            if (!itemClass.Info.CanStack)
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
            _inventoryGrid.Slots[index].UpdateSlot();
        }

        public bool RemoveItemAtIndex(int index, int amount)
        {
            if (!IsSlotEmpty(index) && amount > 0)
            {
                if (amount >= GetAmountAtIndex(index))
                {
                    Slots[index] = new InventorySlot(null, 0);
                    UpdateSlotAtIndex(index);
                    return true;
                }
                else
                {
                    Slots[index] = new InventorySlot(Slots[index].ItemClass, Slots[index].Amount - amount);
                    UpdateSlotAtIndex(index);
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

        // ReSharper disable Unity.PerformanceAnalysis
        public bool UseItemAtIndex(int index)
        {
            if (!IsSlotEmpty(index))
            { 
                if (Slots[index].ItemClass.Info.CanBeUsed)
                {
                    Slots[index].ItemClass.UseItem(this,index);
                    return true;
                }

                return false;
            }

            return false;
        }

        public void OnSlotClicked()
        {
            actionMenu.gameObject.SetActive(true);
        }
    }
}
