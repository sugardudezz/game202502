using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager.PlayerInfo player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        player = GameManager.Instance.player;

        GetComponent<SpriteRenderer>().sprite = player.Icon;

        AdjustStat();
    }

    public void AdjustStat()
    {
        player.MHP = player.baseMHP;
        player.MSP = player.baseMSP;
        player.CHP = Mathf.Min(player.CHP, player.MHP);
        player.CSP = Mathf.Min(player.CSP, player.MSP);
        player.ATK = player.baseATK;
        player.DEF = player.baseDEF;
    }

    public void AdjustStat(string statName, int size)
    {
        switch (statName)
        {
            case "MHP":
                player.baseMHP += size;
                player.CHP += size;
                break;
            case "MSP":
                player.baseMSP += size;
                player.CSP += size;
                break;
            case "ATK":
                player.baseATK += size;
                break;
            case "DEF":
                player.baseDEF += size;
                break;
        }
        player.MHP = player.baseMHP;
        player.MSP = player.baseMSP;
        player.CHP = Mathf.Min(player.CHP, player.MHP);
        player.CSP = Mathf.Min(player.CSP, player.MSP);
        player.ATK = player.baseATK;
        player.DEF = player.baseDEF;
    }

    public void TakeDamage(int damage, int stanceDamage)
    {
        player.CHP = Mathf.Max(0, player.CHP - damage);
        player.CSP = Mathf.Max(0, player.CSP - stanceDamage);
    }

    public void TakeCuring(int curing)
    {
        player.CHP = Mathf.Min(player.CHP + curing, player.MHP);
    }

    public void TakeEffect(EffectData effectData, int effectSize)
    {
        player.currentEffectList.Add(new GameManager.PlayerInfo.CurrentEffect());
        player.currentEffectList[^1].effectData = effectData;
        player.currentEffectList[^1].effectSize = effectSize;
    }
}
