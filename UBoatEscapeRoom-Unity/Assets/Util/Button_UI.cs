#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.Button_UI.cs © SilentWolf6662 - All Rights Reserved
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
//#define SOUND_MANAGER // Has Sound_Manager in project
//#define CURSOR_MANAGER // Has Cursor_Manager in project

#region

using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#endregion
namespace UBER.Utils
{

    /*
     * Button in the UI
     * */
    public sealed class Button_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {

        public Action ClickFunc = null, MouseRightClickFunc = null, MouseMiddleClickFunc = null, MouseDownOnceFunc = null, MouseUpFunc = null, MouseOverOnceTooltipFunc = null, MouseOutOnceTooltipFunc = null, MouseOverOnceFunc = null, MouseOutOnceFunc = null, MouseOverFunc = null, MouseOverPerSecFunc = null, MouseUpdate = null;
        public Action<PointerEventData> OnPointerClickFunc;

        public enum HoverBehaviour
        {
            Custom,
            Change_Color,
            Change_Image,
            Change_SetActive
        }
        public HoverBehaviour hoverBehaviourType = HoverBehaviour.Custom;
        private Action hoverBehaviourFunc_Enter, hoverBehaviourFunc_Exit;

        public Color hoverBehaviour_Color_Enter, hoverBehaviour_Color_Exit;
        public Image hoverBehaviour_Image;
        public Sprite hoverBehaviour_Sprite_Exit, hoverBehaviour_Sprite_Enter;
        public bool hoverBehaviour_Move;
        public Vector2 hoverBehaviour_Move_Amount = Vector2.zero;
        private Vector2 posExit, posEnter;
        public bool triggerMouseOutFuncOnClick;
        private bool mouseOver;
        private float mouseOverPerSecFuncTimer;

        private Action internalOnPointerEnterFunc, internalOnPointerExitFunc, internalOnPointerClickFunc;

#if SOUND_MANAGER
        public Sound_Manager.Sound mouseOverSound, mouseClickSound;
#endif
#if CURSOR_MANAGER
        public CursorManager.CursorType cursorMouseOver, cursorMouseOut;
#endif


        public void OnPointerEnter(PointerEventData eventData)
        {
            internalOnPointerEnterFunc?.Invoke();
            if (hoverBehaviour_Move) transform.localPosition = posEnter;
            hoverBehaviourFunc_Enter?.Invoke();
            MouseOverOnceFunc?.Invoke();
            MouseOverOnceTooltipFunc?.Invoke();
            mouseOver = true;
            mouseOverPerSecFuncTimer = 0f;
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            internalOnPointerExitFunc?.Invoke();
            if (hoverBehaviour_Move) transform.localPosition = posExit;
            hoverBehaviourFunc_Exit?.Invoke();
            MouseOutOnceFunc?.Invoke();
            MouseOutOnceTooltipFunc?.Invoke();
            mouseOver = false;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            internalOnPointerClickFunc?.Invoke();
            OnPointerClickFunc?.Invoke(eventData);
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (triggerMouseOutFuncOnClick) OnPointerExit(eventData);
                ClickFunc?.Invoke();
            }
            if (eventData.button == PointerEventData.InputButton.Right) MouseRightClickFunc?.Invoke();
            if (eventData.button == PointerEventData.InputButton.Middle) MouseMiddleClickFunc?.Invoke();
        }
        public void Manual_OnPointerExit() => OnPointerExit(null);
        public bool IsMouseOver() => mouseOver;
        public void OnPointerDown(PointerEventData eventData) => MouseDownOnceFunc?.Invoke();
        public void OnPointerUp(PointerEventData eventData) => MouseUpFunc?.Invoke();

        private void Update()
        {
            if (mouseOver)
            {
                MouseOverFunc?.Invoke();
                mouseOverPerSecFuncTimer -= Time.unscaledDeltaTime;
                if (mouseOverPerSecFuncTimer <= 0)
                {
                    mouseOverPerSecFuncTimer += 1f;
                    MouseOverPerSecFunc?.Invoke();
                }
            }
            MouseUpdate?.Invoke();

        }
        private void Awake()
        {
            posExit = transform.localPosition;
            posEnter = (Vector2)transform.localPosition + hoverBehaviour_Move_Amount;
            SetHoverBehaviourType(hoverBehaviourType);

#if SOUND_MANAGER
            // Sound Manager
            internalOnPointerEnterFunc += () => { if (mouseOverSound != Sound_Manager.Sound.None) Sound_Manager.PlaySound(mouseOverSound); };
            internalOnPointerClickFunc += () => { if (mouseClickSound != Sound_Manager.Sound.None) Sound_Manager.PlaySound(mouseClickSound); };
#endif

#if CURSOR_MANAGER
            // Cursor Manager
            internalOnPointerEnterFunc += () => { if (cursorMouseOver != CursorManager.CursorType.None) CursorManager.SetCursor(cursorMouseOver); };
            internalOnPointerExitFunc += () => { if (cursorMouseOut != CursorManager.CursorType.None) CursorManager.SetCursor(cursorMouseOut); };
#endif
        }
        public void SetHoverBehaviourType(HoverBehaviour hoverBehaviourType)
        {
            this.hoverBehaviourType = hoverBehaviourType;
            switch (hoverBehaviourType)
            {
                case HoverBehaviour.Change_Color:
                    hoverBehaviourFunc_Enter = delegate { hoverBehaviour_Image.color = hoverBehaviour_Color_Enter; };
                    hoverBehaviourFunc_Exit = delegate { hoverBehaviour_Image.color = hoverBehaviour_Color_Exit; };
                    break;
                case HoverBehaviour.Change_Image:
                    hoverBehaviourFunc_Enter = delegate { hoverBehaviour_Image.sprite = hoverBehaviour_Sprite_Enter; };
                    hoverBehaviourFunc_Exit = delegate { hoverBehaviour_Image.sprite = hoverBehaviour_Sprite_Exit; };
                    break;
                case HoverBehaviour.Change_SetActive:
                    hoverBehaviourFunc_Enter = delegate { hoverBehaviour_Image.gameObject.SetActive(true); };
                    hoverBehaviourFunc_Exit = delegate { hoverBehaviour_Image.gameObject.SetActive(false); };
                    break;
            }
        }









        /*
         * Class for temporarily intercepting a button action
         * Useful for Tutorial disabling specific buttons
         * */
        public class InterceptActionHandler
        {

            private readonly Action removeInterceptFunc;

            public InterceptActionHandler(Action removeInterceptFunc) => this.removeInterceptFunc = removeInterceptFunc;
            public void RemoveIntercept() => removeInterceptFunc();
        }
        public InterceptActionHandler InterceptActionClick(Func<bool> testPassthroughFunc) => InterceptAction("ClickFunc", testPassthroughFunc);
        public InterceptActionHandler InterceptAction(string fieldName, Func<bool> testPassthroughFunc) => InterceptAction(GetType().GetField(fieldName), testPassthroughFunc);
        public InterceptActionHandler InterceptAction(FieldInfo fieldInfo, Func<bool> testPassthroughFunc)
        {
            Action backFunc = fieldInfo.GetValue(this) as Action;
            InterceptActionHandler interceptActionHandler = new InterceptActionHandler(() => fieldInfo.SetValue(this, backFunc));
            fieldInfo.SetValue(this,
                (Action)delegate
                {
                    if (testPassthroughFunc())
                    {
                        // Passthrough
                        interceptActionHandler.RemoveIntercept();
                        backFunc();
                    }
                });

            return interceptActionHandler;
        }
    }
}
