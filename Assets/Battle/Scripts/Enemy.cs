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

    public int maxHP;
    public int maxSP;
    public int currentHP;
    public int currentSP;
    public int currentATK;
    public int currentDEF;

    public List<EnemyActionData> actionDataList;

    public class CurrEffect
    {
        public EffectData effectData;
        public int effectSize;
    }
    public List<CurrEffect> currEffectList;

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

        maxHP = baseHP;
        maxSP = baseSP;
        currentHP = maxHP;
        currentSP = maxSP;
        currentATK = baseATK;
        currentDEF = baseDEF;

        actionDataList = data.actionDataList;

        currEffectList = new List<CurrEffect>();
    }

    public void TakeDamage(int damage, int stanceDamage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
        currentSP = Mathf.Max(0, currentSP - stanceDamage);
    }

    public void TakeCuring(int curing)
    {
        currentHP = Mathf.Min(currentHP + curing, maxHP);
    }

    public void TakeEffect(EffectData effectData, int effectSize)
    {
        currEffectList.Add(new CurrEffect());
        currEffectList[^1].effectData = effectData;
        currEffectList[^1].effectSize = effectSize;
    }
}
