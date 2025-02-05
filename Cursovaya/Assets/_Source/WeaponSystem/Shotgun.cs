using UnityEngine;

namespace WeaponSystem
{
    public class Shotgun : MonoBehaviour, IWeapon
    {
        public GameObject bulletPrefab;
        public Transform shootPoint;
        public int bulletCount = 4;
        public float bulletSpeed = 8f;
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
            for (int i = 0; i < bulletCount; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

                // Расчёт угла разброса
                float angleOffset = Random.Range(-spreadAngle, spreadAngle);
                Quaternion rotation = Quaternion.Euler(0, 0, angleOffset);
                bullet.GetComponent<Rigidbody>().velocity = rotation * shootPoint.right * bulletSpeed;
            }

            Debug.Log("Выстрел из дробовика!");
            
            //PlayerMovement.Instance.StopForSeconds(1f);
        }
    }
}