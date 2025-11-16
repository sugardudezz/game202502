using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class HealthBarManager : MonoBehaviour
{
    public Image healthFillImage;
    public TextMeshProUGUI healthText;
    public float animationDuration = 0.5f; // 체력 감소 속도

    private float maxHealth = 100f;
    private float currentHealth;
    private Coroutine healthCoroutine; // 현재 다른 애니메이션 진행 확인 플래그

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void Damage(float damageValue)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        float targetHealth = currentHealth - damageValue;

        if (targetHealth < 0)
        {
            targetHealth = 0;
        }

        if (healthCoroutine != null) // 기존 애니메이션 정지
        {
            StopCoroutine(healthCoroutine);
        }

        healthCoroutine = StartCoroutine(LerpHealthBar(targetHealth));
    }

    IEnumerator LerpHealthBar(float targetHealth) // 체력바 감소 + 애니메이션 추가
    {
        float startHealth = currentHealth;
        float timeElapsed = 0f;

        float startFillAmount = healthFillImage.fillAmount;
        float targetFillAmount = targetHealth / maxHealth;

        while (timeElapsed < animationDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / animationDuration;

            t = Mathf.SmoothStep(0f, 1f, t);

            healthFillImage.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, t);

            currentHealth = Mathf.Lerp(startHealth, targetHealth, t);
            healthText.text = Mathf.RoundToInt(currentHealth).ToString() + " / " + Mathf.RoundToInt(maxHealth).ToString();

            yield return null;
        }

        currentHealth = targetHealth;
        UpdateHealthBar();

        healthCoroutine = null;
    }

    void UpdateHealthBar()
    {
        float fillRatio = currentHealth / maxHealth;

        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = fillRatio;
        }

        if (healthText != null)
        {
            healthText.text = Mathf.RoundToInt(currentHealth).ToString() + " / " + Mathf.RoundToInt(maxHealth).ToString();
        }
    }
}