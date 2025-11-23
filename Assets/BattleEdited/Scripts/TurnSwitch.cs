using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurnSwitch : MonoBehaviour, IPointerClickHandler
{
    public bool isPlanningTurn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isPlanningTurn = true;

        //GetComponent<Button>().onClick.AddListener(DisableSwitch);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isPlanningTurn = false;
    }

    public void DisableSwitch()
    {
        //isPlanningTurn = false;

        GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
        GetComponent<Button>().interactable = false;
    }

    public void EnableSwitch()
    {
        isPlanningTurn = true;

        GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        GetComponent<Button>().interactable = true;
    }
}
