using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class BattleStateAnimation : MonoBehaviour
{
    public GameStateAnimation animator;

    [Header("Text Components")]
    // 격돌 텍스트
    public GameObject text_left;
    public GameObject text_right;
    public TextMeshProUGUI lefttext;
    public TextMeshProUGUI righttext;

    public float restartDelay = 5.0f; // 임시로 설정한 격돌 후 다시 계획 애니메이션 까지의 간격

    [Header("Behaviour Objects")]
    public GameObject[] objectsB;

    private Button buttonComponent; // 격돌 버튼

    void Awake()
    {
        // 이 스크립트가 부착된 오브젝트에서 Button 컴포넌트를 가져옵니다.
        buttonComponent = GetComponent<Button>();
    }

    public void OnClickBattleButton()
    {
        if (animator != null)
        {
            if (objectsB != null)
            {
                foreach (GameObject go in objectsB)
                {
                    // 각 행동 패널 오브젝트가 null이 아니고 활성화되어 있다면 비활성화
                    if (go != null && go.activeInHierarchy)
                    {
                        go.SetActive(false);
                    }
                }
            }

            animator.LockUI();

            if (buttonComponent != null) // 격돌 버튼 상호작용 차단
            {
                buttonComponent.interactable = false;
            }
            StartCoroutine(RunBattleSequence());
        }
    }

    private IEnumerator RunBattleSequence()
    {
        // 1. 격돌 애니메이션 실행
        animator.PlayGameSequence(text_left, text_right, lefttext, righttext);

        // 2. 애니메이션 완료 대기
        float totalAnimationDuration = animator.barAppearDuration
                                     + animator.delayBeforeText
                                     + 0.5f // 글자 활성화 후 대기 시간
                                     + animator.barDecreaseDuration;
        yield return new WaitForSeconds(totalAnimationDuration + 0.5f);

        // 3. 5초 대기 (임시 설정, 실제 격돌 시의 스크립트 진행 후 4번 코드 실행만 하면 됨. 현재는 없으므로 몇초 대기로 설정)
        yield return new WaitForSeconds(restartDelay);

        // 4. 계획 애니메이션 재호출
        animator.PlayPlanSequence();

        // 5. 계획 애니메이션 완료 대기 (재호출된 시퀀스가 끝날 때까지 기다림) - 격돌 버튼 재활성화를 위해 필요
        float totalStartAnimationDuration = animator.barAppearDuration
                                         + animator.delayBeforeText
                                         + 0.5f
                                         + animator.barDecreaseDuration;
        yield return new WaitForSeconds(totalStartAnimationDuration + 0.5f);

        // 6. 격돌 버튼 및 UI 상호작용 재활성화
        animator.UnlockUI();
        if (buttonComponent != null)
        {
            buttonComponent.interactable = true;
        }
    }
}