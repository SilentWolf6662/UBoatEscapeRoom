#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.Player.Movement.cs © SilentWolf6662 - All Rights Reserved
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
using UnityEngine.InputSystem;

#endregion
namespace UBER.Core.Player
{
    public class Movement : CacheBehaviour2D
    {
        [SerializeField] private float speed = 30;
        [SerializeField] private InputAction playerMovementAction;
        public bool canMove;
        private Vector2 moveInput;

        private void Update() => moveInput = playerMovementAction.ReadValue<Vector2>();

        private void FixedUpdate() => MovePlayer();

        private void OnEnable() => playerMovementAction.Enable();

        private void OnDisable() => playerMovementAction.Disable();

        private void MovePlayer()
        {
            if (!canMove) return;
            Flip(moveInput.x);
            rigidbody2D.velocity = moveInput * (speed * Time.fixedDeltaTime);
        }

        private void Flip(float directionInput)
        {
            if (directionInput < 0) FlipDegrees(160);
            else if (directionInput > 0) FlipDegrees(0);
        }

        private void FlipDegrees(float y, float x = 0, float z = 0, float w = 0) => transform.rotation = new Quaternion(x, y, z, w);
    }
}
