using UnityEngine;

namespace PatrollingSystem
{
    public class Damageable : MonoBehaviour
    {
        public int health = 100;

        public void TakeDamage(int damage)
        {
            health -= damage;
            Debug.Log($"{gameObject.name} получил {damage} урона. «доровье: {health}");
            if (health <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            Debug.Log($"{gameObject.name} уничтожен!");
            Destroy(gameObject); // ”ничтожаем объект
        }
    }
}
