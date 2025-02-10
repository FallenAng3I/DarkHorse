using UnityEngine;

namespace WeaponSystem
{
    public class Shotgun : MonoBehaviour, IWeapon
    {
        public GameObject bulletPrefab;
        public Transform shootPoint;
        public int ammo;
        public float damage = 25f;
        public int bulletCount = 4;
        public float bulletSpeed = 50f;
        public float spreadAngle = 10f;

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

            for (int i = 0; i < bulletCount; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

                // Разброс по X (вправо) и Y (вверх/вниз)
                float xOffset = Random.Range(0, spreadAngle);  // X всегда положительный
                float yOffset = Random.Range(-spreadAngle, spreadAngle); // Y вверх/вниз

                Vector3 direction = shootPoint.forward + new Vector3(xOffset, yOffset, 0); // Двигается вперёд + разброс
                bullet.GetComponent<Rigidbody>().velocity = direction.normalized * bulletSpeed;
            }

            Debug.Log($"Выстрел из дробовика! Осталось патронов: {ammo}");
        }
    }
}