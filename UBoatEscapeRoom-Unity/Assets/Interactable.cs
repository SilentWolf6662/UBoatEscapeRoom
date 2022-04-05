#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.Interactable.cs © SilentWolf6662 - All Rights Reserved
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
    public class Interactable : CacheBehaviour2D, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            GameObject dragData = eventData.pointerDrag;
            print("OnDrop");
            if (dragData == null) return;
            InteractTypes interactType = dragData.GetComponent<InteractType>().interactType;
            if (interactType == InteractTypes.ItemSlot) dragData.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
            else if (interactType == InteractTypes.Note) print("Note Interaction");
            else if (interactType == InteractTypes.Key) print("Key Interaction");
            else print("Not Implemented Yet");
        }
    }
}
