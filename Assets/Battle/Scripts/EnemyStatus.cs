using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
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

    public void Init(Enemy enemy)
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

        nameText.text = enemy.enemyName;
        ATKPowText.text = "<sprite=1> " + enemy.currentATK;
        DEFPowText.text = "<sprite=0> " + enemy.currentDEF;
        healthText.text = enemy.currentHP + "/" + enemy.currentMHP;
        healthRate.fillAmount = (float)enemy.currentHP / enemy.currentMHP;
        stanceRate.fillAmount = (float)enemy.currentSP / enemy.currentMSP;
        for (int i = 1; i < enemy.currentMSP; i++)
        {
            divideList.Add(Instantiate(prefabDivide, new Vector3(stanceRate.rectTransform.rect.width / enemy.currentMSP * i, 0, 0), Quaternion.identity));
            divideList[^1].transform.SetParent(stanceRate.transform, false);
        }
        for (int i = 0; i < enemy.currEffectList.Count; i++)
        {
            effectList.Add(Instantiate(prefabEffect, transform.GetChild(0), false));
            effectList[^1].GetComponent<RectTransform>().anchoredPosition = Vector2.right * (475 + 75 * (effectList.Count - 1));
            effectList[^1].GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
            effectList[^1].GetComponent<Effect>().Init(enemy.currEffectList[i].effectData, enemy.currEffectList[i].effectSize);
        }
    }
}
