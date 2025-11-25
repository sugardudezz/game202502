using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentLevel;
    
    public static event Action<string> OnSceneChange;
    
    void Awake()
    {
        if (instance != null) {
            Destroy(this.gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene("Scenes/" + sceneName);
        OnSceneChange?.Invoke(sceneName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            ChangeScene("Map");
        }
    }
}