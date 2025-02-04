using UnityEngine;

namespace PlayerSystem
{
    public class HealthView : MonoBehaviour
    {
        public int health = 100;

        public void TakeDamage(int damage)
        {
            health -= damage;
            Debug.Log($"{gameObject.name} получил {damage} урона. Осталось: {health} хп.");
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log($"{gameObject.name} умэр!");
            Destroy(gameObject);
        }
    }
}
