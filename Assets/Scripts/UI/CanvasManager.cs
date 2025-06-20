using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    public Slider healthSlider;
    public Slider staminaSlider;

    public TextMeshProUGUI statusMessageText;
    public float messageDuration    = 1f;
    public float messageOffsetY     = 30f;
    public float appearTime         = 0.3f;
    public float disappearTime      = 0.3f;

    private RectTransform rt;
    private Vector2 defaultPos;
    private Coroutine statusCoroutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // RectTransform, 기준 위치 한 번만 저장
        rt = statusMessageText.rectTransform;
        defaultPos = rt.anchoredPosition;

        statusMessageText.gameObject.SetActive(false);
    }

    public void UpdateHealthUI(float current, float max)
    {
        healthSlider.value = current / max;
    }

    public void UpdateStaminaUI(float current, float max)
    {
        staminaSlider.value = current / max;
    }

    public void ShowStatusMessage(string message)
    {
        if (statusCoroutine != null) StopCoroutine(statusCoroutine);
        statusCoroutine = StartCoroutine(StatusMessageRoutine(message));
    }

    private IEnumerator StatusMessageRoutine(string message)
    {
        statusMessageText.text = message;
        statusMessageText.alpha = 0;
        statusMessageText.gameObject.SetActive(true);

        Vector2 startPos = defaultPos + Vector2.down * messageOffsetY;

        // 등장
        float elapsed = 0f;
        while (elapsed < appearTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / appearTime;
            rt.anchoredPosition    = Vector2.Lerp(startPos, defaultPos, t);
            statusMessageText.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }

        yield return new WaitForSeconds(messageDuration);

        // 사라짐
        elapsed = 0f;
        while (elapsed < disappearTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / disappearTime;
            rt.anchoredPosition    = Vector2.Lerp(defaultPos, startPos, t);
            statusMessageText.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }

        rt.anchoredPosition = defaultPos;
        statusMessageText.gameObject.SetActive(false);
    }
}
