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
    [SerializeField] private List<PlayerData> playerDataList;
    [SerializeField] private int playerID;

    public class PlayerInfo
    {
        public int ID;
        public Sprite playerIcon;
        public string playerName;
        public int baseMHP;
        public int baseMSP;
        public int baseATK;
        public int baseDEF;
        public List<PlayerActionData> actionDataList;
        
        public int currentMHP;
        public int currentMSP;
        public int currentHP;
        public int currentSP;
        public int currentATK;
        public int currentDEF;
        public class CurrentEffect
        {
            public EffectData effectData;
            public int effectSize;
        }
        public List<CurrentEffect> currentEffectList;

        public PlayerInfo(PlayerData data)
        {
            ID = data.ID;
            playerIcon = data.playerIcon;
            playerName = data.playerName;
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

            currentEffectList = new List<CurrentEffect>();
        }
    }
    public PlayerInfo playerInfo;

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
        RunStart();
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

    public void RunStart()
    {
        playerID = 0;
        playerInfo = new PlayerInfo(playerDataList[playerID]);
    }
}