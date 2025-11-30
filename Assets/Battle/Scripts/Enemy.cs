using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int ID;
    public Sprite enemyIcon;
    public string enemyName;
    public int baseMHP;
    public int baseMSP;
    public int baseATK;
    public int baseDEF;
    public List<EnemyActionData> actionDataList;

    public int currentMHP;
    public int currentMSP;
    public int currentHP;
    public int currentSP;
    public int currentATK;
    public int currentDEF;

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
        baseMHP = data.baseMHP;
        baseMSP = data.baseMSP;
        baseATK = data.baseATK;
        baseDEF = data.baseDEF;
        actionDataList = data.actionDataList;

        currentMHP = baseMHP;
        currentMSP = baseMSP;
        currentHP = baseMHP;
        currentSP = baseMSP;
        currentATK = baseATK;
        currentDEF = baseDEF;

        currEffectList = new List<CurrEffect>();

        GetComponent<SpriteRenderer>().sprite = enemyIcon;
    }

    public void TakeDamage(int damage, int stanceDamage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
        currentSP = Mathf.Max(0, currentSP - stanceDamage);
    }

    public void TakeCuring(int curing)
    {
        currentHP = Mathf.Min(currentHP + curing, currentMHP);
    }

    public void TakeEffect(EffectData effectData, int effectSize)
    {
        currEffectList.Add(new CurrEffect());
        currEffectList[^1].effectData = effectData;
        currEffectList[^1].effectSize = effectSize;
    }
}
