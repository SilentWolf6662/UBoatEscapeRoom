#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.Player.Draggable.cs © SilentWolf6662 - All Rights Reserved
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
namespace UBER
{
    public class Draggable : CacheBehaviour2D
    {
        public delegate void DragEndedDelegate(Draggable draggableObject);

        public DragEndedDelegate dragEndedCallback;

        private bool isDragged;
        private Vector3 mouseDragStartPosition, spriteDragStartPosition;

        private void OnMouseDown()
        {
            isDragged = true;
            mouseDragStartPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            spriteDragStartPosition = transform.localPosition;
        }

        private void OnMouseDrag()
        {
            if (isDragged) transform.localPosition = spriteDragStartPosition + (mainCamera.ScreenToWorldPoint(Input.mousePosition) - mouseDragStartPosition);
        }

        private void OnMouseUp()
        {
            isDragged = false;
            dragEndedCallback(this);
        }
    }
}
