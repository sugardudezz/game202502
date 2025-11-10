using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameStateAnimation : MonoBehaviour
{
    [Header("UI Control")]
    public CanvasGroup uiBlockerGroup; // Canvas의 Canvas Group 컴포넌트
    public GameObject barContainer;    // LeftBar와 RightBar의 부모 오브젝트
    public Image leftBar;
    public Image rightBar;

    [Header("Start Text Components")] // 계획 텍스트 (씬 시작 시 사용)
    public GameObject text_left;
    public GameObject text_right;
    public TextMeshProUGUI lefttext;
    public TextMeshProUGUI righttext;

    private GameObject currentText1Object;
    private GameObject currentText2Object;
    private TextMeshProUGUI currentText1;
    private TextMeshProUGUI currentText2;

    // 애니메이션 설정
    [Header("Animation Settings")]
    public float barAppearDuration = 1.0f;       // Bar가 나타나 중앙에서 만나는 시간
    public float barDecreaseDuration = 1.5f;     // Bar 감소 및 텍스트 페이드 시간
    public float targetFillAmount = 1.0f;        // Bar 최종 채움 비율 (전체 채움: 1.0f)

    [Header("Timing Adjustments")]
    public float delayBeforeText = 0.5f;         // Bar 완료 후 텍스트 시작까지의 대기 시간
    public float letterAppearDelay = 0.2f;       // 글자 사이의 간격


    void Start()
    {
        // 씬 시작 시 초기화 (Fill Amount 0, Bar/Text 비활성화 등)
        if (barContainer != null) barContainer.SetActive(true);
        if (leftBar != null) leftBar.fillAmount = 0f;
        if (rightBar != null) rightBar.fillAmount = 0f;

        if (text_left != null) text_left.SetActive(false);
        if (text_right != null) text_right.SetActive(false);

        // UI 상호작용 비활성화
        LockUI();

        // 씬 시작 시 계획 애니메이션 호출
        PlayPlanSequence();

        float totalInitialDuration = barAppearDuration + delayBeforeText + 0.5f + barDecreaseDuration;
        StartCoroutine(UnlockAfterInitialStart(totalInitialDuration + 0.5f));
    }
    
    private IEnumerator UnlockAfterInitialStart(float duration) // Scene 시작 시에만 호출되는 UI 잠금 해제 함수
    {
        yield return new WaitForSeconds(duration);
        UnlockUI();
    }

    public void LockUI()
    {
        if (uiBlockerGroup != null)
        {
            // UI 상호작용 차단 (화면은 유지)
            uiBlockerGroup.blocksRaycasts = false;
            uiBlockerGroup.interactable = true;
        }
    }

    public void UnlockUI()
    {
        if (uiBlockerGroup != null)
        {
            // UI 상호작용 재활성화
            uiBlockerGroup.blocksRaycasts = true;
            uiBlockerGroup.interactable = true;
        }
    }

    public void PlayPlanSequence()
    {
        PlayGameSequence(text_left, text_right, lefttext, righttext);
    }

    public void PlayGameSequence(GameObject text1Obj, GameObject text2Obj, TextMeshProUGUI text1Comp, TextMeshProUGUI text2Comp)
    {
        // 현재 애니메이션에 사용될 텍스트 변수 업데이트
        currentText1Object = text1Obj;
        currentText2Object = text2Obj;
        currentText1 = text1Comp;
        currentText2 = text2Comp;

        // 기존에 실행 중이던 코루틴이 있다면 중지하고 새로 시작
        StopAllCoroutines();
        StartCoroutine(RunGameSequence());
    }

    IEnumerator RunGameSequence()
    {
        // Bar 컨테이너 활성화 및 Fill Amount 0 초기화
        if (barContainer != null) barContainer.SetActive(true);
        if (leftBar != null) leftBar.fillAmount = 0f;
        if (rightBar != null) rightBar.fillAmount = 0f;

        // 텍스트 색상을 초기 불투명 상태(alpha 1)로 되돌림
        if (currentText1 != null) currentText1.color = new Color(currentText1.color.r, currentText1.color.g, currentText1.color.b, 1f);
        if (currentText2 != null) currentText2.color = new Color(currentText2.color.r, currentText2.color.g, currentText2.color.b, 1f);


        // 1. Bar 나타나기 애니메이션
        yield return StartCoroutine(AnimateBars(targetFillAmount, barAppearDuration));

        // Bar 완료 후 텍스트 페이트 인까지 대기
        yield return new WaitForSeconds(delayBeforeText);

        // 2. 글자별 활성화 효과
        yield return StartCoroutine(AnimateLetterByLetter());

        // 3. Bar 감소 및 텍스트 페이드 아웃
        StartCoroutine(AnimateBars(0f, barDecreaseDuration));
        yield return StartCoroutine(FadeOutTwoTexts(barDecreaseDuration));

        // 4. 애니메이션 종료 후 오브젝트 비활성화 (화면에서 제거)
        if (currentText1Object != null) currentText1Object.SetActive(false);
        if (currentText2Object != null) currentText2Object.SetActive(false);
        if (barContainer != null) barContainer.SetActive(false); // Bar 컨테이너 비활성화
    }

    // Bar의 Fill Amount 애니메이션
    IEnumerator AnimateBars(float targetFill, float duration)
    {
        float startTime = Time.time;
        float startFillL = leftBar.fillAmount;
        float startFillR = rightBar.fillAmount;

        while (Time.time < startTime + duration)
        {
            float elapsed = Time.time - startTime;
            float t = elapsed / duration;

            leftBar.fillAmount = Mathf.Lerp(startFillL, targetFill, t);
            rightBar.fillAmount = Mathf.Lerp(startFillR, targetFill, t);

            yield return null;
        }
        leftBar.fillAmount = targetFill;
        rightBar.fillAmount = targetFill;
    }

    // 현재 설정된 두 글자 오브젝트를 순차적으로 활성화
    IEnumerator AnimateLetterByLetter()
    {
        if (currentText1Object != null) currentText1Object.SetActive(true);
        yield return new WaitForSeconds(letterAppearDelay);

        if (currentText2Object != null) currentText2Object.SetActive(true);
        yield return new WaitForSeconds(0.5f);
    }

    // 현재 설정된 두 텍스트의 알파값을 동시에 0으로 설정
    IEnumerator FadeOutTwoTexts(float duration)
    {
        // currentText 변수를 사용하여 동적으로 색상 참조
        Color startColor1 = currentText1.color;
        Color startColor2 = currentText2.color;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float elapsed = Time.time - startTime;
            float t = elapsed / duration;

            float alpha = Mathf.Lerp(1f, 0f, t);

            currentText1.color = new Color(startColor1.r, startColor1.g, startColor1.b, alpha);
            currentText2.color = new Color(startColor2.r, startColor2.g, startColor2.b, alpha);

            yield return null;
        }

        currentText1.color = new Color(startColor1.r, startColor1.g, startColor1.b, 0f);
        currentText2.color = new Color(startColor2.r, startColor2.g, startColor2.b, 0f);
    }
}