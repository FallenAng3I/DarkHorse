using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarImage; // Ссылка на Image для полоски здоровья
    public TMP_Text healthText;      // Ссылка на Text для отображения цифр

    private float maxHealth = 100f; // Максимальное здоровье
    private float currentHealth;    // Текущее здоровье

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    // Метод для изменения здоровья
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    // Метод для обновления UI
    private void UpdateHealthBar()
    {
        // Обновляем заполнение полоски здоровья
        healthBarImage.fillAmount = currentHealth / maxHealth;

        // Обновляем текст с текущим здоровьем
        healthText.text = $"{currentHealth}/{maxHealth}";      
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }
}