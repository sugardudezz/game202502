using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject prefabDetailBar;

    public GameObject DetailBar;

    public List<EnemyActionData> actionDataList;

    public int actionID;
    public bool isEnhanced;
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

    public void Init(Enemy enemy)
    {
        actionDataList = enemy.actionDataList;

        AssignAction(actionDataList[0]);
    }

    public void AssignAction(EnemyActionData data)
    {
        actionID = data.ID;
        isEnhanced = data.isEnhanced;
        GetComponent<Image>().sprite = data.actionIcon;
        actionName = data.actionName;
        actionDesc = data.actionDesc;
    }

    public void Disable()
    {
        GetComponent<Image>().color = new Color(1, 1, 1, 0.4f);
        GetComponent<Button>().interactable = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DetailBar = Instantiate(prefabDetailBar, transform.root, false);
        DetailBar.transform.position = transform.position;
        DetailBar.GetComponent<RectTransform>().anchoredPosition += Vector2.down * 50;
        DetailBar.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = actionName + "\n" + actionDesc;

        StartCoroutine(ChasingBar());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(DetailBar);
    }

    IEnumerator ChasingBar()
    {
        while (DetailBar != null)
        {
            DetailBar.transform.position = transform.position;
            DetailBar.GetComponent<RectTransform>().anchoredPosition += Vector2.down * 52.5f;

            yield return null;
        }
    }
}
