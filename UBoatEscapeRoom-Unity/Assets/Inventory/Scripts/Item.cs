using System;
using UnityEngine;

[Serializable]
public class Item 
{

    public enum ItemType 
    {
        Key,
        Note
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite() => itemType switch { ItemType.Key => ItemAssets.Instance.keySprite, ItemType.Note => ItemAssets.Instance.noteSprite };

    public Color GetColor() => itemType switch { ItemType.Key => new Color(1, 1, 1), ItemType.Note => new Color(1, 0, 0) };

    public bool IsStackable() => itemType switch { ItemType.Key => false, ItemType.Note => false, _ => false };

}
