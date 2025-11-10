using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Darkness Configuration")]
    public int darkness = 0;

    [Header("Darkness UI")]
    public GameObject darknessGaugeObject;
    public TextMeshProUGUI darknessValueText;

    void Start()
    {
        // 씬 시작 시 잠식도 UI 초기화 로직 호출
        InitializeDarknessUI();
    }

    private void InitializeDarknessUI()
    {
        int darknessValue = this.darkness;

        // 잠식도 활성화/비활성화 처리
        if (darknessGaugeObject != null)
        {
            // darkness 값이 0이면 비활성화, 1 이상이면 활성화
            bool isActive = darknessValue > 0;
            darknessGaugeObject.SetActive(isActive);
        }

        // 잠식도 텍스트 값 설정
        if (darknessValueText != null)
        {
            // darkness 값이 1, 2, 3일 때 해당 값으로 텍스트 설정
            if (darknessValue >= 1 && darknessValue <= 4)
            {
                darknessValueText.text = darknessValue.ToString();
            }
            // darknessValue가 0이거나 4 초과일 때는 표시 X
            else
            {
                darknessValueText.text = "";
            }
        }
    }

}