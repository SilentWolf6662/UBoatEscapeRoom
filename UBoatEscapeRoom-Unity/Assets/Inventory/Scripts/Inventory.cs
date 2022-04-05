#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.Inventory.cs © SilentWolf6662 - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// 
// This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/
// 
// Created & Copyrighted @ 2022-04-05
// 
// ******************************************************************************************************************

#endregion
#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion
namespace UBER.Inventory
{
    public class Inventory
    {

        private readonly List<Item> itemList;
        private readonly Action<Item> useItemAction;

        public Inventory(Action<Item> useItemAction)
        {
            this.useItemAction = useItemAction;
            itemList = new List<Item>();

            AddItem(new Item { itemType = Item.ItemType.Key, amount = 1 });
            AddItem(new Item { itemType = Item.ItemType.Note, amount = 1 });
        }
        public event EventHandler OnItemListChanged;

        public void AddItem(Item item)
        {
            if (item.IsStackable())
            {
                bool itemAlreadyInInventory = false;
                foreach (Item inventoryItem in itemList.Where(inventoryItem => inventoryItem.itemType == item.itemType))
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
                if (!itemAlreadyInInventory) itemList.Add(item);
            }
            else itemList.Add(item);
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveItem(Item item)
        {
            if (item.IsStackable())
            {
                Item itemInInventory = null;
                foreach (Item inventoryItem in itemList.Where(inventoryItem => inventoryItem.itemType == item.itemType))
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
                if (itemInInventory != null && itemInInventory.amount <= 0) itemList.Remove(itemInInventory);
            }
            else itemList.Remove(item);
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UseItem(Item item) => useItemAction(item);

        public List<Item> GetItemList() => itemList;
    }
}
