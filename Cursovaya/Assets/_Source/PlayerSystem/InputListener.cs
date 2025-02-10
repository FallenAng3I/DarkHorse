using System;
using UnityEngine;

namespace PlayerSystem
{
    public class InputListener : MonoBehaviour
    {
        public static event Action<KeyCode> OnMoveKeyPressed;
        public static event Action<KeyCode> OnMoveKeyReleased;
        public static event Action OnCrouch;
        public static event Action OnSprint;
        public static event Action OnStand;
        public static event Action OnAttack = delegate { };

        private void Update()
        {
            ReadMovement();
            ReadActions();
            ReadAttack();
        }

        private void ReadMovement()
        {
            if (Input.GetKeyDown(KeyCode.W)) OnMoveKeyPressed?.Invoke(KeyCode.W);
            if (Input.GetKeyDown(KeyCode.S)) OnMoveKeyPressed?.Invoke(KeyCode.S);
            if (Input.GetKeyDown(KeyCode.A)) OnMoveKeyPressed?.Invoke(KeyCode.A);
            if (Input.GetKeyDown(KeyCode.D)) OnMoveKeyPressed?.Invoke(KeyCode.D);

            if (Input.GetKeyUp(KeyCode.W)) OnMoveKeyReleased?.Invoke(KeyCode.W);
            if (Input.GetKeyUp(KeyCode.S)) OnMoveKeyReleased?.Invoke(KeyCode.S);
            if (Input.GetKeyUp(KeyCode.A)) OnMoveKeyReleased?.Invoke(KeyCode.A);
            if (Input.GetKeyUp(KeyCode.D)) OnMoveKeyReleased?.Invoke(KeyCode.D);
        }

        private void ReadAttack()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnAttack?.Invoke();
            }
        }

        private void ReadActions()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl)) OnCrouch?.Invoke();
            if (Input.GetKeyUp(KeyCode.LeftControl)) OnStand?.Invoke();
            
            if (Input.GetKey(KeyCode.LeftShift)) OnSprint?.Invoke();
            if (Input.GetKeyUp(KeyCode.LeftShift)) OnStand?.Invoke();
        }
    }
}