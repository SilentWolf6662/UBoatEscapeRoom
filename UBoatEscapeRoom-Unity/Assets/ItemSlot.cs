#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.Player.ItemSlot.cs © SilentWolf6662 - All Rights Reserved
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
using UnityEngine.EventSystems;

#endregion
namespace UBER
{
    public class ItemSlot : CacheBehaviour2D, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            print("OnDrop");
            if (eventData.pointerDrag is null) return;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
        }
    }
}
