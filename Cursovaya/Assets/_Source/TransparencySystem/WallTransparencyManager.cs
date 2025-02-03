using System.Collections;
using UnityEngine;

public class WallTransparencyManager : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer; // ����, �� ������� ����� �����
    [SerializeField] private float transparentAlpha = 0.3f; // ��������� ���������� ���������� �����
    [SerializeField] private float fadeSpeed = 2f; // �������� ��������� ������������
    [SerializeField] private Transform player;

    private Coroutine fadeCoroutine;

    private void Update()
    {
        if (player == null) return;

        // ����� ��� ������� � ���� walls
        Collider[] colliders = Physics.OverlapSphere(player.position, 10f, wallLayer);

        // ���� ����� �� ������ �� ��� Z
        foreach (var collider in colliders)
        {
            if (collider.transform.position.z < player.position.z)
            {
                SetTransparency(collider, transparentAlpha);
            }
            else
            {
                SetTransparency(collider, 1f); // ������ ��������������
            }
        }
    }

    private void SetTransparency(Collider collider, float targetAlpha)
    {
        Material material = collider.GetComponent<Renderer>().material;
        if (material == null) return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeTransparency(material, targetAlpha));
    }

    private IEnumerator FadeTransparency(Material material, float targetAlpha)
    {
        float currentAlpha = material.GetFloat("_Transparency");

        while (!Mathf.Approximately(currentAlpha, targetAlpha))
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
            material.SetFloat("_Transparency", currentAlpha);
            yield return null;
        }
    }
}