using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
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

    public void Init(Player player)
    {
        UpdateStatus(player);
    }

    public void UpdateStatus(Player player)
    {
        foreach (Image divide in rateDivide)
        {
            Destroy(divide);
        }
        rateDivide.Clear();

        nameText.text = player.playerName;
        ATKPowText.text = "<sprite=1> " + player.currentATK;
        DEFPowText.text = "<sprite=0> " + player.currentDEF;
        healthText.text = player.currentHP + "/" + player.maxHP;
        healthRate.fillAmount = (float)player.currentHP / player.maxHP;
        stanceRate.fillAmount = (float)player.currentSP / player.maxSP;
        for (int i = 1; i < player.maxSP; i++)
        {
            rateDivide.Add(Instantiate(prefabDivide, new Vector3(stanceRate.rectTransform.rect.width / player.maxSP * i, 0, 0), Quaternion.identity));
            rateDivide[^1].transform.SetParent(stanceRate.transform, false);
        }
    }
}
