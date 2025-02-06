using UnityEngine;

public class TogglePanel : MonoBehaviour
{
    public GameObject panel; // ������ �� ������

    void Update()
    {
        // ���������, ������ �� ������ Tab
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ����������� ���������� ������
            panel.SetActive(!panel.activeSelf);
        }
    }
}