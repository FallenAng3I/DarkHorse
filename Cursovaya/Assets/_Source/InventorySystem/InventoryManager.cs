using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        public List<InventorySlot> inventorySlots = new List<InventorySlot>();
        public List<Weapon> weapons = new List<Weapon>();
        public Transform weaponHolder; // Точка, где появляется модель оружия
        private int currentWeaponIndex = 0;
        private GameObject currentWeaponModel;

        private void Start()
        {
            if (weapons.Count > 0)
            {
                EquipWeapon(0); // При старте дать первое оружие
            }
        }

        private void Update()
        {
            ReadInput();
            ScrollWeapons();
        }

        private void ReadInput()
        {
            foreach (var slot in inventorySlots)
            {
                if (Input.GetKeyDown(slot.assignedKey))
                {
                    slot.Use();
                }
            }
        }

        private void ScrollWeapons()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                ChangeWeapon(1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                ChangeWeapon(-1);
            }
        }

        private void ChangeWeapon(int direction)
        {
            if (weapons.Count == 0) return;

            currentWeaponIndex += direction;
            if (currentWeaponIndex >= weapons.Count) currentWeaponIndex = 0;
            if (currentWeaponIndex < 0) currentWeaponIndex = weapons.Count - 1;

            EquipWeapon(currentWeaponIndex);
        }

        private void EquipWeapon(int index)
        {
            if (weapons.Count == 0) return;

            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }

            Weapon weapon = weapons[index];

            if (weapon.weaponModel != null)
            {
                currentWeaponModel = Instantiate(weapon.weaponModel, weaponHolder.position, weaponHolder.rotation, weaponHolder);
            }
        
            Debug.Log($"Выбрано оружие: {weapon.weaponName}");
        }

        public void AddWeapon(Weapon newWeapon)
        {
            weapons.Add(newWeapon);
            if (weapons.Count == 1)
            {
                EquipWeapon(0);
            }
        }
    }
}
