using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScalePact.Core.Input
{
    public class InputManager : MonoBehaviour, InputActions.IPlayerActions
    {
        //Properties
        public Vector2 MovementVector { get; private set; }
        public bool IsAttacking { get; private set; }

        //Event handlers
        public event Action JumpEvent;
        public event Action InteractEvent;
        public event Action DodgeEvent;
        public event Action ToggleTargetEvent;
        public event Action SwitchTargetEvent;
        public event Action LightAttackEvent;

        InputActions inputActions;

        private void Start()
        {
            inputActions = new InputActions();
            inputActions.Player.SetCallbacks(this);
            inputActions.Player.Enable();
        }

        private void OnDestroy()
        {
            inputActions.Player.Disable();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            InteractEvent?.Invoke();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            JumpEvent?.Invoke();
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            DodgeEvent?.Invoke();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementVector = context.ReadValue<Vector2>();
        }

        public void OnCamera(InputAction.CallbackContext context)
        {

        }

        public void OnToggleTarget(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            ToggleTargetEvent?.Invoke();

        }

        public void OnLightAttack(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            LightAttackEvent?.Invoke();
        }
    }
}