#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.Player.ItemAssets.cs © SilentWolf6662 - All Rights Reserved
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

using UBER.Util;
using UnityEngine;

#endregion
public class ItemAssets : CacheBehaviour2D
{

    public Transform pfItemWorld;

    public Sprite keySprite;
    public Sprite noteSprite;
    public static ItemAssets Instance { get; private set; }

    private void Awake() => Instance = this;
}
