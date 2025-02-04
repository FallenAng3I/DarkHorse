using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string weaponName;
    public GameObject weaponModel; // Модель оружия
    public float fireRate;
    public int damage;
    public int maxAmmo;
    public int currentAmmo;

    public Weapon(string name, GameObject model, float rate, int dmg, int max)
    {
        weaponName = name;
        weaponModel = model;
        fireRate = rate;
        damage = dmg;
        maxAmmo = max;
        currentAmmo = max;
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
        Debug.Log($"{weaponName} перезаряжено!");
    }

    public bool CanShoot()
    {
        return currentAmmo > 0;
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            currentAmmo--;
            Debug.Log($"Выстрел из {weaponName}! Осталось патронов: {currentAmmo}");
        }
        else
        {
            Debug.Log($"{weaponName} без патрон!");
        }
    }
}