using UnityEngine;
using TMPro;
using UBER.Util;

public class ItemWorld : CacheBehaviour2D 
{

    public static ItemWorld SpawnItemWorld(Vector3 position, Item item) 
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    public static ItemWorld DropItem(Vector3 dropPosition, Item item) 
    {
        Vector3 randomDir = Extension.GetRandomDir();
        ItemWorld itemWorld = SpawnItemWorld(dropPosition + randomDir * 8f, item);
        itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 40f, ForceMode2D.Impulse);
        return itemWorld;
    }

    private Item item;
    public void SetItem(Item item) 
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
        light2D.color = item.GetColor();
        textMeshPro.SetText(item.amount > 1 ? item.amount.ToString() : "");
    }

    public Item GetItem() => item;

    public void DestroySelf() => Destroy(gameObject);
}
