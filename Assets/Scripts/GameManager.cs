using System;
using Map;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentLevel = 0;
    
    public static event Action<string> OnSceneChange;
    
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }

    public void ChangeScene(string sceneName)
    {
        sceneName = "Scenes/" + sceneName;
        SceneManager.LoadScene(sceneName);
        OnSceneChange?.Invoke(sceneName);
    }
}