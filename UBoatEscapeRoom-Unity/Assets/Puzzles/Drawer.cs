#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.Drawer.cs © SilentWolf6662 - All Rights Reserved
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
using UBER.Core.Interaction;
using UBER.Util;
using UnityEngine;

#endregion
namespace UBER.Puzzles
{
    public class Drawer : CacheBehaviour2D, IInteractable
    {
        [SerializeField] private CanvasGroup drawerUI;

        // fade speed length
        [SerializeField] private float fadeSpeed;

        //Pause length between fades
        [SerializeField] private int fadePause;

        public void Activate() => throw new NotImplementedException();

        public void OpenUI() => StartCoroutine(Extension.FadeIn(drawerUI.GetComponent<Renderer>().material, fadePause, fadeSpeed));
    }
}
