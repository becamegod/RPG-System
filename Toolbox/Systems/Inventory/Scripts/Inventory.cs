using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(menuName = "Inventory/Inventory")]
    public class Inventory : ScriptableObject
    {
        [SerializeField] int size = -1;

        // props
        public List<Slot> Slots => slots;
        public int Size => size < 0 ? slots.Count : size;

        // events
        public event Action OnChanged;

        // fields
        [SerializeField, ReadOnly] private List<Slot> slots;

        public void Init()
        {
            slots = new();
            for (var i = 0; i < size; i++) slots.Add(new(i));
        }

        public void Add(Item item, int amount = 1)
        {
            // add to stack
            try
            {
                var slot = slots.First(slot => slot.Item == item && slot.Amount < item.StackSize || !slot.Item);
                slot.Item = item;
                slot.Amount += amount;

                var overflownAmount = slot.Amount - slot.Item.StackSize;
                if (overflownAmount > 0)
                {
                    slot.Amount = slot.Item.StackSize;
                    Add(item, overflownAmount);
                }
            }

            // create new slot
            catch (InvalidOperationException)
            {
                Slot slot = new(item, amount, slots.Count);
                slot.OnChanged += OnSlotChanged;
                slots.Add(slot);
            }
        }

        private void OnSlotChanged() => OnChanged?.Invoke();

        public void Remove(Item item, int amount = 1)
        {
            try
            {
                var slot = slots.First(slot => slot.Item == item);
                Remove(slot, amount);
            }
            catch (InvalidOperationException) { Debug.LogWarning($"Item {item.Name} is not existed to remove"); }
        }

        public void Remove(Slot slot, int amount = 1)
        {
            slot.Amount -= amount;
            if (slot.Amount == 0)
            {
                slot.OnChanged -= OnSlotChanged;
                slots.Remove(slot);
            }
        }
    }
}