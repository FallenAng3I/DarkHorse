using UnityEngine;

namespace WeaponSystem
{
    public class WeaponSwitcher : MonoBehaviour
    {
        public GameObject[] weapons; // Массив оружия (объектов)
        private int _currentWeaponIndex = 0;

        private void Start()
        {
            UpdateWeaponState();
        }

        private void Update()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel"); //TODO Занести ввод в Input Listener
            if (scroll > 0f) 
            {
                _currentWeaponIndex = (_currentWeaponIndex + 1) % weapons.Length;
                UpdateWeaponState();
            }
            else if (scroll < 0f) 
            {
                _currentWeaponIndex = (_currentWeaponIndex - 1 + weapons.Length) % weapons.Length;
                UpdateWeaponState();
            }
        }

        private void UpdateWeaponState()
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].SetActive(i == _currentWeaponIndex);
            }
        }
    }
}