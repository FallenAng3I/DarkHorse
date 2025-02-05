using UnityEngine;

namespace PlayerSystem
{
    public class HealthView : MonoBehaviour
    {
        private Player player;
        private int health;

        public void TakeDamage(int damage)
        {
            health = player.health;
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
