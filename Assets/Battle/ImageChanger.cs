using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    public Image objectAImage;
    public GameObject objectB;

    public void ChangeAImage(Sprite newSprite)
    {
        if (objectAImage != null && newSprite != null)
        {
            objectAImage.sprite = newSprite; // A의 이미지를 변경
        }

        if (objectB != null)
        {
            objectB.SetActive(false); // B(행동 선택창) 숨김
        }
    }
}