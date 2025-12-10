using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject prefabDetailWindow;

    public GameObject DetailWindow;

    public List<EnemyActionData> actionDataList;

    public int actionID;
    public bool isEnhanced;
    public Sprite actionIcon;
    public string actionName;
    public string actionDesc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(Enemy.EnemyInfo enemy)
    {
        actionDataList = enemy.actionDataList;

        AssignAction(actionDataList[0]);
    }

    public void AssignAction(EnemyActionData data)
    {
        actionID = data.ID;
        isEnhanced = data.isEnhanced;
        actionIcon = data.actionIcon;
        actionName = data.actionName;
        actionDesc = data.actionDesc;

        GetComponent<Image>().sprite = actionIcon;
    }

    public void Disable()
    {
        GetComponent<Image>().color = new Color(1, 1, 1, 0.4f);
        GetComponent<Button>().interactable = false;
    }
    
    public static event Action OnUIHover;

    public void OnPointerEnter(PointerEventData eventData)
    {
        DetailWindow = Instantiate(prefabDetailWindow, transform.root, false);
        DetailWindow.transform.position = transform.position;
        DetailWindow.GetComponent<RectTransform>().anchoredPosition += Vector2.down * 50;
        DetailWindow.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = actionName + "\n" + actionDesc;

        StartCoroutine(FollowAction());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(DetailWindow);
    }

    IEnumerator FollowAction()
    {
        while (DetailWindow != null)
        {
            DetailWindow.transform.position = transform.position;
            DetailWindow.GetComponent<RectTransform>().anchoredPosition += Vector2.down * 52.5f;

            yield return null;
        }
    }
}
