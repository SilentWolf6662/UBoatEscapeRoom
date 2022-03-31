#region Copyright Notice
// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.System.cs © SilentWolf6662 - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// 
// This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/
// 
// Created & Copyrighted @ 2022-03-29
// 
// ******************************************************************************************************************
#endregion
using UBER.Core.Interaction;
using UBER.Core.Player;
using UBER.Util;
using UnityEngine;
namespace UBER.Core.Inventory
{
    public class System : CacheBehaviour2D
    {
        

        public Texture cursorTexture;
        public Movement playerController;
        public PickItem[] availableItems; //List with Prefabs of all the available items
        [SerializeField] private InteractableSystem interactableSystem;

        //Available items slots
        private readonly int[] itemSlots = new int[12];
        public bool showInventory;
        private float windowAnimation = 1;
        private float animationTimer;

        //UI Drag & Drop
        private int hoveringOverIndex = -1;
        private int itemIndexToDrag = -1;
        private Vector2 dragOffset = Vector2.zero;

        //Item Pick up
        private PickItem detectedItem;
        private int detectedItemIndex;

        // Start is called before the first frame update
        private void Start()
        {
            //Initialize Item Slots
            for (int i = 0; i < itemSlots.Length; i++) itemSlots[i] = -1;
        }

        // Update is called once per frame
        private void Update()
        {
            //Show/Hide inventory
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                showInventory = !showInventory;
                animationTimer = 0;
            }

            if (animationTimer < 1) animationTimer += Time.deltaTime;

            if (showInventory)
            {
                windowAnimation = Mathf.Lerp(windowAnimation, 0, animationTimer);
                playerController.canMove = false;
            }
            else
            {
                windowAnimation = Mathf.Lerp(windowAnimation, 1f, animationTimer);
                playerController.canMove = true;
            }

            //Begin item drag
            if (Input.GetMouseButtonDown(0) && hoveringOverIndex > -1 && itemSlots[hoveringOverIndex] > -1) itemIndexToDrag = hoveringOverIndex;

            //Release dragged item
            if (Input.GetMouseButtonUp(0) && itemIndexToDrag > -1)
            {
                if (hoveringOverIndex < 0)
                {
                    //Drop the item outside to Use
                    itemSlots[itemIndexToDrag] = -1;
                }
                else
                {
                    //Switch items between the selected slot and the one we are hovering on
                    (itemSlots[itemIndexToDrag], itemSlots[hoveringOverIndex]) = (itemSlots[hoveringOverIndex], itemSlots[itemIndexToDrag]);

                }
                itemIndexToDrag = -1;
            }

            //Item pick up
            if (!detectedItem || detectedItemIndex <= -1 || !Input.GetKeyDown(KeyCode.E)) return;
            //Add the item to inventory
            int slotToAddTo = -1;
            for (int i = 0; i < itemSlots.Length; i++)
                if (itemSlots[i].Equals(-1))
                {
                    slotToAddTo = i;
                    break;
                }
            if (slotToAddTo > -1)
            {
                itemSlots[slotToAddTo] = detectedItemIndex;
                detectedItem.PickupItem();
            }
        }

        private void FixedUpdate()
        {
            //Detect if the Player is looking at any item

            if (interactableSystem.hit)
            {
                Transform objectHit = interactableSystem.hit.transform;

                if (objectHit.CompareTag("InteractableObject"))
                {
                    if ((detectedItem == null || detectedItem.transform != objectHit) && objectHit.GetComponent<PickItem>() != null)
                    {
                        PickItem itemTmp = objectHit.GetComponent<PickItem>();

                        //Check if item is in availableItemsList
                        for (int i = 0; i < availableItems.Length; i++)
                            if (availableItems[i].itemName == itemTmp.itemName)
                            {
                                detectedItem = itemTmp;
                                detectedItemIndex = i;
                            }
                    }
                }
                else detectedItem = null;
            }
            else detectedItem = null;
        }

        private void OnGUI()
        {
            //Inventory UI
            GUI.Label(new Rect(5, 5, 200, 25), "Press 'Tab' to open Inventory");

            //Inventory window
            if (windowAnimation < 1)
            {
                GUILayout.BeginArea(new Rect(10 - 430 * windowAnimation, Screen.height / 2 - 200, 302, 430), GUI.skin.GetStyle("box"));

                GUILayout.Label("Inventory", GUILayout.Height(25));

                GUILayout.BeginVertical();
                for (int i = 0; i < itemSlots.Length; i += 3)
                {
                    GUILayout.BeginHorizontal();
                    //Display 3 items in a row
                    for (int a = 0; a < 3; a++)
                        if (i + a < itemSlots.Length)
                        {
                            if (itemIndexToDrag.Equals(i + a) || itemIndexToDrag > -1 && hoveringOverIndex.Equals(i + a)) GUI.enabled = false;

                            if (itemSlots[i + a] > -1)
                            {
                                if (availableItems[itemSlots[i + a]].itemPreview) GUILayout.Box(availableItems[itemSlots[i + a]].itemPreview, GUILayout.Width(95), GUILayout.Height(95));
                                else GUILayout.Box(availableItems[itemSlots[i + a]].itemName, GUILayout.Width(95), GUILayout.Height(95));
                            }
                            else GUILayout.Box("", GUILayout.Width(95), GUILayout.Height(95)); //Empty slot

                            //Detect if the mouse cursor is hovering over item
                            Rect lastRect = GUILayoutUtility.GetLastRect();
                            Vector2 eventMousePosition = Event.current.mousePosition;
                            if (Event.current.type == EventType.Repaint && lastRect.Contains(eventMousePosition))
                            {
                                hoveringOverIndex = i + a;
                                if (itemIndexToDrag < 0) dragOffset = new Vector2(lastRect.x - eventMousePosition.x, lastRect.y - eventMousePosition.y);
                            }

                            GUI.enabled = true;
                        }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                if (Event.current.type == EventType.Repaint && !GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition)) hoveringOverIndex = -1;

                GUILayout.EndArea();
            }

            //Item dragging
            if (itemIndexToDrag > -1)
            {
                if (availableItems[itemSlots[itemIndexToDrag]].itemPreview)
                    GUI.Box(new Rect(Input.mousePosition.x + dragOffset.x, Screen.height - Input.mousePosition.y + dragOffset.y, 95, 95), availableItems[itemSlots[itemIndexToDrag]].itemPreview);
                else
                    GUI.Box(new Rect(Input.mousePosition.x + dragOffset.x, Screen.height - Input.mousePosition.y + dragOffset.y, 95, 95), availableItems[itemSlots[itemIndexToDrag]].itemName);
            }

            //Display item name when hovering over it
            if (hoveringOverIndex > -1 && itemSlots[hoveringOverIndex] > -1 && itemIndexToDrag < 0)
                GUI.Box(new Rect(interactableSystem.mousePosition.x, Screen.height - interactableSystem.mousePosition.y - 30, 100, 25), availableItems[itemSlots[hoveringOverIndex]].itemName);

            if (!showInventory)
            {
                //Pick up message
                if (!detectedItem) return;
                GUI.color = new Color(0, 0, 0, 0.84f);
                GUI.Label(new Rect(Screen.width / 2 - 75 + 1, Screen.height / 2 - 50 + 1, 150, 20), $"Press 'E' to pick '{detectedItem.itemName}'");
                GUI.color = Color.green;
                GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 50, 150, 20), $"Press 'E' to pick '{detectedItem.itemName}'");
            }
        }
    }
}
