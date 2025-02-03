using UnityEngine;

namespace PlayerSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 5f;
    
        private Vector3 _moveDirection = Vector3.zero;
        private KeyCode _lastKeyPressed = KeyCode.None;

        private void OnEnable()
        {
            InputListener.OnMoveKeyPressed += HandleKeyPress;
            InputListener.OnMoveKeyReleased += HandleKeyRelease;
        }

        private void OnDisable()
        {
            InputListener.OnMoveKeyPressed -= HandleKeyPress;
            InputListener.OnMoveKeyReleased -= HandleKeyRelease;
        }

        private void Update()
        {
            Move();
        }

        private void HandleKeyPress(KeyCode key)
        {
            _lastKeyPressed = key;
        }

        private void HandleKeyRelease(KeyCode key)
        {
            if (key == _lastKeyPressed)
            {
                _lastKeyPressed = GetLastPressedKey();
            }
        }

        private void Move()
        {
            _moveDirection = Vector3.zero;

            switch (_lastKeyPressed)
            {
                case KeyCode.W:
                    _moveDirection = Vector3.forward;
                    break;
                case KeyCode.S:
                    _moveDirection = Vector3.back;
                    break;
                case KeyCode.A:
                    _moveDirection = Vector3.left;
                    break;
                case KeyCode.D:
                    _moveDirection = Vector3.right;
                    break;
            }

            transform.position += _moveDirection * (moveSpeed * Time.deltaTime);
        }

        private KeyCode GetLastPressedKey()
        {
            if (Input.GetKey(KeyCode.W)) return KeyCode.W;
            if (Input.GetKey(KeyCode.S)) return KeyCode.S;
            if (Input.GetKey(KeyCode.A)) return KeyCode.A;
            if (Input.GetKey(KeyCode.D)) return KeyCode.D;
            return KeyCode.None;
        }
    }
}