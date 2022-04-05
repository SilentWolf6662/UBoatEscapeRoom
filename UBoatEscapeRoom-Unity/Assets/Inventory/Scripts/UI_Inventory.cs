#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.Player.UI_Inventory.cs © SilentWolf6662 - All Rights Reserved
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
using TMPro;
using UBER.Inventory;
using UBER.Util;
using UBER.Utils;
using UnityEngine;
using UnityEngine.UI;

#endregion
public class UI_Inventory : CacheBehaviour2D
{
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Transform itemSlotTemplate;
    private Inventory inventory;

    private void Awake()
    {
        itemSlotContainer ??= transform.Find("itemSlotContainer");
        itemSlotTemplate ??= itemSlotContainer.Find("itemSlotTemplate");
    }

    public void ToggleInventory()
    {
        float alpha;
        alpha = (alpha = canvasGroup.alpha) switch { 0 => 1, 1 => 0, _ => alpha };
        canvasGroup.alpha = alpha;
    }


    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e) => RefreshInventoryItems();

    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child.Equals(itemSlotTemplate)) continue;
            Destroy(child.gameObject);
        }

        int x = 0, y = 0;
        const float itemSlotCellSize = 75f;
        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                // Use item
                inventory.UseItem(item);
            };

            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
            {
                // Drop item
                Item duplicateItem = new Item { itemType = item.itemType, amount = item.amount };
                inventory.RemoveItem(item);
                ItemWorld.DropItem(mainCamera.transform.position, duplicateItem);
            };

            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            TextMeshProUGUI uiText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
            uiText.SetText(item.amount > 1 ? item.amount.ToString() : "");

            x++;
            if (x >= 4)
            {
                x = 0;
                y++;
            }
        }
    }
}
