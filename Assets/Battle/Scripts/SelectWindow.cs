using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isCursorOver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(PlayerAction action)
    {
        if (action.actionID == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                int idx = i;

                EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry();
                entry_PointerEnter.eventID = EventTriggerType.PointerEnter;
                entry_PointerEnter.callback.AddListener((data) => action.DemoAction(action.actionDataList[idx * 2 + 1]));
                transform.GetChild(idx).GetComponent<EventTrigger>().triggers.Add(entry_PointerEnter);

                EventTrigger.Entry entry_PointerExit = new EventTrigger.Entry();
                entry_PointerExit.eventID = EventTriggerType.PointerExit;
                entry_PointerExit.callback.AddListener((data) => action.UndoAction());
                transform.GetChild(idx).GetComponent<EventTrigger>().triggers.Add(entry_PointerExit);

                EventTrigger.Entry entry_PointerClick = new EventTrigger.Entry();
                entry_PointerClick.eventID = EventTriggerType.PointerClick;
                entry_PointerClick.callback.AddListener((data) => action.AssignAction(action.actionDataList[idx * 2 + 1]));
                transform.GetChild(idx).GetComponent<EventTrigger>().triggers.Add(entry_PointerClick);
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                int idx = i;

                if (action.actionID == idx * 2 + 1)
                {
                    EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry();
                    entry_PointerEnter.eventID = EventTriggerType.PointerEnter;
                    entry_PointerEnter.callback.AddListener((data) => action.DemoAction(action.actionDataList[idx * 2 + 2]));
                    transform.GetChild(idx).GetComponent<EventTrigger>().triggers.Add(entry_PointerEnter);

                    EventTrigger.Entry entry_PointerExit = new EventTrigger.Entry();
                    entry_PointerExit.eventID = EventTriggerType.PointerExit;
                    entry_PointerExit.callback.AddListener((data) => action.UndoAction());
                    transform.GetChild(idx).GetComponent<EventTrigger>().triggers.Add(entry_PointerExit);

                    EventTrigger.Entry entry_PointerClick = new EventTrigger.Entry();
                    entry_PointerClick.eventID = EventTriggerType.PointerClick;
                    entry_PointerClick.callback.AddListener((data) => action.AssignAction(action.actionDataList[idx * 2 + 2]));
                    transform.GetChild(idx).GetComponent<EventTrigger>().triggers.Add(entry_PointerClick);
                }
                else
                {
                    transform.GetChild(idx).GetComponent<Image>().color = new Color(1, 1, 1, 0.4f);
                    transform.GetChild(idx).GetComponent<Button>().interactable = false;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isCursorOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isCursorOver = false;
    }
}
