using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int ID;
    public Sprite enemyIcon;
    public string enemyName;
    public int baseHP;
    public int baseSP;
    public int baseATK;
    public int baseDEF;

    public List<EnemyActionData> actionDataList;

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

    public void Init(EnemyData data)
    {
        ID = data.ID;
        enemyIcon = data.enemyIcon;
        enemyName = data.enemyName;
        baseHP = data.baseHP;
        baseSP = data.baseSP;
        baseATK = data.baseATK;
        baseDEF = data.baseDEF;
        GetComponent<SpriteRenderer>().sprite = enemyIcon;

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
