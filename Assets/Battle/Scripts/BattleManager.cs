using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [Header("UI Element Assign")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject scrollView;
    [SerializeField] private GameObject content;

    [Header("Enemy Data Assign")]
    [SerializeField] private List<EnemyData> enemyDataList;
    [SerializeField] private int enemyID;

    [Header("Player/Enemy Icon&Status Prefab Assign")]
    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private GameObject prefabPlayerStatus;
    [SerializeField] private GameObject prefabEnemy;
    [SerializeField] private GameObject prefabEnemyStatus;

    [Header("Player/Enemy Icon&Status GameObject")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject playerStatusObject;
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private GameObject enemyStatusObject;

    [Header("Player/Enemy Script")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyStatus enemyStatus;

    [Header("Action Holder/Action Prefab Assign")]
    [SerializeField] private GameObject prefabActionHolder;
    [SerializeField] private GameObject prefabPlayerAction;
    [SerializeField] private GameObject prefabEnemyAction;

    [Header("Action Holder/Action GameObject List")]
    [SerializeField] private List<GameObject> actionHolderList;
    [SerializeField] private List<GameObject> actionList;
    [SerializeField] private int playerAP;
    [SerializeField] private int enemyAP;

    [Header("Effect Data Assign")]
    [SerializeField] private List<EffectData> effectDataList;

    [Header("Turn Switch Assign")]
    [SerializeField] private GameObject turnSwitch;

    [SerializeField] private enum PlayerActionID
    {
        Empty,
        Attack,
        Charged_Attack,
        Defend,
        Charged_Defend,
        Special1,
        Charged_Special1
    }
    [SerializeField] private enum EnemyActionID
    {
        Empty,
        Attack,
        Charged_Attack,
        Defend,
        Charged_Defend,
        Special1,
        Charged_Special1
    }
    [SerializeField] private enum EffectID
    {
        Break,
        Reinforce_Defend
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BattleStart();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //void RunStart()
    //{
    //    playerObject = Instantiate(prefabPlayer, new Vector3(-5, 0, 0), Quaternion.identity);
    //    player = playerObject.GetComponent<Player>();
    //    player.Init(GameManager.Instance.playerInfo);

    //    playerStatusObject = Instantiate(prefabPlayerStatus, new Vector3(-500, -100, 0), Quaternion.identity);
    //    playerStatusObject.transform.SetParent(canvas.transform, false);
    //    playerStatus = playerStatusObject.GetComponent<PlayerStatus>();
    //    playerStatus.Init(player);



    //    BattleStart();
    //}

    void BattleStart()
    {
        playerObject = Instantiate(prefabPlayer, new Vector3(-5, 0, 0), Quaternion.identity);
        player = playerObject.GetComponent<Player>();
        player.Init(GameManager.Instance.playerInfo);

        playerStatusObject = Instantiate(prefabPlayerStatus, new Vector3(-500, -100, 0), Quaternion.identity);
        playerStatusObject.transform.SetParent(canvas.transform, false);
        playerStatus = playerStatusObject.GetComponent<PlayerStatus>();
        playerStatus.Init(player.playerInfo);

        enemyID = GameManager.Instance.currentLevel;
        enemyObject = Instantiate(prefabEnemy, new Vector3(5, 0, 0), Quaternion.identity);
        enemy = enemyObject.GetComponent<Enemy>();
        enemy.Init(enemyDataList[enemyID]);

        enemyStatusObject = Instantiate(prefabEnemyStatus, new Vector3(500, -100, 0), Quaternion.identity);
        enemyStatusObject.transform.SetParent(canvas.transform, false);
        enemyStatus = enemyStatusObject.GetComponent<EnemyStatus>();
        enemyStatus.Init(enemy);

        TurnStart();
    }

    void TurnStart()
    {
        turnSwitch.GetComponent<TurnSwitch>().EnableSwitch();
        scrollView.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Elastic;
        scrollView.GetComponent<ScrollRect>().vertical = true;
        content.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        foreach (GameObject actionHolder in actionHolderList)
        {
            Destroy(actionHolder);
        }
        foreach (GameObject action in actionList)
        {
            Destroy(action);
        }
        actionHolderList.Clear();
        actionList.Clear();
        for (int i = player.playerInfo.currentEffectList.Count - 1; i >= 0; i--)
        {
            if (player.playerInfo.currentEffectList[i].effectData.ID == (int)EffectID.Break)
            {
                player.playerInfo.currentEffectList.RemoveAt(i);
            }
        }
        for (int i = enemy.currEffectList.Count - 1; i >= 0; i--)
        {
            if (enemy.currEffectList[i].effectData.ID == (int)EffectID.Break)
            {
                enemy.currEffectList.RemoveAt(i);
            }
        }
        player.playerInfo.currentSP = player.playerInfo.currentMSP;
        enemy.currentSP = enemy.currentMSP;

        int playerMaxAP = player.playerInfo.currentMSP;
        int enemyMaxAP = enemy.currentMSP;
        playerAP = playerMaxAP;
        enemyAP = enemyMaxAP;
        for (int i = 0; i < Mathf.Max(playerMaxAP, enemyMaxAP); i++)
        {
            actionHolderList.Add(Instantiate(prefabActionHolder, content.transform, false));

            actionList.Add(Instantiate(prefabPlayerAction, new Vector3(-50, 0, 0), Quaternion.identity));
            actionList[^1].transform.SetParent(actionHolderList[^1].transform, false);
            actionList[^1].GetComponent<PlayerAction>().Init(player.playerInfo);
            if (i < playerMaxAP)
            {
                actionList[^1].GetComponent<PlayerAction>().OnActionAssigned += () =>
                {
                    playerAP -= 1;
                };
            }
            else
            {
                actionList[^1].SetActive(false);
            }

            actionList.Add(Instantiate(prefabEnemyAction, new Vector3(50, 0, 0), Quaternion.identity));
            actionList[^1].transform.SetParent(actionHolderList[^1].transform, false);
            actionList[^1].GetComponent<EnemyAction>().Init(enemy);
            if (i < enemyMaxAP)
            {
                if (enemyAP > 0)
                {
                    switch (enemy.ID)
                    {
                        default:
                            int idx;
                            do
                            {
                                idx = Random.Range(1, enemy.actionDataList.Count);
                            }
                            while (enemy.actionDataList[idx].isEnhanced && enemyAP < 2);

                            actionList[^1].GetComponent<EnemyAction>().AssignAction(enemy.actionDataList[idx]);

                            if (actionList[^1].GetComponent<EnemyAction>().isEnhanced)
                            {
                                enemyAP -= 2;
                            }
                            else
                            {
                                enemyAP -= 1;
                            }
                            break;
                    }
                }
                else
                {
                    actionList[^1].GetComponent<EnemyAction>().Disable();
                }
            }
            else
            {
                actionList[^1].SetActive(false);
            }
        }
        playerStatus.Init(player.playerInfo);
        enemyStatus.Init(enemy);

        StartCoroutine(PlanningTurn());
    }

    void TurnEnd()
    {
        turnSwitch.GetComponent<TurnSwitch>().DisableSwitch();
        scrollView.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Unrestricted;
        scrollView.GetComponent<ScrollRect>().vertical = false;
        foreach (var action in actionList)
        {
            action.GetComponent<Image>().raycastTarget = false;

            if (action.GetComponent<PlayerAction>() != null)
            {
                Destroy(action.GetComponent<PlayerAction>().DetailWindow);
                Destroy(action.GetComponent<PlayerAction>().SelectWindow);
            }
            else
            {
                Destroy(action.GetComponent<EnemyAction>().DetailWindow);
            }
        }

        StartCoroutine(ClashingTurn());
    }

    void BattleEnd()
    {

    }

    void RunEnd()
    {

    }

    IEnumerator PlanningTurn()
    {
        bool isAPZero = false;

        while (turnSwitch.GetComponent<TurnSwitch>().isPlanningTurn)
        {
            if (playerAP <= 0)
            {
                isAPZero = true;
            }
            if (isAPZero)
            {
                for (int i = 0; i < actionHolderList.Count; i++)
                {
                    if (actionList[i * 2] != null)
                    {
                        actionList[i * 2].GetComponent<PlayerAction>().Disable();
                    }
                }
            }
            yield return null;
        }

        TurnEnd();
    }

    IEnumerator ClashingTurn()
    {
        for (float t = 0; t < 0.5f; t += Time.deltaTime)
        {
            content.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(content.GetComponent<RectTransform>().anchoredPosition, Vector2.up * 100, t);
            yield return null;
        }

        for (int i = 0; i < actionList.Count; i += 2)
        {
            bool pIsPiercing = false;
            bool eIsPiercing = false;
            int pDamage = 0;
            int pDmgDef = 0;
            int eDamage = 0;
            int eDmgDef = 0;
            float pGivenDamageRate = 1.0f;
            float pTakenDamageRate = 1.0f;
            float eGivenDamageRate = 1.0f;
            float eTakenDamageRate = 1.0f;
            int pCuring = 0;
            int eCuring = 0;

            for (int j = player.playerInfo.currentEffectList.Count - 1; j >= 0; j--)
            {
                if (player.playerInfo.currentEffectList[j].effectSize == 0)
                {
                    if (player.playerInfo.currentEffectList[j].effectData.ID == (int)EffectID.Break)
                    {
                        player.playerInfo.currentSP = player.playerInfo.currentMSP;
                    }
                    player.playerInfo.currentEffectList.RemoveAt(j);
                }
                else
                {
                    if (player.playerInfo.currentEffectList[j].effectData.ID == (int)EffectID.Break)
                    {
                        pTakenDamageRate += 0.5f;
                        player.playerInfo.currentEffectList[j].effectSize -= 1;
                    }
                }
            }

            for (int j = enemy.currEffectList.Count - 1; j >= 0; j--)
            {
                if (enemy.currEffectList[j].effectSize == 0)
                {
                    if (enemy.currEffectList[j].effectData.ID == (int)EffectID.Break)
                    {
                        enemy.currentSP = enemy.currentMSP;
                    }
                    enemy.currEffectList.RemoveAt(j);
                }
                else
                {
                    if (enemy.currEffectList[j].effectData.ID == (int)EffectID.Break)
                    {
                        eTakenDamageRate += 0.5f;
                        enemy.currEffectList[j].effectSize -= 1;
                    }
                }
            }

            playerStatus.Init(player.playerInfo);
            enemyStatus.Init(enemy);

            switch (actionList[i].GetComponent<PlayerAction>().actionID)
            {
                case (int)PlayerActionID.Attack:
                    pDamage += (int)(player.playerInfo.currentATK * 1.0f);
                    break;

                case (int)PlayerActionID.Charged_Attack:
                    pDamage += (int)(player.playerInfo.currentATK * 1.5f);
                    break;

                case (int)PlayerActionID.Defend:
                    pDmgDef += (int)(player.playerInfo.currentDEF * 1.0f);
                    if (player.playerInfo.currentEffectList.Exists(item => item.effectData.ID == (int)EffectID.Reinforce_Defend))
                    {
                        pDmgDef += (int)(player.playerInfo.currentDEF * (player.playerInfo.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Reinforce_Defend).effectSize * 0.01));
                        player.playerInfo.currentEffectList.Remove(player.playerInfo.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Reinforce_Defend));
                    }
                    break;

                case (int)PlayerActionID.Charged_Defend:
                    pDmgDef += (int)(player.playerInfo.currentDEF * 1.5f);
                    if (player.playerInfo.currentEffectList.Exists(item => item.effectData.ID == (int)EffectID.Reinforce_Defend))
                    {
                        pDmgDef += (int)(player.playerInfo.currentDEF * (player.playerInfo.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Reinforce_Defend).effectSize * 0.01));
                        player.playerInfo.currentEffectList.Remove(player.playerInfo.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Reinforce_Defend));
                    }
                    break;

                case (int)PlayerActionID.Special1:
                    player.TakeEffect(effectDataList[(int)EffectID.Reinforce_Defend], 75);
                    break;

                case (int)PlayerActionID.Charged_Special1:
                    pIsPiercing = true;
                    pDamage = (int)(player.playerInfo.currentATK * 1.0f);
                    break;
            }
            switch (actionList[i + 1].GetComponent<EnemyAction>().actionID)
            {
                case (int)EnemyActionID.Attack:
                    eDamage += (int)(enemy.currentATK * 1.0f);
                    break;

                case (int)EnemyActionID.Charged_Attack:
                    eDamage += (int)(enemy.currentATK * 1.5f);
                    break;

                case (int)EnemyActionID.Defend:
                    eDmgDef += (int)(enemy.currentDEF * 1.0f);
                    break;

                case (int)EnemyActionID.Charged_Defend:
                    eDmgDef += (int)(enemy.currentDEF * 1.5f);
                    break;

                case (int)EnemyActionID.Special1:
                    eCuring += 4;
                    break;

                case (int)EnemyActionID.Charged_Special1:
                    eCuring += 6;
                    break;
            }
            int pUsedAP = (!actionList[i].GetComponent<PlayerAction>().isEnhanced) ? 1 : 2;
            int eUsedAP = (!actionList[i + 1].GetComponent<EnemyAction>().isEnhanced) ? 1 : 2;
            int pStanceDamage = 0;
            int eStanceDamage = 0;
            if (pDamage > 0)
            {
                if (!pIsPiercing)
                {
                    if (eDmgDef <= 0)
                    {
                        pStanceDamage += pUsedAP + 1;
                    }
                    else if (eDmgDef < pDamage)
                    {
                        pDamage -= eDmgDef;
                        pStanceDamage += pUsedAP;
                        eStanceDamage += eUsedAP;
                    }
                    else if (eDmgDef >= pDamage)
                    {
                        pDamage = 0;
                        eStanceDamage += eUsedAP + 1;
                    }
                }
                else
                {
                    pStanceDamage += pUsedAP + 1;
                }
            }
            if (eDamage > 0 && !eIsPiercing)
            {
                if (!eIsPiercing)
                {
                    if (pDmgDef <= 0)
                    {
                        eStanceDamage += eUsedAP + 1;
                    }
                    else if (pDmgDef < eDamage)
                    {
                        eDamage -= pDmgDef;
                        eStanceDamage += eUsedAP;
                        pStanceDamage += pUsedAP;
                    }
                    else if (pDmgDef >= eDamage)
                    {
                        eDamage = 0;
                        pStanceDamage += pUsedAP + 1;
                    }
                }
                else
                {
                    eStanceDamage = eUsedAP + 1;
                }
            }

            for (float t = 0; t < 0.25f; t += Time.deltaTime)
            {
                content.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(content.GetComponent<RectTransform>().anchoredPosition, Vector2.down * 100 * (i / 2), t);
                yield return null;
            }
            yield return new WaitForSeconds(0.25f);

            while (actionList[i].GetComponent<RectTransform>().anchoredPosition != actionList[i + 1].GetComponent<RectTransform>().anchoredPosition)
            {
                actionList[i].GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(actionList[i].GetComponent<RectTransform>().anchoredPosition, Vector2.zero, 1.0f);
                actionList[i + 1].GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(actionList[i + 1].GetComponent<RectTransform>().anchoredPosition, Vector2.zero, 1.0f);
                yield return null;
            }
            actionList[i].SetActive(false);
            actionList[i + 1].SetActive(false);

            pDamage = (int)((pDamage * pGivenDamageRate) * eTakenDamageRate);
            eDamage = (int)((eDamage * eGivenDamageRate) * pTakenDamageRate);
            player.TakeDamage(eDamage, eStanceDamage);
            enemy.TakeDamage(pDamage, pStanceDamage);

            if (player.playerInfo.currentSP <= 0)
            {
                if (!player.playerInfo.currentEffectList.Exists(item => item.effectData.ID == (int)EffectID.Break))
                {
                    player.TakeEffect(effectDataList[(int)EffectID.Break], 1);
                    if (actionList[i + 2] != null)
                    {
                        actionList[i + 2].GetComponent<PlayerAction>().AssignAction(player.playerInfo.actionDataList[0]);
                        actionList[i + 2].GetComponent<PlayerAction>().Disable();
                    }
                }
            }
            if (enemy.currentSP <= 0)
            {
                if (!enemy.currEffectList.Exists(item => item.effectData.ID == (int)EffectID.Break))
                {
                    enemy.TakeEffect(effectDataList[(int)EffectID.Break], 1);
                    if (actionList[i + 3] != null)
                    {
                        actionList[i + 3].GetComponent<EnemyAction>().AssignAction(enemy.actionDataList[0]);
                        actionList[i + 3].GetComponent<EnemyAction>().Disable();
                    }
                }
            }

            bool pIsDead = player.playerInfo.currentHP <= 0;
            bool eIsDead = enemy.currentHP <= 0;
            if (!pIsDead)
            {
                player.TakeCuring(pCuring);
            }
            if (!eIsDead)
            {
                enemy.TakeCuring(eCuring);
            }

            playerStatus.Init(player.playerInfo);
            enemyStatus.Init(enemy);

            if (pIsDead || eIsDead)
            {
                if (pIsDead)
                {
                    //게임오버 효과
                }
                if (eIsDead)
                {
                    //적 처치 효과
                }
                yield break;
            }

            yield return new WaitForSeconds(0.5f);

            List<GameObject> pActions = actionList.Where((item, i) => item.activeSelf && i % 2 == 0).ToList();
            List<GameObject> eActions = actionList.Where((item, i) => item.activeSelf && i % 2 == 1).ToList();
            bool pIsRemainEmpty = pActions.All(action => action.GetComponent<PlayerAction>().actionID == (int)PlayerActionID.Empty);
            bool eIsRemainEmpty = eActions.All(action => action.GetComponent<EnemyAction>().actionID == (int)EnemyActionID.Empty);
            if (pIsRemainEmpty && eIsRemainEmpty)
            {
                break;
            }
        }

        TurnStart();
    }
}