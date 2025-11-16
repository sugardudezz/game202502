using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
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

    public void Init(Player player)
    {
        nameText.text = player.playerName;
        ATKPowText.text = "<sprite=1> " + player.currentATK;
        DEFPowText.text = "<sprite=0> " + player.currentDEF;
        healthText.text = player.currentHP + "/" + player.maxHP;
        healthPoint.fillAmount = player.maxHP / player.currentHP;
        stancePoint.fillAmount = player.maxSP / player.currentSP;
        for (int i = 1; i < player.maxSP; i++)
        {
            pointScale.Add(Instantiate(prefabScale, new Vector3(stancePoint.rectTransform.rect.width / player.maxSP * i, 0, 0), Quaternion.identity));
            pointScale[^1].transform.SetParent(stancePoint.transform, false);
        }
    }
}
