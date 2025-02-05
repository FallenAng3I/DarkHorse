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
            if (!canAttack) return;

            canAttack = false;
            Debug.Log("Топор ударил по области!");
            
            //Movement.Instance.StopForSeconds(1f);
            
            Invoke(nameof(ResetAttack), attackCooldown);
        }

        private void ResetAttack()
        {
            canAttack = true;
        }
    }
}