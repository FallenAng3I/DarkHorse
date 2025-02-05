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
    
        private int _currentAmount;
        private float _lastUseTime;

        public int CurrentAmount => _currentAmount;

        public InventorySlot(string name, KeyCode key, int max, float cd)
        {
            slotName = name;
            assignedKey = key;
            maxAmount = max;
            cooldown = cd;
            _currentAmount = 0;
            _lastUseTime = -cd; // Позволяет сразу использовать при старте
        }

        public bool CanUse()
        {
            return _currentAmount > 0 && Time.time >= _lastUseTime + cooldown;
        }

        public void Use()
        {
            if (CanUse())
            {
                _currentAmount--;
                _lastUseTime = Time.time;
                Debug.Log($"{slotName} использован! Осталось: {_currentAmount}");
            }
            else
            {
                Debug.Log($"{slotName} нельзя использовать сейчас!");
            }
        }

        public bool AddItem(int amount)
        {
            if (_currentAmount < maxAmount)
            {
                _currentAmount = Mathf.Clamp(_currentAmount + amount, 0, maxAmount);
                return true;
            }
            return false;
        }
    }
}