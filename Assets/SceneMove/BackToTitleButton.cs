using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class BackToTitleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string titleSceneName = "Title";
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f);
    //public float transitionSpeed = 0.1f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
