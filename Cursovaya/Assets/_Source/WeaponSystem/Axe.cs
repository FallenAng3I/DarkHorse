using PlayerSystem;
using UnityEngine;

namespace WeaponSystem
{
    public class Axe : MonoBehaviour, IWeapon
    {
        public float attackCooldown = 1.5f;
        private bool canAttack = true;

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
            Debug.Log("Топор атакует!");
            // Здесь твоя логика атаки
        }


        private void ResetAttack()
        {
            canAttack = true;
        }
    }
}