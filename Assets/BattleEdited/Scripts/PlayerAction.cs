using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject prefabDetailBar;
    public GameObject prefabSelectBar;

    public GameObject DetailBar;
    public GameObject SelectBar;

    public List<PlayerActionData> actionDataList;

    public int actionID;
    public bool isEnhanced;
    public Sprite actionIcon;
    public string actionName;
    public string actionDesc;

    public PlayerActionData originData;

    public bool isEnabled;

    public event Action OnActionAssigned;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(Player player)
    {
        isEnabled = true;

        actionDataList = player.actionDataList;

        AssignAction(actionDataList[0]);
    }

    public void AssignAction(PlayerActionData data)
    {
        actionID = data.ID;
        isEnhanced = data.isEnhanced;
        GetComponent<Image>().sprite = data.actionIcon;
        actionName = data.actionName;
        actionDesc = data.actionDesc;

        originData = data;

        OnActionAssigned?.Invoke();

        if (SelectBar != null)
        {
            Destroy(DetailBar);
            Destroy(SelectBar);

            if (isEnabled)
            {
                ShowSelectBar();
                ExecuteEvents.Execute(SelectBar, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
            }
        }
    }

    public void DemoAction(PlayerActionData data)
    {
        GetComponent<Image>().sprite = data.actionIcon;
        actionID = data.ID;
        actionName = data.actionName;
        actionDesc = data.actionDesc;

        ShowDetailBar();
    }

    public void UndoAction()
    {
        GetComponent<Image>().sprite = originData.actionIcon;
        actionID = originData.ID;
        actionName = originData.actionName;
        actionDesc = originData.actionDesc;

        Destroy(DetailBar);
    }

    public void ShowDetailBar()
    {
        DetailBar = Instantiate(prefabDetailBar, transform.root, false);
        DetailBar.transform.position = transform.position;
        DetailBar.GetComponent<RectTransform>().anchoredPosition += Vector2.down * 52.5f;
        DetailBar.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = actionName + "\n" + actionDesc;
    }

    public void ShowSelectBar()
    {
        if (SelectBar != null)
        {
            Destroy(SelectBar);
        }
        SelectBar = Instantiate(prefabSelectBar, transform.root, false);
        SelectBar.transform.position = transform.position;
        SelectBar.transform.SetSiblingIndex(GetComponentInParent<ScrollRect>().transform.GetSiblingIndex());
        SelectBar.transform.GetComponent<SelectBar>().Init(this);
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

            Destroy(DetailBar);
            Destroy(SelectBar);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowDetailBar();

        if (isEnabled)
        {
            GetComponentsInParent<Image>()[1].raycastTarget = false;

            ShowSelectBar();
        }

        StartCoroutine(ChasingBar());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(DetailBar);

        if (isEnabled)
        {
            StartCoroutine(DelayedCheck());
        }
    }

    IEnumerator ChasingBar()
    {
        while (DetailBar != null || SelectBar != null)
        {
            if (DetailBar != null)
            {
                DetailBar.transform.position = transform.position;
                DetailBar.GetComponent<RectTransform>().anchoredPosition += Vector2.down * 52.5f;
            }
            if (SelectBar != null)
            {
                SelectBar.transform.position = transform.position;
            }
            yield return null;
        }
    }

    IEnumerator DelayedCheck()
    {
        yield return null;

        if (SelectBar != null)
        {
            if (SelectBar.GetComponent<SelectBar>().isCursorOver)
            {
                yield return new WaitWhile(() => (SelectBar != null) ? SelectBar.GetComponent<SelectBar>().isCursorOver : false);
            }
        }
        GetComponentsInParent<Image>()[1].raycastTarget = true;

        Destroy(SelectBar);
    }
}
