using System;
using UnityEngine;

namespace PlayerSystem
{
    [Serializable]
    public class Player
    {
        public int health = 100;
        public float stamina = 100f;
        public float speed = 6f;
        
        public bool IsCrouching { get; private set; }
        public bool IsSprinting { get; private set; }
        
        
        public void Crouch()
        {
            IsCrouching = true;
            IsSprinting = false;
        }

        public void Sprint()
        {
            if (stamina > 0)
            {
                IsSprinting = true;
                IsCrouching = false;
            }
        }

        public void Stand()
        {
            IsCrouching = false;
            IsSprinting = false;
        }
    }
}