using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UBER.Core.Player;
using UBER.Inventory;
using UBER.Util;
using UBER.Utils;

public class UI_Inventory : MonoBehaviour 
{

    private Inventory inventory;
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Transform itemSlotTemplate;

    private void Awake()
    {
        itemSlotContainer ??= transform.Find("itemSlotContainer");
        itemSlotTemplate ??= itemSlotContainer.Find("itemSlotTemplate");
    }

    public void SetInventory(Inventory inventory) 
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e) => RefreshInventoryItems();

    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer) 
        {
            if (child.Equals(itemSlotTemplate)) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        const float itemSlotCellSize = 75f;
        foreach (Item item in inventory.GetItemList()) 
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            
            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () => {
                // Use item
                inventory.UseItem(item);
            };
            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () => {
                // Drop item
                Item duplicateItem = new Item { itemType = item.itemType, amount = item.amount };
                inventory.RemoveItem(item);
                ItemWorld.DropItem(CacheBehaviour2D.mainCamera.transform.position, duplicateItem);
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
