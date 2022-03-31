#region Copyright Notice
// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.InteractSystem.cs © SilentWolf6662 - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// 
// This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/
// 
// Created & Copyrighted @ 2022-03-22
// 
// ******************************************************************************************************************
#endregion
using System;
using UBER.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
namespace UBER.Core.Interaction
{
    public class InteractableSystem : CacheBehaviour2D, IPointerClickHandler
    {
        [SerializeField] private InputAction playerInteractionAction;
        [SerializeField] private Inventory.System inventorySystem;
        [SerializeField] private UI_Inventory uiInventory;
        

        public Vector2 mousePosition;
        public RaycastHit2D hit;
        private GameObject objectHit;
        private UBER.Inventory.Inventory inventory;
        private float interactInput;
        private bool hasInteracted;

        private void OnEnable() => playerInteractionAction.Enable();

        private void OnDisable() => playerInteractionAction.Disable();

        protected override void OnAwake()
        {
            inventory = new UBER.Inventory.Inventory(UseItem);
            uiInventory.SetInventory(inventory);
        }
        private void UseItem(Item item)
        {
            switch (item.itemType)
            {
                case Item.ItemType.Key:
                    KeyBehaviour();
                    inventory.RemoveItem(new Item { itemType = Item.ItemType.Key, amount = 1 });
                    break;
                case Item.ItemType.Note:
                    NoteBehaviour();
                    inventory.RemoveItem(new Item { itemType = Item.ItemType.Note, amount = 1 });
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void Update()
        {
            InputHandler();
            InteractHandler();
        }
        private void Interaction()
        {

            if (playerInteractionAction.triggered && hit.collider != null)
            {
                objectHit = hit.collider.gameObject;
                if (objectHit != null) hasInteracted = true;
            }
        }
        public void OnPointerClick(PointerEventData eventData) // 3
        {
            print("I was clicked");
            if (eventData.pointerClick.CompareTag("InventoryBtn")) inventorySystem.showInventory = !inventorySystem.showInventory;
        }
        private void InteractHandler()
        {
            Interaction();
            if (!hasInteracted) return;
            //if (hit.collider.CompareTag("InventoryBtn")) inventorySystem.showInventory = !inventorySystem.showInventory; 
            if (!hit.collider.CompareTag("InventoryBtn")) print($"Target Position: {objectHit.transform.position.ToString()} || Target Name: {objectHit.name}");
            hasInteracted = false;
        }
        private void InputHandler()
        {
            interactInput = playerInteractionAction.ReadValue<float>();
            mousePosition = Mouse.current.position.ReadValue();
            hit = GetMouseRayHit(mousePosition);
        }

        public static RaycastHit2D GetMouseRayHit(Vector2 mousePos) => Physics2D.Raycast(mainCamera.ScreenToWorldPoint(mousePos), Vector2.zero);

        private void KeyBehaviour()
        {
            throw new NotImplementedException();
        }

        private void NoteBehaviour()
        {
            throw new NotImplementedException();
        }
    }
}
