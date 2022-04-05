#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.Assets.cs © SilentWolf6662 - All Rights Reserved
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

using UnityEngine;

#endregion
namespace UBER
{

    /*
     * Global Asset references
     * Edit Asset references in the prefab CodeMonkey/Resources/CodeMonkeyAssets
     * */
    public class Assets : MonoBehaviour
    {

        // Internal instance reference
        private static Assets _i;


        // All references

        public Sprite s_White;
        public Sprite s_Circle;

        public Material m_White;

        // Instance reference
        public static Assets i
        {
            get
            {
                if (_i == null) _i = Instantiate(Resources.Load<Assets>("CodeMonkeyAssets"));
                return _i;
            }
        }
    }

}
