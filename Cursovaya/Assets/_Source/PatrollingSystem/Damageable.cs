using UnityEngine;

namespace PatrollingSystem
{
    public class Damageable : MonoBehaviour
    {
        public int health = 100;

        public void TakeDamage(int damage)
        {
            health -= damage;
            Debug.Log($"{gameObject.name} ������� {damage} �����. ��������: {health}");
            if (health <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            Debug.Log($"{gameObject.name} ���������!");
            Destroy(gameObject); // ���������� ������
        }
    }
}
