#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.Player.DragDrop.cs © SilentWolf6662 - All Rights Reserved
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
    public class DragDrop : CacheBehaviour2D, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IInitializePotentialDragHandler
    {
        [SerializeField] private Canvas canvas;

        public void OnBeginDrag(PointerEventData eventData)
        {
            print("OnBeginDrag");
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            print("OnDrag");
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor * 2;
        }

        public void OnDrop(PointerEventData eventData) {}

        public void OnEndDrag(PointerEventData eventData)
        {
            print("OnEndDrag");
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        public void OnInitializePotentialDrag(PointerEventData eventData) => eventData.useDragThreshold = false;

        public void OnPointerDown(PointerEventData eventData) => print("OnPointerDown");
    }
}
