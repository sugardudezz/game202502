using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager.PlayerInfo playerInfo;
    //public int ID;
    //public Sprite playerIcon;
    //public string playerName;
    //public int baseMHP;
    //public int baseMSP;
    //public int baseATK;
    //public int baseDEF;
    //public List<PlayerActionData> actionDataList;

    //public int currentMHP;
    //public int currentMSP;
    //public int currentHP;
    //public int currentSP;
    //public int currentATK;
    //public int currentDEF;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(GameManager.PlayerInfo info)
    {
        playerInfo = info;

        GetComponent<SpriteRenderer>().sprite = playerInfo.playerIcon;
    }

    public void TakeDamage(int damage, int stanceDamage)
    {
        playerInfo.currentHP = Mathf.Max(0, playerInfo.currentHP - damage);
        playerInfo.currentSP = Mathf.Max(0, playerInfo.currentSP - stanceDamage);
    }

    public void TakeCuring(int curing)
    {
        playerInfo.currentHP = Mathf.Min(playerInfo.currentHP + curing, playerInfo.currentMHP);
    }

    public void TakeEffect(EffectData effectData, int effectSize)
    {
        playerInfo.currentEffectList.Add(new GameManager.PlayerInfo.CurrentEffect());
        playerInfo.currentEffectList[^1].effectData = effectData;
        playerInfo.currentEffectList[^1].effectSize = effectSize;
    }
}
