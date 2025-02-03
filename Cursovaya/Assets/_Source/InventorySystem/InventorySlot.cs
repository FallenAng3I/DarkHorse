using System;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class InventorySlot
    {
        public string slotName;
        public KeyCode assignedKey;
        public int maxAmount;
        [SerializeField] private int currentAmount;
        public float cooldown;
    
        private float lastUseTime;

        public int CurrentAmount => currentAmount;

        public InventorySlot(string name, KeyCode key, int max, float cd, int startAmount = 0)
        {
            slotName = name;
            assignedKey = key;
            maxAmount = max;
            cooldown = cd;
            currentAmount = Mathf.Clamp(startAmount, 0, maxAmount);
            lastUseTime = -cd; 
        }

        public bool CanUse()
        {
            return currentAmount > 0 && Time.time >= lastUseTime + cooldown;
        }

        public void Use()
        {
            if (CanUse())
            {
                currentAmount--;
                lastUseTime = Time.time;
                Debug.Log($"{slotName} использован! Осталось: {currentAmount}");
            }
            else
            {
                Debug.Log($"{slotName} нельзя использовать сейчас!");
            }
        }

        public bool AddItem(int amount)
        {
            if (currentAmount < maxAmount)
            {
                currentAmount = Mathf.Clamp(currentAmount + amount, 0, maxAmount);
                return true;
            }
            return false;
        }

        public void SetAmount(int amount)
        {
            currentAmount = Mathf.Clamp(amount, 0, maxAmount);
        }
    }
}