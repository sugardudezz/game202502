using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject scrollView;
    [SerializeField] private GameObject content;

    [SerializeField] private int playerDataID;
    [SerializeField] private int enemyDataID;
    [SerializeField] private List<PlayerData> playerDataList;
    [SerializeField] private List<EnemyData> enemyDataList;

    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private GameObject prefabPlayerStatus;
    [SerializeField] private GameObject prefabEnemy;
    [SerializeField] private GameObject prefabEnemyStatus;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject playerStatusObject;
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private GameObject enemyStatusObject;

    [SerializeField] private Player player;
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyStatus enemyStatus;

    [SerializeField] private GameObject prefabActionHolder;
    [SerializeField] private GameObject prefabPlayerAction;
    [SerializeField] private GameObject prefabEnemyAction;

    [SerializeField] private int playerAP;
    [SerializeField] private int enemyAP;
    [SerializeField] private List<GameObject> actionHolderList;
    [SerializeField] private List<GameObject> actionList;

    [SerializeField] private GameObject prefabEffect;

    [SerializeField] private List<EffectData> effectDataList;
    [SerializeField] private List<EffectData> pCurrentEffect;
    [SerializeField] private List<EffectData> eCurrentEffect;
    [SerializeField] private List<Effect> pEffectList;
    [SerializeField] private List<Effect> eEffectList;

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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RunStart();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RunStart()
    {
        playerDataID = 0;
        playerObject = Instantiate(prefabPlayer, new Vector3(-5, 0, 0), Quaternion.identity);
        player = playerObject.GetComponent<Player>();
        player.Init(playerDataList[playerDataID]);

        playerStatusObject = Instantiate(prefabPlayerStatus, new Vector3(-500, -100, 0), Quaternion.identity);
        playerStatusObject.transform.SetParent(canvas.transform, false);
        playerStatus = playerStatusObject.GetComponent<PlayerStatus>();
        playerStatus.Init(player);



        BattleStart();
    }

    void BattleStart()
    {
        enemyDataID = Random.Range(0, enemyDataList.Count);
        enemyObject = Instantiate(prefabEnemy, new Vector3(5, 0, 0), Quaternion.identity);
        enemy = enemyObject.GetComponent<Enemy>();
        enemy.Init(enemyDataList[enemyDataID]);

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
        actionHolderList.Clear();
        foreach (GameObject action in actionList)
        {
            Destroy(action);
        }
        actionList.Clear();

        int playerMaxAP = player.maxSP;
        int enemyMaxAP = enemy.maxSP;
        playerAP = playerMaxAP;
        enemyAP = enemyMaxAP;
        for (int i = 0; i < Mathf.Max(playerMaxAP, enemyMaxAP); i++)
        {
            actionHolderList.Add(Instantiate(prefabActionHolder, content.transform, false));

            actionList.Add(Instantiate(prefabPlayerAction, new Vector3(-50, 0, 0), Quaternion.identity));
            actionList[^1].transform.SetParent(actionHolderList[^1].transform, false);
            actionList[^1].GetComponent<PlayerAction>().Init(player);
            if (i < playerMaxAP)
            {
                actionList[^1].GetComponent<PlayerAction>().OnActionAssigned += () =>
                {
                    Debug.Log(actionList[^1].GetComponent<PlayerAction>());
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
        for (int i = 0; i < actionHolderList.Count; i++)
        {
            Debug.Log("Player" + i + ": " + actionList[i * 2].GetComponent<PlayerAction>().actionName);
            Debug.Log("Enemy" + i + ": " + actionList[i * 2 + 1].GetComponent<EnemyAction>().actionName);
        }

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
            for (float t = 0; t < 0.5f; t += Time.deltaTime)
            {
                content.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(content.GetComponent<RectTransform>().anchoredPosition, Vector2.down * 100 * (i / 2), t);
                yield return null;
            }

            int pDamage = 0;
            int pDmgDef = 0;
            int eDamage = 0;
            int eDmgDef = 0;
            switch (actionList[i].GetComponent<PlayerAction>().actionID)
            {
                case (int)PlayerActionID.Attack:
                    pDamage += (int)(player.currentATK * 1.0f);
                    break;

                case (int)PlayerActionID.Charged_Attack:
                    pDamage += (int)(player.currentATK * 1.5f);
                    break;

                case (int)PlayerActionID.Defend:
                    pDmgDef += (int)(player.currentDEF * 1.0f);
                    break;

                case (int)PlayerActionID.Charged_Defend:
                    pDmgDef += (int)(player.currentDEF * 1.5f);
                    break;

                case (int)PlayerActionID.Special1:
                    break;

                case (int)PlayerActionID.Charged_Special1:
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
                    break;

                case (int)EnemyActionID.Charged_Special1:
                    break;
            }
            int pUsedAP = (!actionList[i].GetComponent<PlayerAction>().isEnhanced) ? 1 : 2;
            int eUsedAP = (!actionList[i + 1].GetComponent<EnemyAction>().isEnhanced) ? 1 : 2;
            int pStanceDamage = 0;
            int eStanceDamage = 0;
            if (pDamage > 0)
            {
                if (eDmgDef <= 0)
                {
                    pStanceDamage += pUsedAP + 1;
                }
                else if (eDmgDef < pDamage)
                {
                    pStanceDamage += pUsedAP;
                    eStanceDamage += eUsedAP;
                }
                else if (eDmgDef >= pDamage)
                {
                    eStanceDamage += eUsedAP + 1;
                }
            }
            if (eDamage > 0)
            {
                if (pDmgDef <= 0)
                {
                    eStanceDamage += eUsedAP + 1;
                }
                else if (pDmgDef < eDamage)
                {
                    eStanceDamage += eUsedAP;
                    pStanceDamage += pUsedAP;
                }
                else if (pDmgDef >= eDamage)
                {
                    pStanceDamage += pUsedAP + 1;
                }
            }
            yield return new WaitForSeconds(0.5f);

            while (actionList[i].GetComponent<RectTransform>().anchoredPosition != actionList[i + 1].GetComponent<RectTransform>().anchoredPosition)
            {
                actionList[i].GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(actionList[i].GetComponent<RectTransform>().anchoredPosition, Vector2.zero, 1.0f);
                actionList[i + 1].GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(actionList[i + 1].GetComponent<RectTransform>().anchoredPosition, Vector2.zero, 1.0f);
                yield return null;
            }
            actionList[i].SetActive(false);
            actionList[i + 1].SetActive(false);

            player.TakeDamage(Mathf.Max(0, eDamage - pDmgDef), eStanceDamage);
            enemy.TakeDamage(Mathf.Max(0, pDamage - eDmgDef), pStanceDamage);
            playerStatus.UpdateStatus(player);
            enemyStatus.UpdateStatus(enemy);

            bool pIsDead = player.currentHP <= 0;
            bool eIsDead = enemy.currentHP <= 0;
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
        }

        TurnStart();
    }
}
