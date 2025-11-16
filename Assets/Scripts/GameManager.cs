using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject scrollView;
    [SerializeField] private GameObject content;

    [SerializeField] private int playerDataID;
    [SerializeField] private List<PlayerData> playerDataList;
    [SerializeField] private int enemyDataID;
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

    [SerializeField] private List<GameObject> actionHolderList;
    [SerializeField] private List<GameObject> actionList;

    [SerializeField] private int playerAP;
    [SerializeField] private int enemyAP;

    [SerializeField] private GameObject turnSwitch;

    [SerializeField] private enum ActionID
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
        actionHolderList.Clear();
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
        scrollView.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Unrestricted;
        scrollView.GetComponent<ScrollRect>().vertical = false;
        foreach (var action in actionList)
        {
            action.GetComponent<Image>().raycastTarget = false;

            if (action.GetComponent<PlayerAction>() != null)
            {
                Destroy(action.GetComponent<PlayerAction>().DetailBar);
                Destroy(action.GetComponent<PlayerAction>().SelectBar);
            }
            else
            {
                Destroy(action.GetComponent<EnemyAction>().DetailBar);
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
        for (float t = 0; t < 1.0f; t += Time.deltaTime)
        {
            content.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(content.GetComponent<RectTransform>().anchoredPosition, Vector2.zero, t);
            yield return null;
        }
        for (int i = 0; i < actionList.Count; i += 2)
        {
            while (actionList[i].GetComponent<RectTransform>().anchoredPosition != actionList[i + 1].GetComponent<RectTransform>().anchoredPosition)
            {
                actionList[i].GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(actionList[i].GetComponent<RectTransform>().anchoredPosition, Vector2.zero, 1.0f);
                actionList[i + 1].GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(actionList[i + 1].GetComponent<RectTransform>().anchoredPosition, Vector2.zero, 1.0f);
                yield return null;
            }
            actionList[i].SetActive(false);
            actionList[i + 1].SetActive(false);
            Debug.Log("Test");

            yield return new WaitForSeconds(1.0f);

            for (float t = 0; t < 0.5f; t += Time.deltaTime)
            {
                content.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(content.GetComponent<RectTransform>().anchoredPosition, Vector2.zero, t);
                yield return null;
            }
        }
    }
}
