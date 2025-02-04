using PlayerSystem;
using UnityEngine;
using UnityEngine.UI;
namespace BarsSystem
{
    public class HealthBar : MonoBehaviour
    {
        public Image healthBar;
        public Image staminaBar;
        public Text healthText;
        public Text staminaText;

        private Player playerStats;

        public void Initialize(Player stats)
        {
            playerStats = stats;
            UpdateHealthBar();
            UpdateStaminaBar();
        }

        public void UpdateHealthBar()
        {
            //healthBar.fillAmount = playerStats.currentHealth / playerStats.maxHealth;
           // healthText.text = $"{playerStats.currentHealth}/{playerStats.maxHealth}";
        }

        public void UpdateStaminaBar()
        {
           // staminaBar.fillAmount = playerStats.currentStamina / playerStats.maxStamina;
           // staminaText.text = $"{playerStats.currentStamina}/{playerStats.maxStamina}";
        }
    }
