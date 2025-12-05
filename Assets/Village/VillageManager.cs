using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageManager : MonoBehaviour
{
    private Vector3 originalScale;
    public Vector3 hoverScale;

    // Start에서 원래 크기를 저장합니다.
    void Start()
    {
        GameManager.Instance.currentLevel = 0;
    }

    // 마우스 진입 시 호출
    public void SetHoverScale(GameObject target)
    {
        originalScale = target.transform.localScale;
        target.transform.localScale = hoverScale;
    }

    // 마우스 이탈 시 호출
    public void SetOriginalScale(GameObject target)
    {
        target.transform.localScale = originalScale;
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene("Scenes/" + sceneName);
    }
}
