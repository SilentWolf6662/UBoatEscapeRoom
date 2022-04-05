#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.Item.cs © SilentWolf6662 - All Rights Reserved
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
using UnityEngine;

#endregion
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

    public Sprite GetSprite() => itemType switch { ItemType.Key => ItemAssets.Instance.keySprite, ItemType.Note => ItemAssets.Instance.noteSprite, _ => throw new ArgumentOutOfRangeException() };

    public Color GetColor() => itemType switch { ItemType.Key => new Color(1, 1, 1), ItemType.Note => new Color(1, 0, 0), _ => throw new ArgumentOutOfRangeException() };

    public bool IsStackable() => itemType switch { ItemType.Key => false, ItemType.Note => false, _ => false };
}
