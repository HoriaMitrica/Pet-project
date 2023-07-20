using System;
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
        [SerializeField] private GameObject inventoryUI;
        private Canvas _inventoryUICanvas;
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
             _inventoryUICanvas = inventoryUI.GetComponent<Canvas>();

         }
         private void Update()
         {
             if (Input.GetKeyDown(KeyCode.I))
             {
                 _inventoryUICanvas.enabled = !_inventoryUICanvas.enabled;
             }
         }
        public void ChangeAmountOfSlots(int slots)
        {
            AmountOfSlots = slots;
        }
        public bool IsSlotEmpty(int index)
        {
            return Slots[index]==null;
        }

        public Tuple<bool,ItemInfo,int> GetItemAtIndex(int index)
        {
            if (IsSlotEmpty(index))
            {
                return new Tuple<bool, ItemInfo, int>(true, null,-1);
            }
            return new Tuple<bool, ItemInfo, int>(false, Slots[index].ItemClass.Info, Slots[index].Amount);
        }

        public Tuple<bool,int> SearchEmptySlot()
        {
            for (int i = 0; i < Slots.Length;i++)
            {
                if (IsSlotEmpty(i))
                    return new Tuple<bool, int>(true, i);
            }
            return new Tuple<bool, int>(false, -1);
        }

        public Tuple<bool, int> SearchFreeStack(MasterItem itemClass)
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (!IsSlotEmpty(i))
                {
                    if (Slots[i].ItemClass == itemClass && Slots[i].Amount<_maxStackSize)
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
                    Slots[emptySlot.Item2] = new InventorySlot(itemClass,1);
                    UpdateSlotAtIndex(emptySlot.Item2);
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
                        Slots[emptySlot.Item2] = new InventorySlot(itemClass, _maxStackSize);
                        UpdateSlotAtIndex(emptySlot.Item2);
                        addItem=AddItem(itemClass, amount - _maxStackSize);
                        return new Tuple<bool, int>(true,addItem.Item2);
                    }
                    else
                    {
                        Slots[emptySlot.Item2] = new InventorySlot(itemClass, amount);
                        UpdateSlotAtIndex(emptySlot.Item2);
                        return new Tuple<bool, int>(true,0) ;
                    }
                }
                else
                {
                    int sum = amount + Slots[freeStack.Item2].Amount;
                    if (sum > _maxStackSize)
                    {
                        Slots[freeStack.Item2] = new InventorySlot(itemClass, 64);
                        UpdateSlotAtIndex(freeStack.Item2);
                        addItem=AddItem(itemClass, sum - _maxStackSize);
                        return new Tuple<bool, int>(true,addItem.Item2);
                    }
                    else
                    {
                        Slots[freeStack.Item2] = new InventorySlot(itemClass, sum);
                        UpdateSlotAtIndex(freeStack.Item2);
                        return new Tuple<bool, int>(true,0);
                    }
                }
            }
            return new Tuple<bool, int>(false,0);
        }

        public int GetAmountAtIndex(int index)
        {
            return Slots[index].Amount;
        }

        public void UpdateSlotAtIndex(int index)
        {
            _inventoryGrid.Slots[index].UpdateSlot();
        }
    }
}
