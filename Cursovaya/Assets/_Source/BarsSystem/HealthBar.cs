using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarImage; // ������ �� Image ��� ������� ��������
    public TMP_Text healthText;      // ������ �� Text ��� ����������� ����

    private float maxHealth = 100f; // ������������ ��������
    private float currentHealth;    // ������� ��������

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    // ����� ��� ��������� ��������
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    // ����� ��� ���������� UI
    private void UpdateHealthBar()
    {
        // ��������� ���������� ������� ��������
        healthBarImage.fillAmount = currentHealth / maxHealth;

        // ��������� ����� � ������� ���������
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