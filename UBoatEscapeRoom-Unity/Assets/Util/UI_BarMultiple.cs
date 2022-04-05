#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.Player.UI_BarMultiple.cs © SilentWolf6662 - All Rights Reserved
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

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

#endregion
namespace UBER.Utils
{

    /*
     * UI Container with multiple bars, useful for displaying one bar with multiple inner bars like success chance and failure chance
     * */
    public class UI_BarMultiple
    {
        private readonly RectTransform[] barArr;
        private readonly Vector2 size;
        private Image[] barImageArr;

        private GameObject gameObject;
        private RectTransform rectTransform;

        public UI_BarMultiple(Transform parent, Vector2 anchoredPosition, Vector2 size, Color[] barColorArr, Outline outline)
        {
            this.size = size;
            SetupParent(parent, anchoredPosition, size);
            if (outline != null) SetupOutline(outline, size);
            List<RectTransform> barList = new List<RectTransform>();
            List<Image> barImageList = new List<Image>();
            List<float> defaultSizeList = new List<float>();
            foreach (Color color in barColorArr)
            {
                barList.Add(SetupBar(color));
                defaultSizeList.Add(1f / barColorArr.Length);
            }
            barArr = barList.ToArray();
            barImageArr = barImageList.ToArray();
            SetSizes(defaultSizeList.ToArray());
        }
        private void SetupParent(Transform parent, Vector2 anchoredPosition, Vector2 size)
        {
            gameObject = new GameObject("UI_BarMultiple", typeof(RectTransform));
            rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            rectTransform.sizeDelta = size;
            rectTransform.anchorMin = new Vector2(0, .5f);
            rectTransform.anchorMax = new Vector2(0, .5f);
            rectTransform.pivot = new Vector2(0, .5f);
            rectTransform.anchoredPosition = anchoredPosition;
        }
        private void SetupOutline(Outline outline, Vector2 size) => UtilsClass.DrawSprite(outline.color, gameObject.transform, Vector2.zero, size + new Vector2(outline.size, outline.size), "Outline");
        private RectTransform SetupBar(Color barColor)
        {
            RectTransform bar = UtilsClass.DrawSprite(barColor, gameObject.transform, Vector2.zero, Vector2.zero, "Bar");
            bar.anchorMin = new Vector2(0, 0);
            bar.anchorMax = new Vector2(0, 1f);
            bar.pivot = new Vector2(0, .5f);
            return bar;
        }
        public void SetSizes(float[] sizeArr)
        {
            if (sizeArr.Length != barArr.Length)
                throw new Exception("Length doesn't match!");
            Vector2 pos = Vector2.zero;
            for (int i = 0; i < sizeArr.Length; i++)
            {
                float scaledSize = sizeArr[i] * size.x;
                barArr[i].anchoredPosition = pos;
                barArr[i].sizeDelta = new Vector2(scaledSize, 0f);
                pos.x += scaledSize;
            }
        }
        public Vector2 GetSize() => size;
        public void DestroySelf() => Object.Destroy(gameObject);

        public class Outline
        {
            public Color color = Color.black;
            public float size = 1f;
            public Outline(float size, Color color)
            {
                this.size = size;
                this.color = color;
            }
        }
    }
}
