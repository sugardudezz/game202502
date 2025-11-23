using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    public Image prefabDivide;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ATKPowText;
    public TextMeshProUGUI DEFPowText;
    public TextMeshProUGUI healthText;
    public Image healthRate;
    public Image stanceRate;
    public List<Image> rateDivide;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rateDivide = new List<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(Enemy enemy)
    {
        UpdateStatus(enemy);
    }

    public void UpdateStatus(Enemy enemy)
    {
        foreach (Image divide in rateDivide)
        {
            Destroy(divide);
        }
        rateDivide.Clear();

        nameText.text = enemy.enemyName;
        ATKPowText.text = "<sprite=1> " + enemy.currentATK;
        DEFPowText.text = "<sprite=0> " + enemy.currentDEF;
        healthText.text = enemy.currentHP + "/" + enemy.maxHP;
        healthRate.fillAmount = (float)enemy.currentHP / enemy.maxHP;
        stanceRate.fillAmount = (float)enemy.currentSP / enemy.maxSP;
        for (int i = 1; i < enemy.maxSP; i++)
        {
            rateDivide.Add(Instantiate(prefabDivide, new Vector3(stanceRate.rectTransform.rect.width / enemy.maxSP * i, 0, 0), Quaternion.identity));
            rateDivide[^1].transform.SetParent(stanceRate.transform, false);
        }
    }
}
