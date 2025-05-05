using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class Slot : ISaveLoadProcessing
    {
        [SerializeField, ReadOnly] private Item item;
        [SerializeField, ReadOnly] private int amount;
        [SerializeField, ReadOnly] private int index;
        [SerializeField, ReadOnly] private string itemKey;

        public Slot(int index) => this.index = index;

        public Slot(Item item, int amount, int index)
        {
            this.item = item;
            this.amount = amount;
            this.index = index;
        }

        public event Action OnChanged;

        public int Amount
        {
            get => amount;
            set
            {
                value = Mathf.Clamp(value, 0, 999);
                if (value == amount) return;
                amount = value;
                OnChanged?.Invoke();
            }
        }

        public Item Item
        {
            get => item;
            set
            {
                if (value == item) return;
                item = value;
                OnChanged?.Invoke();
            }
        }

        public int Index
        {
            get => index;
            set
            {
                if (value == index) return;
                index = value;
                OnChanged?.Invoke();
            }
        }

        public void OnBeforeSaving() => itemKey = item?.name;

        public void OnAfterLoading() => item = ItemUtils.AllItems[itemKey];
    }

    static class ItemUtils
    {
        private static Dictionary<string, Item> allItems;
        public static Dictionary<string, Item> AllItems => allItems ??= Resources.LoadAll<Item>("Consumables")
                .ToDictionary(definition => definition.name, definition => definition);
    }
}