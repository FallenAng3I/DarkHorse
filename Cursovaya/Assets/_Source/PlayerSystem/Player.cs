using UnityEngine;

namespace PlayerSystem
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 15;
        [SerializeField] private int health;
        public float stamina = 100f;
        public float speed = 6f;
        
        public bool IsCrouching { get; private set; }
        public bool IsSprinting { get; private set; }

        private void Awake()
        {
            health = maxHealth;
        }

        public void Crouch()
        {
            IsCrouching = true;
            IsSprinting = false;
        }
        
        public void TakeDamage(int damage)
        {
            health -= damage;
            //health = Mathf.Clamp(health, 0, maxHealth);
            
            if (health <= 0)
            {
                Die();
            }
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
        
        private void Die()
        {
            Debug.Log("Player died");
            Destroy(gameObject);
        }
    }
}