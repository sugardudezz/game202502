using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [Header("UI Element Assign")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject scrollView;
    [SerializeField] private GameObject content;
    [SerializeField] private List<Sprite> backgroundList;
    [SerializeField] private GameObject background;

    [Header("Player/Enemy Info Container")]
    [SerializeField] private GameManager.PlayerInfo player;
    [SerializeField] private Enemy.EnemyInfo enemy;

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
    [SerializeField] private Player playerScript;
    [SerializeField] private PlayerStatus playerStatusScript;
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private EnemyStatus enemyStatusScript;

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

    [Header("Result Assign")]
    [SerializeField] private GameObject result;

    public Vector3 originalScale;
    public Vector3 hoverScale;

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
        Charged_Special1,
        Special2,
        Charged_Special2,
        Special3,
        Charged_Special3
    }
    [SerializeField] private enum EffectID
    {
        Break,
        Attack_Buff,
        Defend_Buff,
        ATK_Buff
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.Instance.player;

        background.GetComponent<Image>().sprite = backgroundList[GameManager.Instance.currentLevel - 1];

        result.SetActive(false);

        BattleStart();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void BattleStart()
    {
        playerObject = Instantiate(prefabPlayer, new Vector3(-5f, 1f, 0), Quaternion.identity);
        playerScript = playerObject.GetComponent<Player>();
        playerScript.Init();

        playerStatusObject = Instantiate(prefabPlayerStatus, new Vector3(-500, -150, 0), Quaternion.identity);
        playerStatusObject.transform.SetParent(canvas.transform, false);
        playerStatusScript = playerStatusObject.GetComponent<PlayerStatus>();
        playerStatusScript.Init(player);

        enemyObject = Instantiate(prefabEnemy, new Vector3(5f, 1, 0), Quaternion.identity);
        enemyScript = enemyObject.GetComponent<Enemy>();
        enemyScript.Init(GameManager.Instance.enemyDataList[GameManager.Instance.currentLevel - 1]);
        enemy = enemyScript.enemy;

        enemyStatusObject = Instantiate(prefabEnemyStatus, new Vector3(500, -150, 0), Quaternion.identity);
        enemyStatusObject.transform.SetParent(canvas.transform, false);
        enemyStatusScript = enemyStatusObject.GetComponent<EnemyStatus>();
        enemyStatusScript.Init(enemy);

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

        playerScript.AdjustStat();
        enemyScript.AdjustStat();

        for (int i = player.currentEffectList.Count - 1; i >= 0; i--)
        {
            if (player.currentEffectList[i].effectData.ID == (int)EffectID.Break)
            {
                player.currentEffectList.RemoveAt(i);
                continue;
            }
        }
        for (int i = enemy.currentEffectList.Count - 1; i >= 0; i--)
        {
            if (enemy.currentEffectList[i].effectData.ID == (int)EffectID.Break)
            {
                enemy.currentEffectList.RemoveAt(i);
                continue;
            }
            if (enemy.currentEffectList[i].effectData.ID == (int)EffectID.ATK_Buff)
            {
                enemy.ATK += enemy.currentEffectList[i].effectSize;
            }
        }
        player.CSP = player.MSP;
        enemy.CSP = enemy.MSP;

        playerStatusScript.Init(player);
        enemyStatusScript.Init(enemy);

        int playerMaxAP = player.MSP;
        int enemyMaxAP = enemy.MSP;
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

    // 마우스 진입 시 호출
    public void SetHoverScale(GameObject target)
    {
        originalScale = target.transform.localScale;
        target.transform.localScale = hoverScale;
    }

    // 마우스 이탈 시 호출
    public void SetOriginalScale(GameObject target)
    {
        target.transform.localScale = originalScale;
    }

    IEnumerator PlanningTurn()
    {
        bool isAPZero = false;

        while (turnSwitch.GetComponent<TurnSwitch>().isPlanningTurn)
        {
            for (int i = 0; i < actionList.Count; i += 2)
            {
                int t = player.MSP - playerAP;
                for (int j = 0; j < actionList.Count; j += 2)
                {
                    if (actionList[j].GetComponent<PlayerAction>().isEnhanced)
                    {
                        t -= 1;
                    }
                }
                if (i / 2 <= t)
                {
                    actionList[i].GetComponent<PlayerAction>().Enable();
                }
                else
                {
                    actionList[i].GetComponent<PlayerAction>().Disable();
                }
            }
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

            playerScript.AdjustStat();
            enemyScript.AdjustStat();

            for (int j = player.currentEffectList.Count - 1; j >= 0; j--)
            {
                if (player.currentEffectList[j].effectSize == 0)
                {
                    if (player.currentEffectList[j].effectData.ID == (int)EffectID.Break)
                    {
                        player.CSP = player.MSP;
                    }
                    player.currentEffectList.RemoveAt(j);
                }
                else
                {
                    if (player.currentEffectList[j].effectData.ID == (int)EffectID.Break)
                    {
                        pTakenDamageRate += 0.5f;
                        player.currentEffectList[j].effectSize -= 1;
                    }
                }
            }
            for (int j = enemy.currentEffectList.Count - 1; j >= 0; j--)
            {
                if (enemy.currentEffectList[j].effectSize == 0)
                {
                    if (enemy.currentEffectList[j].effectData.ID == (int)EffectID.Break)
                    {
                        enemy.CSP = enemy.MSP;
                    }
                    enemy.currentEffectList.RemoveAt(j);
                }
                else
                {
                    if (enemy.currentEffectList[j].effectData.ID == (int)EffectID.Break)
                    {
                        eTakenDamageRate += 0.5f;
                        enemy.currentEffectList[j].effectSize -= 1;
                    }
                    if (enemy.currentEffectList[j].effectData.ID == (int)EffectID.ATK_Buff)
                    {
                        enemy.ATK += enemy.currentEffectList[j].effectSize;
                    }
                }
            }

            playerStatusScript.Init(player);
            enemyStatusScript.Init(enemy);

            yield return new WaitForSeconds(0.25f);

            switch (actionList[i].GetComponent<PlayerAction>().actionID)
            {
                case (int)PlayerActionID.Attack:
                    pDamage += Mathf.RoundToInt(player.ATK * 1.0f);
                    break;

                case (int)PlayerActionID.Charged_Attack:
                    pDamage += Mathf.RoundToInt(player.ATK * 1.5f);
                    break;

                case (int)PlayerActionID.Defend:
                    pDmgDef += Mathf.RoundToInt(player.DEF * 1.0f);
                    while (player.currentEffectList.Exists(item => item.effectData.ID == (int)EffectID.Defend_Buff))
                    {
                        pDmgDef += Mathf.RoundToInt(player.DEF * (player.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Defend_Buff).effectSize * 0.01f));
                        player.currentEffectList.Remove(player.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Defend_Buff));
                    }
                    break;

                case (int)PlayerActionID.Charged_Defend:
                    pDmgDef += Mathf.RoundToInt(player.DEF * 1.5f);
                    while (player.currentEffectList.Exists(item => item.effectData.ID == (int)EffectID.Defend_Buff))
                    {
                        pDmgDef += Mathf.RoundToInt(player.DEF * (player.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Defend_Buff).effectSize * 0.01f));
                        player.currentEffectList.Remove(player.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Defend_Buff));
                    }
                    break;

                case (int)PlayerActionID.Special1:
                    playerScript.TakeEffect(effectDataList[(int)EffectID.Defend_Buff], 100);
                    break;

                case (int)PlayerActionID.Charged_Special1:
                    pIsPiercing = true;
                    pDamage = Mathf.RoundToInt(player.ATK * 1.0f);
                    break;
            }
            switch (actionList[i + 1].GetComponent<EnemyAction>().actionID)
            {
                case (int)EnemyActionID.Attack:
                    eDamage += Mathf.RoundToInt(enemy.ATK * 1.0f);
                    while (enemy.currentEffectList.Exists(item => item.effectData.ID == (int)EffectID.Attack_Buff))
                    {
                        eDamage += Mathf.RoundToInt(enemy.ATK * (enemy.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Attack_Buff).effectSize * 0.01f));
                        enemy.currentEffectList.Remove(enemy.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Attack_Buff));
                    }
                    break;

                case (int)EnemyActionID.Charged_Attack:
                    eDamage += Mathf.RoundToInt(enemy.ATK * 1.5f);
                    while (enemy.currentEffectList.Exists(item => item.effectData.ID == (int)EffectID.Attack_Buff))
                    {
                        eDamage += Mathf.RoundToInt(enemy.ATK * (enemy.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Attack_Buff).effectSize * 0.01f));
                        enemy.currentEffectList.Remove(enemy.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Attack_Buff));
                    }
                    break;

                case (int)EnemyActionID.Defend:
                    eDmgDef += Mathf.RoundToInt(enemy.DEF * 1.0f);
                    while (enemy.currentEffectList.Exists(item => item.effectData.ID == (int)EffectID.Defend_Buff))
                    {
                        eDmgDef += Mathf.RoundToInt(enemy.DEF * (enemy.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Defend_Buff).effectSize * 0.01f));
                        enemy.currentEffectList.Remove(enemy.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Defend_Buff));
                    }
                    break;

                case (int)EnemyActionID.Charged_Defend:
                    eDmgDef += Mathf.RoundToInt(enemy.DEF * 1.5f);
                    while (enemy.currentEffectList.Exists(item => item.effectData.ID == (int)EffectID.Defend_Buff))
                    {
                        eDmgDef += Mathf.RoundToInt(enemy.DEF * (enemy.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Defend_Buff).effectSize * 0.01f));
                        enemy.currentEffectList.Remove(enemy.currentEffectList.Find(item => item.effectData.ID == (int)EffectID.Defend_Buff));
                    }
                    break;

                case (int)EnemyActionID.Special1:
                    eCuring += 2;
                    break;

                case (int)EnemyActionID.Charged_Special1:
                    eCuring += 4;
                    break;

                case (int)EnemyActionID.Special2:
                    enemyScript.TakeEffect(effectDataList[(int)EffectID.Attack_Buff], 50);
                    break;

                case (int)EnemyActionID.Charged_Special2:
                    enemyScript.TakeEffect(effectDataList[(int)EffectID.ATK_Buff], 2);
                    break;

                case (int)EnemyActionID.Special3:
                    enemyScript.TakeEffect(effectDataList[(int)EffectID.Defend_Buff], 100);
                    break;

                case (int)EnemyActionID.Charged_Special3:
                    pIsPiercing = true;
                    eDamage = Mathf.RoundToInt(enemy.ATK * 1.0f);
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

            pDamage = Mathf.RoundToInt((pDamage * pGivenDamageRate) * eTakenDamageRate);
            eDamage = Mathf.RoundToInt((eDamage * eGivenDamageRate) * pTakenDamageRate);
            playerScript.TakeDamage(eDamage, eStanceDamage);
            enemyScript.TakeDamage(pDamage, pStanceDamage);

            if (player.CSP <= 0)
            {
                if (!player.currentEffectList.Exists(item => item.effectData.ID == (int)EffectID.Break))
                {
                    playerScript.TakeEffect(effectDataList[(int)EffectID.Break], 1);
                    if (actionList[i + 2] != null)
                    {
                        actionList[i + 2].GetComponent<PlayerAction>().AssignAction(player.actionDataList[0]);
                        actionList[i + 2].GetComponent<PlayerAction>().Disable();
                    }
                }
            }
            if (enemy.CSP <= 0)
            {
                if (!enemy.currentEffectList.Exists(item => item.effectData.ID == (int)EffectID.Break))
                {
                    enemyScript.TakeEffect(effectDataList[(int)EffectID.Break], 1);
                    if (actionList[i + 3] != null)
                    {
                        actionList[i + 3].GetComponent<EnemyAction>().AssignAction(enemy.actionDataList[0]);
                        actionList[i + 3].GetComponent<EnemyAction>().Disable();
                    }
                }
            }

            bool pIsDead = player.CHP <= 0;
            bool eIsDead = enemy.CHP <= 0;
            if (!pIsDead)
            {
                playerScript.TakeCuring(pCuring);
            }
            if (!eIsDead)
            {
                enemyScript.TakeCuring(eCuring);
            }

            playerStatusScript.Init(player);
            enemyStatusScript.Init(enemy);

            if (pIsDead || eIsDead)
            {
                scrollView.SetActive(false);
                result.SetActive(true);
                EventTrigger.Entry entry_PointerClick = new EventTrigger.Entry();
                entry_PointerClick.eventID = EventTriggerType.PointerClick;
                if (pIsDead)
                {
                    //게임오버 효과
                    result.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "패배";
                    entry_PointerClick.callback.AddListener((data) => GameManager.Instance.ChangeScene("Village"));
                }
                else if (eIsDead)
                {
                    //적 처치 효과
                    switch ((GameManager.Instance.currentLevel - 1) % 3)
                    {
                        case 0:
                            playerScript.AdjustStat("DEF", 1);
                            break;
                        case 1:
                            playerScript.AdjustStat("DEF", 1);
                            break;
                        case 2:
                            playerScript.AdjustStat("MSP", 1);
                            break;
                    }
                    playerStatusScript.Init(player);

                    result.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "승리";
                    if (GameManager.Instance.currentLevel < 12)
                        entry_PointerClick.callback.AddListener((data) => GameManager.Instance.ChangeScene("Map"));
                    else
                        entry_PointerClick.callback.AddListener((data) => GameManager.Instance.ChangeScene("Ending"));
                }
                result.transform.GetChild(1).GetComponent<EventTrigger>().triggers.Add(entry_PointerClick);
                yield break;
            }

            List<GameObject> pActions = actionList.Where((item, i) => item.activeSelf && i % 2 == 0).ToList();
            List<GameObject> eActions = actionList.Where((item, i) => item.activeSelf && i % 2 == 1).ToList();
            bool pIsRemainEmpty = pActions.All(action => action.GetComponent<PlayerAction>().actionID == (int)PlayerActionID.Empty);
            bool eIsRemainEmpty = eActions.All(action => action.GetComponent<EnemyAction>().actionID == (int)EnemyActionID.Empty);
            if (pIsRemainEmpty && eIsRemainEmpty)
            {
                break;
            }
        }

        yield return new WaitForSeconds(0.5f);

        TurnStart();
    }
}