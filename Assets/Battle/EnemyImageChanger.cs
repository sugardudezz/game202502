using UnityEngine;
using UnityEngine.UI;

public class EnemyImageChanger : MonoBehaviour
{
    private Image targetImage;

    // 변경 스프라이트 배열 - 0: 공격, 1: 방어, 2: 특화, 3: 잠식
    public Sprite[] changeSprites = new Sprite[4];

    void Awake()
    {
        targetImage = GetComponent<Image>();

        if (targetImage == null)
        {
            Debug.LogError("ImageReceiver 스크립트에는 Image 컴포넌트가 필요합니다.");
        }
    }

    public void ChangeImage(int index)
    {
        if (targetImage == null) return;

        // 인덱스가 유효한 범위(0, 1, 2, 3) 내에 있는지 확인
        if (index >= 0 && index < changeSprites.Length)
        {
            targetImage.sprite = changeSprites[index];
        }
        else
        {
            Debug.LogError("잘못된 이미지 인덱스입니다: " + index + ". 0, 1, 2, 3 중 하나를 사용해야 합니다.");
        }
    }
}