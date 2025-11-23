using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int ID;
    public Sprite playerIcon;
    public string playerName;
    public int baseHP;
    public int baseSP;
    public int baseATK;
    public int baseDEF;

    public List<PlayerActionData> actionDataList;

    public int maxHP;
    public int maxSP;
    public int currentHP;
    public int currentSP;
    public int currentATK;
    public int currentDEF;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(PlayerData data)
    {
        ID = data.ID;
        playerIcon = data.playerIcon;
        playerName = data.playerName;
        baseHP = data.baseHP;
        baseSP = data.baseSP;
        baseATK = data.baseATK;
        baseDEF = data.baseDEF;
        GetComponent<SpriteRenderer>().sprite = playerIcon;

        actionDataList = data.actionDataList;

        maxHP = baseHP;
        maxSP = baseSP;
        currentHP = maxHP;
        currentSP = maxSP;
        currentATK = baseATK;
        currentDEF = baseDEF;
    }

    public void TakeDamage(int damage, int stanceDamage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
        currentSP = Mathf.Max(0, currentSP - stanceDamage);
    }
}
