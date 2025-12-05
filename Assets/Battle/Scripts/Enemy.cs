using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public class EnemyInfo
    {
        public int ID;
        public Sprite Icon;
        public string Name;
        public int baseMHP;
        public int baseMSP;
        public int baseATK;
        public int baseDEF;
        public List<EnemyActionData> actionDataList;

        public int MHP;
        public int MSP;
        public int CHP;
        public int CSP;
        public int ATK;
        public int DEF;

        public class CurrentEffect
        {
            public EffectData effectData;
            public int effectSize;
        }
        public List<CurrentEffect> currentEffectList;

        public EnemyInfo(EnemyData data)
        {
            ID = data.ID;
            Icon = data.Icon;
            Name = data.Name;
            baseMHP = data.initialMHP;
            baseMSP = data.initialMSP;
            baseATK = data.initialATK;
            baseDEF = data.initialDEF;
            actionDataList = data.actionDataList;

            MHP = baseMHP;
            MSP = baseMSP;
            CHP = baseMHP;
            CSP = baseMSP;
            ATK = baseATK;
            DEF = baseDEF;

            currentEffectList = new List<CurrentEffect>();
        }
    }
    public EnemyInfo enemy;

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
        enemy = new EnemyInfo(data);

        GetComponent<SpriteRenderer>().sprite = enemy.Icon;

        AdjustStat();
    }

    public void AdjustStat()
    {
        enemy.MHP = enemy.baseMHP;
        enemy.MSP = enemy.baseMSP;
        enemy.CHP = Mathf.Min(enemy.CHP, enemy.MHP);
        enemy.CSP = Mathf.Min(enemy.CSP, enemy.MSP);
        enemy.ATK = enemy.baseATK;
        enemy.DEF = enemy.baseDEF;
    }

    public void AdjustStat(string statName, int size)
    {
        switch (statName)
        {
            case "MHP":
                enemy.baseMHP += size;
                enemy.CHP += size;
                break;
            case "MSP":
                enemy.baseMSP += size;
                enemy.CSP += size;
                break;
            case "ATK":
                enemy.baseATK += size;
                break;
            case "DEF":
                enemy.baseDEF += size;
                break;
        }
        enemy.MHP = enemy.baseMHP;
        enemy.MSP = enemy.baseMSP;
        enemy.CHP = Mathf.Min(enemy.CHP, enemy.MHP);
        enemy.CSP = Mathf.Min(enemy.CSP, enemy.MSP);
        enemy.ATK = enemy.baseATK;
        enemy.DEF = enemy.baseDEF;
    }

    public void TakeDamage(int damage, int stanceDamage)
    {
        enemy.CHP = Mathf.Max(0, enemy.CHP - damage);
        enemy.CSP = Mathf.Max(0, enemy.CSP - stanceDamage);
    }

    public void TakeCuring(int curing)
    {
        enemy.CHP = Mathf.Min(enemy.CHP + curing, enemy.MHP);
    }

    public void TakeEffect(EffectData effectData, int effectSize)
    {
        enemy.currentEffectList.Add(new EnemyInfo.CurrentEffect());
        enemy.currentEffectList[^1].effectData = effectData;
        enemy.currentEffectList[^1].effectSize = effectSize;
    }
}
