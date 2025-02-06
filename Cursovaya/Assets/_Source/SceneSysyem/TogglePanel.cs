using UnityEngine;

public class TogglePanel : MonoBehaviour
{
    public GameObject panel; // Ссылка на панель

    void Update()
    {
        // Проверяем, нажата ли кнопка Tab
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Переключаем активность панели
            panel.SetActive(!panel.activeSelf);
        }
    }
}