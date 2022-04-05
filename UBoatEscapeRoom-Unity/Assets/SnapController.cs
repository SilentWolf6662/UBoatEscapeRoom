#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.SnapController.cs © SilentWolf6662 - All Rights Reserved
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

using System.Collections.Generic;
using UBER.Util;
using UnityEngine;

#endregion
namespace UBER
{
    public class SnapController : CacheBehaviour2D
    {
        public List<Transform> interactPoints;
        public List<Draggable> draggableObjects;
        public List<InteractType> itemSlots, interactables;
        public float interactRange = 0.5f;

        protected override void OnAwake()
        {
            base.OnAwake();
            for (int i = 0; i < itemSlots.Count; i++) if (itemSlots[i].interactType != InteractTypes.ItemSlot) itemSlots.Remove(itemSlots[i]);
            for (int i = 0; i < interactables.Count; i++)
            {
                if (interactables[i].interactType == InteractTypes.ItemSlot) interactables.Remove(interactables[i]);
            }
        }

        private void Start()
        {
            foreach (Draggable draggable in draggableObjects) draggable.dragEndedCallback = OnDragEnded;
        }

        private void OnDragEnded(Draggable draggable)
        {

            float closestDistance = -1;
            Transform closestInteractPoint = null;

            foreach (Transform interactPoint in interactPoints)
            {
                float currentDistance = Vector2.Distance(draggable.transform.localPosition, interactPoint.localPosition);
                if (closestInteractPoint != null && !(currentDistance < closestDistance)) continue;
                closestInteractPoint = interactPoint;
                closestDistance = currentDistance;
            }

            if (closestInteractPoint is null || !(closestDistance <= interactRange)) return;
            InteractTypes interactPointType = closestInteractPoint.GetComponent<InteractType>().interactType;
            if (interactPointType == InteractTypes.ItemSlot) draggable.transform.localPosition = closestInteractPoint.localPosition;
            if (interactPointType == InteractTypes.Key) {}
        }
    }
}
