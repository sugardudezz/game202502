using UnityEngine;
using UnityEngine.UI; // UI 관련 작업을 위해 필요 (필수는 아니지만 관례상 추가)

public class ButtonScaler : MonoBehaviour
{
    private Vector3 originalScale = new Vector3(1f, 1f, 1f);
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f); // 인스펙터에서 설정 가능

    // Start에서 원래 크기를 저장합니다.
    void Start()
    {
        originalScale = transform.localScale;
    }

    // 마우스 진입 시 호출
    public void SetHoverScale()
    {
        transform.localScale = hoverScale;
    }

    // 마우스 이탈 시 호출
    public void SetOriginalScale()
    {
        transform.localScale = originalScale;
    }
}