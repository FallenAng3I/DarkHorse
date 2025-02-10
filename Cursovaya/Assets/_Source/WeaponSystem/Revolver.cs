using UnityEngine;

namespace WeaponSystem
{
    public class Revolver : MonoBehaviour, IWeapon
    {
        public GameObject bulletPrefab;
        public Transform shootPoint;
        public int ammo = 10000;
        public float damage = 25f;
        public float bulletSpeed = 100f;

        private void OnEnable()
        {
            PlayerSystem.InputListener.OnAttack += Attack;
        }

        private void OnDisable()
        {
            PlayerSystem.InputListener.OnAttack -= Attack;
        }

        public void Attack()
        {
            if (ammo <= 0)
            {
                Debug.Log("Нет патронов!");
                return;
            }

            ammo--;

            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

            // Проверяем Rigidbody компонента пули
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

            // Строго по оси X, без отклонений
            Vector3 direction = shootPoint.right; // Пуля будет лететь только по X

            // Устанавливаем скорость пули
            bulletRigidbody.velocity = direction * bulletSpeed;

            Debug.Log($"Выстрел из револьвера! Осталось патронов: {ammo}");
        }
    }
}