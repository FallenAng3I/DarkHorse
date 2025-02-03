using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        public List<InventorySlot> inventorySlots = new List<InventorySlot>();

        private void Update()
        {
            ReadInput();
        }

        private void ReadInput()
        {
            for (var index = 0; index < inventorySlots.Count; index++)
            {
                var slot = inventorySlots[index];
                if (Input.GetKeyDown(slot.assignedKey))
                {
                    slot.Use();
                }
            }
        }

        private InventorySlot GetSlot(string slotName)
        {
            return inventorySlots.Find(slot => slot.slotName == slotName);
        }

        public void AddItemToSlot(string slotName, int amount)
        {
            var slot = GetSlot(slotName);
            if (slot != null)
            {
                bool added = slot.AddItem(amount);
                Debug.Log(added
                    ? $"Добавлено {amount} в {slotName}. Текущее количество: {slot.CurrentAmount}"
                    : $"Слот {slotName} заполнен!");
            }
        }

        public void SetSlotAmount(string slotName, int amount)
        {
            var slot = GetSlot(slotName);
            if (slot != null)
            {
                slot.SetAmount(amount);
                Debug.Log($"Количество в {slotName} установлено на {amount}");
            }
        }
    }
}