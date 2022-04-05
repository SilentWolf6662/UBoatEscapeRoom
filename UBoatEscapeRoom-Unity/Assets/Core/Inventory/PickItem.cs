#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.PickItem.cs © SilentWolf6662 - All Rights Reserved
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
namespace UBER.Core.Inventory
{
    public class PickItem : CacheBehaviour2D
    {
        public string itemName = "Some Item"; //Each item must have an unique name
        public Texture itemPreview;

        private void Start() => gameObject.tag = "InteractableObject"; //Change item tag to Respawn to detect when we look at it

        public void PickupItem() => Destroy(gameObject);

        public void Use()
        {

        }
    }
}
