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

    public void Init(Player player)
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

        nameText.text = player.playerName;
        ATKPowText.text = "<sprite=1> " + player.currentATK;
        DEFPowText.text = "<sprite=0> " + player.currentDEF;
        healthText.text = player.currentHP + "/" + player.maxHP;
        healthRate.fillAmount = (float)player.currentHP / player.maxHP;
        stanceRate.fillAmount = (float)player.currentSP / player.maxSP;
        for (int i = 1; i < player.maxSP; i++)
        {
            divideList.Add(Instantiate(prefabDivide, stanceRate.transform, false));
            divideList[^1].GetComponent<RectTransform>().anchoredPosition = new Vector2(stanceRate.rectTransform.rect.width / player.maxSP * i, 0);
        }
        for (int i = 0; i < player.currEffectList.Count; i++)
        {
            effectList.Add(Instantiate(prefabEffect, transform.GetChild(0), false));
            effectList[^1].GetComponent<RectTransform>().anchoredPosition = Vector2.right * (475 + 75 * (effectList.Count - 1));
            effectList[^1].GetComponent<Effect>().Init(player.currEffectList[i].effectData, player.currEffectList[i].effectSize);
        }
    }
}
