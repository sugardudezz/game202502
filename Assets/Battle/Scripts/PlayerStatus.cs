using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public GameObject prefabDivide;
    public GameObject prefabEffect;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ATKPowText;
    public TextMeshProUGUI DEFPowText;
    public TextMeshProUGUI healthText;
    public Image healthRate;
    public Image stanceRate;
    
    public List<GameObject> divideList;
    public List<GameObject> effectList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        divideList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(GameManager.PlayerInfo info)
    {
        foreach (var divide in divideList)
        {
            Destroy(divide);
        }
        foreach (var effect in effectList)
        {
            Destroy(effect);
        }
        divideList.Clear();
        effectList.Clear();

        nameText.text = info.playerName;
        ATKPowText.text = "<sprite=1> " + info.currentATK;
        DEFPowText.text = "<sprite=0> " + info.currentDEF;
        healthText.text = info.currentHP + "/" + info.currentMHP;
        healthRate.fillAmount = (float)info.currentHP / info.currentMHP;
        stanceRate.fillAmount = (float)info.currentSP / info.currentMSP;
        for (int i = 1; i < info.currentMSP; i++)
        {
            divideList.Add(Instantiate(prefabDivide, stanceRate.transform, false));
            divideList[^1].GetComponent<RectTransform>().anchoredPosition = new Vector2(stanceRate.rectTransform.rect.width / info.currentMSP * i, 0);
        }
        for (int i = 0; i < info.currentEffectList.Count; i++)
        {
            effectList.Add(Instantiate(prefabEffect, transform.GetChild(0), false));
            effectList[^1].GetComponent<RectTransform>().anchoredPosition = Vector2.right * (475 + 75 * (effectList.Count - 1));
            effectList[^1].GetComponent<Effect>().Init(info.currentEffectList[i].effectData, info.currentEffectList[i].effectSize);
        }
    }
}
