#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.Extension.cs © SilentWolf6662 - All Rights Reserved
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

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

#endregion
namespace UBER.Util
{
    public static class Extension
    {
        public static Vector2 MouseToWorldPosition(Mouse mouse)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldPosition = CacheBehaviour2D.mainCamera.ScreenToWorldPoint(mousePosition);
            return worldPosition;
        }
        public static IEnumerator FadeIn(Material material, float fadePause, float fadeSpeed)
        {
            // fade in
            yield return Fade(material, 1, fadeSpeed);
            yield return new WaitForSeconds(fadePause);
        }

        public static IEnumerator FadeOut(Material material, float fadePause, float fadeSpeed)
        {
            // fade in
            yield return Fade(material, 0, fadeSpeed);
            yield return new WaitForSeconds(fadePause);
        }

        public static IEnumerator Fade(Material mat, float targetAlpha, float fadeSpeed)
        {
            while (mat.color.a != targetAlpha)
            {
                float newAlpha = Mathf.MoveTowards(mat.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, newAlpha);
                yield return null;
            }
        }

        // Generate random normalized direction
        public static Vector3 GetRandomDir() => new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
