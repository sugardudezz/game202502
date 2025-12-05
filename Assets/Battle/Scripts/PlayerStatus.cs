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

    private void Awake()
    {
        divideList = new List<GameObject>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(GameManager.PlayerInfo player)
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

        nameText.text = player.Name;
        ATKPowText.text = "<sprite Name=\"ATKPow\"> " + player.ATK;
        DEFPowText.text = "<sprite Name=\"DEFPow\"> " + player.DEF;
        healthText.text = player.CHP + "/" + player.MHP;
        healthRate.fillAmount = (float)player.CHP / player.MHP;
        stanceRate.fillAmount = (float)player.CSP / player.MSP;
        for (int i = 1; i < player.MSP; i++)
        {
            divideList.Add(Instantiate(prefabDivide, stanceRate.transform, false));
            divideList[^1].GetComponent<RectTransform>().anchoredPosition = new Vector2(stanceRate.rectTransform.rect.width / player.MSP * i, 0);
        }
        for (int i = 0; i < player.currentEffectList.Count; i++)
        {
            effectList.Add(Instantiate(prefabEffect, transform.GetChild(0), false));
            effectList[^1].GetComponent<RectTransform>().anchoredPosition = Vector2.right * (430 + 50 * (effectList.Count - 1));
            effectList[^1].GetComponent<Effect>().Init(player.currentEffectList[i].effectData, player.currentEffectList[i].effectSize);
        }
    }
}
