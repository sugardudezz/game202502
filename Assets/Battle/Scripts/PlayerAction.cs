using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject prefabDetailWindow;
    public GameObject prefabSelectWindow;

    public GameObject DetailWindow;
    public GameObject SelectWindow;

    public List<PlayerActionData> actionDataList;

    public int actionID;
    public bool isEnhanced;
    public Sprite actionIcon;
    public string actionName;
    public string actionDesc;

    public PlayerActionData originData;

    public bool isEnabled;

    public event Action OnActionAssigned;
    public static event Action<PlayerActionData> OnActionAssignedStatic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(GameManager.PlayerInfo info)
    {
        isEnabled = true;

        actionDataList = info.actionDataList;

        AssignAction(actionDataList[0]);
    }

    public void AssignAction(PlayerActionData data)
    {
        actionID = data.ID;
        isEnhanced = data.isEnhanced;
        actionIcon = data.actionIcon;
        actionName = data.actionName;
        actionDesc = data.actionDesc;
        originData = data;

        GetComponent<Image>().sprite = actionIcon;

        OnActionAssigned?.Invoke();
        OnActionAssignedStatic?.Invoke(data);

        if (SelectWindow)
        {
            Destroy(DetailWindow);
            Destroy(SelectWindow);

            if (isEnabled)
            {
                ShowSelectWindow();
                ExecuteEvents.Execute(SelectWindow, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
            }
        }
    }

    public void DemoAction(PlayerActionData data)
    {
        actionID = data.ID;
        actionIcon = data.actionIcon;
        actionName = data.actionName;
        actionDesc = data.actionDesc;
        GetComponent<Image>().sprite = actionIcon;

        ShowDetailWindow();
    }

    public void UndoAction()
    {
        actionID = originData.ID;
        actionIcon = originData.actionIcon;
        actionName = originData.actionName;
        actionDesc = originData.actionDesc;
        GetComponent<Image>().sprite = actionIcon;

        Destroy(DetailWindow);
    }

    public void ShowDetailWindow()
    {
        DetailWindow = Instantiate(prefabDetailWindow, transform.root, false);
        DetailWindow.transform.position = transform.position;
        DetailWindow.GetComponent<RectTransform>().anchoredPosition += Vector2.down * 52.5f;
        DetailWindow.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = actionName + "\n" + actionDesc;
    }

    public void ShowSelectWindow()
    {
        if (SelectWindow != null)
        {
            Destroy(SelectWindow);
        }
        SelectWindow = Instantiate(prefabSelectWindow, transform.root, false);
        SelectWindow.transform.position = transform.position;
        SelectWindow.transform.SetSiblingIndex(GetComponentInParent<ScrollRect>().transform.GetSiblingIndex());
        SelectWindow.transform.GetComponent<SelectWindow>().Init(this);
    }

    public void Enable()
    {
        isEnabled = true;

        GetComponent<Image>().color = new Color(1, 1, 1, 1f);

        GetComponent<Button>().interactable = true;
    }

    public void Disable()
    {
        if (isEnabled)
        {
            isEnabled = false;

            if (actionID == 0)
            {
                GetComponent<Image>().color = new Color(1, 1, 1, 0.4f);
            }
            GetComponent<Button>().interactable = false;

            Destroy(DetailWindow);
            Destroy(SelectWindow);
        }
    }

    public static event Action OnUIHover;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowDetailWindow();

        if (isEnabled)
        {
            GetComponentsInParent<Image>()[1].raycastTarget = false;

            ShowSelectWindow();
        }

        StartCoroutine(FollowAction());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(DetailWindow);

        if (isEnabled)
        {
            StartCoroutine(DelayedCheck());
        }
    }

    IEnumerator FollowAction()
    {
        while (DetailWindow != null || SelectWindow != null)
        {
            if (DetailWindow != null)
            {
                DetailWindow.transform.position = transform.position;
                DetailWindow.GetComponent<RectTransform>().anchoredPosition += Vector2.down * 52.5f;
            }
            if (SelectWindow != null)
            {
                SelectWindow.transform.position = transform.position;
            }
            yield return null;
        }
    }

    IEnumerator DelayedCheck()
    {
        yield return null;

        if (SelectWindow != null)
        {
            if (SelectWindow.GetComponent<SelectWindow>().isCursorOver)
            {
                yield return new WaitWhile(() => (SelectWindow != null) ? SelectWindow.GetComponent<SelectWindow>().isCursorOver : false);
            }
        }
        GetComponentsInParent<Image>()[1].raycastTarget = true;

        Destroy(SelectWindow);
    }
}
