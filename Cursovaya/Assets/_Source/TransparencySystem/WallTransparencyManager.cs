using System.Collections;
using UnityEngine;

public class WallTransparencyManager : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer; // Слой, на котором будут стены
    [SerializeField] private float transparentAlpha = 0.3f; // Насколько прозрачной становится стена
    [SerializeField] private float fadeSpeed = 2f; // Скорость изменения прозрачности
    [SerializeField] private Transform player;

    private Coroutine fadeCoroutine;

    private void Update()
    {
        if (player == null) return;

        // Найти все объекты в слое walls
        Collider[] colliders = Physics.OverlapSphere(player.position, 10f, wallLayer);

        // Если игрок за стеной по оси Z
        foreach (var collider in colliders)
        {
            if (collider.transform.position.z < player.position.z)
            {
                SetTransparency(collider, transparentAlpha);
            }
            else
            {
                SetTransparency(collider, 1f); // Ставим непрозрачность
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