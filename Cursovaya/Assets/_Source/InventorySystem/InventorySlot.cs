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
        public float cooldown;
    
        private int currentAmount;
        private float lastUseTime;

        public int CurrentAmount => currentAmount;

        public InventorySlot(string name, KeyCode key, int max, float cd)
        {
            slotName = name;
            assignedKey = key;
            maxAmount = max;
            cooldown = cd;
            currentAmount = 0;
            lastUseTime = -cd; // Позволяет сразу использовать при старте
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
    }
}