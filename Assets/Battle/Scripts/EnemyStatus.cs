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

    public void Init(Enemy.EnemyInfo enemy)
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

        nameText.text = enemy.Name;
        ATKPowText.text = "<sprite Name=\"ATKPow\"> " + enemy.ATK;
        DEFPowText.text = "<sprite Name=\"DEFPow\"> " + enemy.DEF;
        healthText.text = enemy.CHP + "/" + enemy.MHP;
        healthRate.fillAmount = (float)enemy.CHP / enemy.MHP;
        stanceRate.fillAmount = (float)enemy.CSP / enemy.MSP;
        for (int i = 1; i < enemy.MSP; i++)
        {
            divideList.Add(Instantiate(prefabDivide, new Vector3(stanceRate.rectTransform.rect.width / enemy.MSP * i, 0, 0), Quaternion.identity));
            divideList[^1].transform.SetParent(stanceRate.transform, false);
        }
        for (int i = 0; i < enemy.currentEffectList.Count; i++)
        {
            effectList.Add(Instantiate(prefabEffect, transform.GetChild(0), false));
            effectList[^1].GetComponent<RectTransform>().anchoredPosition = Vector2.right * (430 + 50 * (effectList.Count - 1));
            effectList[^1].GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
            effectList[^1].GetComponent<Effect>().Init(enemy.currentEffectList[i].effectData, enemy.currentEffectList[i].effectSize);
        }
    }
}
