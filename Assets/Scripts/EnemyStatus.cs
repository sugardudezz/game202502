using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    public Image prefabScale;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ATKPowText;
    public TextMeshProUGUI DEFPowText;
    public TextMeshProUGUI healthText;
    public Image healthPoint;
    public Image stancePoint;
    public List<Image> pointScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pointScale = new List<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(Enemy enemy)
    {
        updateStatus(enemy);
        for (int i = 1; i < enemy.maxSP; i++)
        {
            pointScale.Add(Instantiate(prefabScale, new Vector3(stancePoint.rectTransform.rect.width / enemy.maxSP * i, 0, 0), Quaternion.identity));
            pointScale[^1].transform.SetParent(stancePoint.transform, false);
        }
    }

    public void updateStatus(Enemy enemy)
    {
        nameText.text = enemy.enemyName;
        ATKPowText.text = "<sprite=1> " + enemy.currentATK;
        DEFPowText.text = "<sprite=0> " + enemy.currentDEF;
        healthText.text = enemy.currentHP + "/" + enemy.maxHP;
        healthPoint.fillAmount = enemy.maxHP / enemy.currentHP;
        stancePoint.fillAmount = enemy.maxSP / enemy.currentSP;
    }
}
