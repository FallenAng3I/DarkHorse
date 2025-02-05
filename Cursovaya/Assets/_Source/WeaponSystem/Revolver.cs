using UnityEngine;

namespace WeaponSystem
{
    public class Revolver : MonoBehaviour, IWeapon
    {
        public GameObject bulletPrefab;
        public Transform shootPoint;
        public int ammo = 6;
        public float bulletSpeed = 10f;

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
            bullet.GetComponent<Rigidbody>().velocity = shootPoint.right * bulletSpeed;

            Debug.Log("Выстрел из револьвера!");
            
            //PlayerMovement.Instance.StopForSeconds(1f);
        }
    }
}