using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentLevel;
    
    public static event Action<string> OnSceneChange;

    [Header("Player Data Assign")]
    public List<PlayerData> playerDataList;
    public int playerID;

    [Header("Enemy Data Assign")]
    public List<EnemyData> enemyDataList;
    public int enemyID;

    public class PlayerInfo
    {
        public int ID;
        public Sprite Icon;
        public string Name;
        public int baseMHP;
        public int baseMSP;
        public int baseATK;
        public int baseDEF;
        public List<PlayerActionData> actionDataList;
        
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

        public PlayerInfo(PlayerData data)
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
    public PlayerInfo player;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ChangeScene("Map");
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene("Scenes/" + sceneName);
        OnSceneChange?.Invoke(sceneName);
    }

    public void InitializePlayer()
    {
        playerID = 0;
        player = new PlayerInfo(playerDataList[playerID]);
    }
}